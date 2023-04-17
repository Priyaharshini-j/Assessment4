﻿using System.ComponentModel.DataAnnotations;

namespace LiveDocument.Models
{
    public class LoginModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public Int64 ContactNo { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
