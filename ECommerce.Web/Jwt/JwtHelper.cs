using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Web.Jwt
{
    public class JwtHelper
    {
        public static string GenerateJwtToken(JwtDto jwtInfo)
        {
            // SecretKey'i kullanarak simetrik bir güvenlik anahtarı oluştur
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtInfo.SecretKey));

            // Token imzalamak için kimlik bilgileri (credentials) oluştur
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            // Token içerisine konulacak claim'ler (kullanıcı bilgileri)
            var claims = new[]
            {
                new Claim(JwtClaimName.Id,jwtInfo.Id.ToString()),              // Kullanıcı ID
                new Claim(JwtClaimName.FirstName, jwtInfo.FirstName ),        // Kullanıcı adı
                new Claim(JwtClaimName.LastName, jwtInfo.LastName),           // Kullanıcı soyadı
                new Claim(JwtClaimName.Email, jwtInfo.Email),                 // Email adresi
                new Claim(JwtClaimName.PhoneNumber,jwtInfo.PhoneNumber?? string.Empty), // Telefon numarası
                new Claim(JwtClaimName.UserType,jwtInfo.UserType.ToString()), // Kullanıcı tipi
                new Claim(ClaimTypes.Role,jwtInfo.UserType.ToString())         // Kullanıcının rolü
            };

            // Token'ın geçerlilik süresi
            var expireTime = DateTime.UtcNow.AddMinutes(jwtInfo.ExpireMinutes);

            // JWT token nesnesi oluştur
            var tokenDescriptor = new JwtSecurityToken(
                jwtInfo.Issuer,     // Token'ı yayınlayan
                jwtInfo.Audience,   // Token'ın hedef kitlesi
                claims,             // Claim'ler
                null,               // NotBefore zamanı (isteğe bağlı)
                expireTime,         // Expire zamanı
                credentials         // Token imzalama bilgisi
            );

            // Token'ı string olarak dön
            var token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

            return token;
        }
    }
}
