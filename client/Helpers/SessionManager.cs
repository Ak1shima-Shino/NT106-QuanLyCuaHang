using System;
using RetailClient.DTO;

namespace RetailClient.Helpers
{
    public static class SessionManager
    {
        public static string AccessToken { get; private set; }

        public static UserDto CurrentUser { get; private set; }

        public static DateTime? ExpiredAt { get; private set; }

        public static void SaveSession(
            string accessToken,
            UserDto user,
            int expiresIn)
        {
            AccessToken = accessToken;
            CurrentUser = user;
            ExpiredAt = DateTime.Now.AddSeconds(expiresIn);
        }

        public static void UpdateUser(UserDto user)
        {
            CurrentUser = user;
        }

        public static void Clear()
        {
            AccessToken = null;
            CurrentUser = null;
            ExpiredAt = null;
        }
    }
}
