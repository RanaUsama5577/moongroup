using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Entities
{
    public class UpdateProfileImage
    {
        public IFormFile file { get; set; }
    }

    public class UpdateCategoryImage
    {
        public IFormFile file { get; set; }
        public int Id { get; set; }
    }

    public class UpdateCategoryDetails
    {
        public string Name { get; set; }
        public string Name_Arb { get; set; }
        public int Id { get; set; }
    }
}