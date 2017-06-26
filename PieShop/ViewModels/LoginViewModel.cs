using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations; 

namespace PieShop.ViewModels
{
    public class LoginViewModel
    {
        
        [Required]
        [Display(Name = "User Name: ")]
        public string UserName { get; set; }

        
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }

    }// end class 
}
