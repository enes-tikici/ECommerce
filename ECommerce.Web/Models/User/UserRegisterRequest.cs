using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models.User
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "İsim alanı zorunludur.")]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Soyisim alanı zorunludur.")]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email alanı zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir email giriniz.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Şifre alanı zorunludur.")]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Telefon numarası zorunludur.")]
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
