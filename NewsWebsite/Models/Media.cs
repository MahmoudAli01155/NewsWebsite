using System.ComponentModel.DataAnnotations;

namespace NewsWebsite.Models
{
    public class Media
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        [Display(Name = "رابط الملف")]
        public string Url { get; set; }

        [Display(Name = "نوع الوسائط")]
        public string Type { get; set; }

        public Article Article { get; set; }
    }
}