namespace SuperariLife.Model.Settings
{
    public class AppSettings
    {
        public string? JWT_Secret { get; set; }
        public string? CustomerKey { get; set; }
        public int JWT_Validity_Mins { get; set; }
        public int PasswordLinkValidityMins { get; set; }
        public string ErrorSendToEmail { get; set; }
        public int ForgotPasswordAttemptValidityHours { get; set; }
        public string? ApiUrl { get; set; }
        public string ErrorEmail { get; set; }
        public string EmailLogo { get; set; }
        public string URL { get; set; }
        public string EnvelopIcon { get; set; }
        public string FacebookIcon { get; set; }
        public string InstagramIcon { get; set; }
        public string LinkedIn { get; set; }
        public string RecurimentBanner { get; set; }
        public string AppStoreLogo { get; set; }
        public string PlayStoreLogo { get; set; }
        public string AppStoreURL { get; set; }
        public string PlayStoreURL { get; set; }
        public string EnvelopURL { get; set; }
        public string FacebookURL { get; set; }
        public string InstagramURL { get; set; }
        public string LinkedInURL { get; set; }
        public string ContactUsMail { get;set; }


    }

  

}
