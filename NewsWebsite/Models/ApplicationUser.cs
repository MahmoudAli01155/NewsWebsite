using Microsoft.AspNetCore.Identity;

namespace NewsWebsite.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
        public ICollection<Article>? Articles { get; set; }
    }
}