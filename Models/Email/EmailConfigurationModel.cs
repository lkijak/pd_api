

namespace pd_api.Models.Email
{
    public class EmailConfigurationModel : BaseModel
    {
        public string FriendlyName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public bool UseDefaultCredential { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSSL { get; set; }
        public string DefaultMessageBody { get; set; }
    }
}
