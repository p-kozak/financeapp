using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PersonalFinance.Domain.Configuration
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "cef3b69a-9c1d-4bbd-8c98-e3024d93c924",
                    Name = "Visitor",
                    NormalizedName = "VISITOR"
                },
                new IdentityRole
                {
                    Id = "6b122f3c-8700-4208-8902-d77b72eff1b9",
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                },
                new IdentityRole
                {
                    Id = "3cd25e1b-2501-4fce-8bac-fb7ab724d6a0",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                });
        }
    }
}
