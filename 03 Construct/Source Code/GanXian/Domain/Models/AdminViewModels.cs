using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Domain.Models
{
    public class AdminLoginViewModels
    {
        [Required(ErrorMessage = "用户名不能为空")]
        [DataType(DataType.Text)]
        public string Account { get; set; }

        [Required(ErrorMessage = "密码不能为空")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

       
    }
}