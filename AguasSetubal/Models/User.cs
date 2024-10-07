﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace AguasSetubal.Models
{
    public class User : IdentityUser
    {

        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} characters.")]
        public string FirstName { get; set; }

        [MaxLength(50, ErrorMessage = "The field {0} can only contain {1} characters.")]
        public string LastName { get; set; }

        [MaxLength(100, ErrorMessage = "The field {0} can only contain {1} characters.")]
        public string Address { get; set; }

        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}";

        //[Display(Name = "Image")]
        //public IFormFile ImageFile { get; set; }


    }
}
