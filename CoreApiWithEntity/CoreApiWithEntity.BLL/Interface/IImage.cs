using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiWithEntity.BLL.Interface
{
    public interface IImage
    {
        Task<string> UploadImageAsync(IFormFile imageFile);
    }
}
