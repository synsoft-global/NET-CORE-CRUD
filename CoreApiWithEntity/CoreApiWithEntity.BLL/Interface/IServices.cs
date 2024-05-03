using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiWithEntity.BLL.Interface
{
    public interface IServices
    {
        Task<bool> SendMail(string emailid, string subject, string body);
    }
}
