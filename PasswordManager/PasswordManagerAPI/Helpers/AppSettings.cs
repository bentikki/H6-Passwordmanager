namespace PasswordManagerAPI.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string PepperStringValue { get; set; }
        public int RefreshTokenTTL { get; set; }
    }
}