using Microsoft.Extensions.Configuration;
using CoreApiWithEntity.BLL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiWithEntity.BLL.Repository
{
    public class ServicesRepo: IServices
    {
        #region Dependency injection  
        public IConfiguration _configuration { get; }
        public ServicesRepo(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        #endregion
        /*This Service Is Used To Send Email*/
        public async Task<bool> SendMail(string emailid, string subject, string body)
        {
            bool Send = true;
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.Host = _configuration["Smtp:SmtpClient"];
                    client.Port = Convert.ToInt32(_configuration["Smtp:SmtpPort"]);

                    MailAddress frmAddress = new MailAddress(_configuration["Smtp:FromMailAddress"], "Crud API Support");

                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(_configuration["Smtp:FromMailAddress"], _configuration["Smtp:Password"]);
                    client.UseDefaultCredentials = false;
                    client.Credentials = credentials;
                    client.EnableSsl = true;

                    using (MailMessage msg = new MailMessage())
                    {
                        msg.From = frmAddress;
                        msg.To.Add(new MailAddress(emailid));

                        msg.Subject = subject;
                        msg.IsBodyHtml = true;
                        msg.Body = body;

                        await client.SendMailAsync(msg);
                    }
                }
            }
            catch (System.Net.Mail.SmtpException ex)
            {
                Send = false;
                // throw ex;
            }
            catch (Exception ex)
            {
                Send = false;
                //throw ex;
            }
            return Send;
        }
    }
}
