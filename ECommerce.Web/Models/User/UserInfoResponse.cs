using ECommerce.Core.Enums;

namespace ECommerce.Web.Models.User
{
    public class UserInfoResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public UserType UserType { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
