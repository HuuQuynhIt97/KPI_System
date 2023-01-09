using System;

namespace A4KPI.DTO
{
    public class MailSettings
    {
        public string Mail { get; set; }
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string UserName { get; set; }

        public string DisplayName { get; set; }
        public bool UseDefaultCredentials { get; set; }
        public bool EnableSsl { get; set; }

        public string Password { get; set; }
        public string Host { get; set; }
        public string Server { get; set; }

        public int Port { get; set; }
    }

}
