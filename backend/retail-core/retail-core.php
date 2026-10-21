<?php

/**
 * Plugin Name: Retail Core
 * Description: Backend 
 * Version: 1.0.0
 * Author: NT106 Team
 */

if (!defined('ABSPATH')) {
    exit;
}

require_once __DIR__ . '/vendor/autoload.php';

function retail_core_activate()
{

    add_role('retail_customer', 'Customer', [
        'read' => true,
    ]);

    add_role('retail_staff', 'Staff', [
        'read' => true,
        'retail_view_orders' => true,
    ]);

    add_role('retail_manager', 'Manager', [
        'read' => true,
        'retail_view_orders' => true,
        'retail_manage_orders' => true,
    ]);
}

register_activation_hook(
    __FILE__,
    'retail_core_activate'
);

add_action('wp_set_password', function ($password, $user_id) {
    $version = (int) get_user_meta(
        $user_id,
        'retail_jwt_version',
        true
    );

    update_user_meta(
        $user_id,
        'retail_jwt_version',
        $version + 1
    );
}, 10, 2);


add_action('rest_api_init', function () {

    register_rest_route('retail/v1', '/register', [
        'methods' => 'POST',
        'callback' => 'retail_core_register_customer',
        'permission_callback' => '__return_true',
    ]);

    register_rest_route('retail/v1', '/login', [
        'methods' => 'POST',
        'callback' => 'retail_core_login',
        'permission_callback' => '__return_true',
    ]);

    register_rest_route('retail/v1', '/me', [
        'methods' => 'GET',
        'callback' => 'retail_core_me',
        'permission_callback' => 'retail_core_require_jwt',
    ]);

    register_rest_route('retail/v1', '/logout', [
        'methods' => 'POST',
        'callback' => 'retail_core_logout',
        'permission_callback' => 'retail_core_require_jwt',
    ]);

    register_rest_route('retail/v1', '/forgot-password', [
        'methods' => 'POST',
        'callback' => 'retail_core_forgot_password',
        'permission_callback' => '__return_true',
    ]);

    register_rest_route('retail/v1', '/reset-password', [
        'methods' => 'POST',
        'callback' => 'retail_core_reset_password',
        'permission_callback' => '__return_true',
    ]);
});


function retail_core_register_customer(WP_REST_Request $request)
{
    $data = $request->get_json_params();

    $username = $data['username'] ?? null;
    $email = $data['email'] ?? null;
    $password = $data['password'] ?? null;

    if (
        !is_string($username) ||
        !is_string($email) ||
        !is_string($password)
    ) {
        return new WP_Error('invalid_input', 'Du lieu khong hop le', [
            'status' => 400
        ]);
    }

    $username = trim($username);
    $email = trim($email);

    if (
        !validate_username($username) ||
        !is_email($email) ||
        strlen($password) < 8
    ) {
        return new WP_Error('invalid_input', 'Thong tin dang ky khong hop le', [
            'status' => 400
        ]);
    }

    $user_id = wp_insert_user([
        'user_login' => $username,
        'user_email' => $email,
        'user_pass' => $password,
        'role' => 'retail_customer'
    ]);

    if (is_wp_error($user_id)) {
        return new WP_Error('registration_failed', $user_id->get_error_message(), [
            'status' => 400
        ]);
    }

    return new WP_REST_Response([
        'success' => true,
        'user_id' => $user_id
    ], 201);
}


function retail_core_login(WP_REST_Request $request)
{
    $data = $request->get_json_params();

    $username = $data['username'] ?? null;
    $password = $data['password'] ?? null;

    if (
        !is_string($username) ||
        trim($username) === '' ||
        !is_string($password) ||
        $password === ''
    ) {
        return new WP_Error(
            'invalid_input',
            'Du lieu dang nhap khong hop le',
            ['status' => 400]
        );
    }

    $user = wp_authenticate(trim($username), $password);

    if (is_wp_error($user)) {
        return new WP_Error(
            'invalid_credentials',
            'Ten dang nhap hoac mat khau khong dung',
            ['status' => 401]
        );
    }


    if (
        !defined('RETAIL_JWT_SECRET') ||
        strlen(RETAIL_JWT_SECRET) < 32
    ) {
        return new WP_Error(
            'jwt_config_error',
            'JWT chua duoc cau hinh',
            ['status' => 500]
        );
    }

    $now = time();

    $payload = [
        'iss' => home_url(),
        'sub' => (string) $user->ID,
        'iat' => $now,
        'exp' => $now + 3600,
        'jti' => bin2hex(random_bytes(16)),
        'ver' => (int) get_user_meta($user->ID, 'retail_jwt_version', true),
    ];

    $token = \Firebase\JWT\JWT::encode(
        $payload,
        RETAIL_JWT_SECRET,
        'HS256'
    );

    return new WP_REST_Response([
        'success' => true,
        'user_id' => $user->ID,
        'access_token' => $token,
        'token_type' => 'Bearer',
        'expires_in' => 3600
    ], 200);
}

function retail_core_require_jwt(WP_REST_Request $request)
{
    $authorization = $request->get_header('authorization');

    if (
        !is_string($authorization) ||
        !preg_match('/^Bearer ([^\s]+)$/i', $authorization, $matches)
    ) {
        return new WP_Error(
            'missing_token',
            'Thieu Bearer token',
            ['status' => 401]
        );
    }

    if (
        !defined('RETAIL_JWT_SECRET') ||
        !is_string(RETAIL_JWT_SECRET) ||
        strlen(RETAIL_JWT_SECRET) < 32
    ) {
        return new WP_Error(
            'jwt_config_error',
            'JWT chua duoc cau hinh',
            ['status' => 500]
        );
    }

    try {
        $claims = \Firebase\JWT\JWT::decode(
            $matches[1],
            new \Firebase\JWT\Key(RETAIL_JWT_SECRET, 'HS256')
        );
    } catch (\Exception $e) {
        return new WP_Error(
            'invalid_token',
            'Token khong hop le hoac da het han',
            ['status' => 401]
        );
    }

    if (
        !isset($claims->iss, $claims->sub) ||
        $claims->iss !== home_url() ||
        !is_string($claims->sub) ||
        !ctype_digit($claims->sub) ||
        (int) $claims->sub < 1
    ) {
        return new WP_Error(
            'invalid_token',
            'Token khong hop le',
            ['status' => 401]
        );
    }

    $user = get_user_by('id', (int) $claims->sub);

    if (!$user) {
        return new WP_Error(
            'invalid_token',
            'Tai khoan khong ton tai',
            ['status' => 401]
        );
    }

    $current_version = (int) get_user_meta(
        $user->ID,
        'retail_jwt_version',
        true
    );

    if (
        !isset($claims->ver) ||
        !is_int($claims->ver) ||
        $claims->ver !== $current_version
    ) {
        return new WP_Error(
            'revoked_token',
            'Token da bi thu hoi',
            ['status' => 401]
        );
    }

    wp_set_current_user($user->ID);
    return true;
}

function retail_core_me()
{
    $user = wp_get_current_user();

    return new WP_REST_Response([
        'success' => true,
        'user' => [
            'id' => $user->ID,
            'username' => $user->user_login,
            'email' => $user->user_email,
            'roles' => $user->roles,
        ],
    ], 200);
}

function retail_core_logout()
{
    $user_id = get_current_user_id();
    $version = (int) get_user_meta(
        $user_id,
        'retail_jwt_version',
        true
    );

    update_user_meta(
        $user_id,
        'retail_jwt_version',
        $version + 1
    );

    return new WP_REST_Response([
        'success' => true,
        'message' => 'Da dang xuat',
    ], 200);
}

function retail_core_forgot_password(WP_REST_Request $request)
{
    $data = $request->get_json_params();
    $email = $data['email'] ?? null;

    if (!is_string($email) || !is_email(trim($email))) {
        return new WP_Error(
            'invalid_input',
            'Email khong hop le',
            ['status' => 400]
        );
    }

    $email = trim($email);

    $response = new WP_REST_Response([
        'success' => true,
        'message' => 'Neu email ton tai, huong dan khoi phuc se duoc gui',
    ], 200);

    // Tránh gửi email liên tục cho cùng một địa chỉ.
    $limit_key = 'retail_reset_' . md5(strtolower($email));

    if (get_transient($limit_key)) {
        return $response;
    }

    set_transient($limit_key, 1, MINUTE_IN_SECONDS);

    $user = get_user_by('email', $email);

    if (!$user) {
        return $response;
    }

    $key = get_password_reset_key($user);

    if (is_wp_error($key)) {
        error_log('Retail Core: khong tao duoc ma khoi phuc');
        return $response;
    }

    $message = "Tai khoan: {$user->user_login}\n"
        . "Ma khoi phuc: {$key}\n\n"
        . "Nhap ma nay trong ung dung de dat mat khau moi.";

    if (!wp_mail($user->user_email, 'Khoi phuc mat khau', $message)) {
        error_log('Retail Core: khong gui duoc email khoi phuc');
    }

    return $response;
}

function retail_core_reset_password(WP_REST_Request $request)
{
    $data = $request->get_json_params();

    $username = $data['username'] ?? null;
    $key = $data['key'] ?? null;
    $new_password = $data['new_password'] ?? null;

    if (
        !is_string($username) || trim($username) === '' ||
        !is_string($key) || $key === '' ||
        !is_string($new_password) || strlen($new_password) < 8
    ) {
        return new WP_Error(
            'invalid_input',
            'Du lieu dat lai mat khau khong hop le',
            ['status' => 400]
        );
    }

    $user = check_password_reset_key($key, trim($username));

    if (is_wp_error($user)) {
        return new WP_Error(
            'invalid_reset_key',
            'Ma khoi phuc khong hop le hoac da het han',
            ['status' => 400]
        );
    }

    reset_password($user, $new_password);

    return new WP_REST_Response([
        'success' => true,
        'message' => 'Da dat lai mat khau',
    ], 200);
}
