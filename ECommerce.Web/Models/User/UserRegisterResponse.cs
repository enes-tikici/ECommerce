namespace ECommerce.Web.Models.User
{
    public class UserRegisterResponse
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
