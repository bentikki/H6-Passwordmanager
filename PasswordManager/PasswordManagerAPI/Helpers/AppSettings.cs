﻿namespace PasswordManagerAPI.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string PepperStringValue { get; set; }

        // refresh token time to live (in days), inactive tokens are
        // automatically deleted from the database after this time
        public int RefreshTokenTTL { get; set; }
    }
}