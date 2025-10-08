using ECommerce.Service.Types;

namespace ECommerce.Web.Models.User
{
    public class UserLoginResponse : ServiceMassage<UserInfoResponse>
    {
        public bool IsSucced { get; set; }
        public string Massage { get; set; }
        public string Token { get; set; }// Placeholder, ileride JWT eklenecek
    }
}
