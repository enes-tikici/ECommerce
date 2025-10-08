using System.ComponentModel.DataAnnotations;

namespace ECommerce.Web.Models.User
{
    public class UserInfoRequest
    {
        [Required(ErrorMessage = "Kullanıcı ID alanı zorunludur.")]
        public int UserId { get; set; }
    }
}
