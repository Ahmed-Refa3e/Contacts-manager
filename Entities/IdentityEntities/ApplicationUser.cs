using Microsoft.AspNetCore.Identity;

namespace Entities.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
    }
}
