using System.ComponentModel.DataAnnotations;

namespace NewsWebsite.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "عنوان المقال مطلوب")]
        [MaxLength(250)]
        [Display(Name = "عنوان المقال")]
        public string Title { get; set; }

        [Display(Name = "ملخص المقال")]
        public string Summary { get; set; }

        [Required(ErrorMessage = "محتوى المقال مطلوب")]
        [Display(Name = "محتوى المقال")]
        public string Content { get; set; }

        [Display(Name = "الصورة الرئيسية")]
        public string CoverImage { get; set; }

        [Display(Name = "تاريخ النشر")]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Display(Name = "تم النشر")]
        public bool IsPublished { get; set; }

        [Display(Name = "التصنيف")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Display(Name = "الوسائط")]
        public List<Media> Media { get; set; }

        // علاقة المقال بالكاتب
        [Display(Name = "الكاتب")]
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }
    }
}