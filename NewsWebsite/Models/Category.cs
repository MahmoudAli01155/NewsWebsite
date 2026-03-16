using System.ComponentModel.DataAnnotations;

namespace NewsWebsite.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "اسم التصنيف مطلوب")]
        [Display(Name = "اسم التصنيف")]
        public string Name { get; set; }

        public List<Article> Articles { get; set; }
    }
}