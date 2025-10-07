using ECommerce.Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Core.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public  string ResetToken { get; set; }
        public UserType UserType { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public Cart Cart { get; set; }

        [NotMapped]
        public string FullName => $"{FirstName} {LastName}"; // Okuma için, DB’ye kaydolmaz
    }

    public class UserConfiguration : BaseConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.Property(u => u.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

            builder.Property(u => u.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(150);
        }
    }
}
