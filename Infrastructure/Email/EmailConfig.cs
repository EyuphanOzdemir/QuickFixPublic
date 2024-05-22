using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Email
{
    public class EmailConfig
    {
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }

        public string UserName { get; set; }
        public string Password { get; set; }
        public bool EnableSSL = true;

        public EmailConfig() { }
        public EmailConfig(string smtpServer, int smtpPort)
        {
            SMTPServer = smtpServer;
            SMTPPort = smtpPort;
        }
    }
}
