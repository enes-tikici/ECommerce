using ECommerce.Core.Enums;

namespace ECommerce.Web.Jwt
{
    public class JwtDto
    {
        public int Id { get; set; }                 // Kullanıcının benzersiz ID'si
        public string Email { get; set; }           // Kullanıcının email adresi
        public string FirstName { get; set; }       // Kullanıcının adı
        public string LastName { get; set; }        // Kullanıcının soyadı
        public string PhoneNumber { get; set; }     // Kullanıcının telefon numarası
        public UserType UserType { get; set; }      // Kullanıcı tipi (Admin, Customer vb.)
        public string SecretKey { get; set; }       // JWT oluşturmak için kullanılan gizli anahtar
        public string Issuer { get; set; }          // Token'ın yayıncısı
        public string Audience { get; set; }        // Token'ın hedef kitlesi
        public int ExpireMinutes { get; set; }
    }
}
