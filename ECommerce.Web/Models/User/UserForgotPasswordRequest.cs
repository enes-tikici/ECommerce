using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models.User
{
    public class UserForgotPasswordRequest
    {
        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email adresi giriniz.")]
        public string Email { get; set; }
    }
}
