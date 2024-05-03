using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreApiWithEntity.DAL.Models
{
    public class ImageUpload
    {
        public IFormFile ImageFile { get; set; }
    }
}
