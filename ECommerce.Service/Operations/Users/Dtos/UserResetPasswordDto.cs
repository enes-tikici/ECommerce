using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Service.Operations.Users.Dtos
{
    public class UserResetPasswordDto
    {
        public string Token { get; set; }         // Mailden gelen token
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
