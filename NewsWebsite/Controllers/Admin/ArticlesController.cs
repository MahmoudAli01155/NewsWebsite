using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsWebsite.Data;
using NewsWebsite.Models;

namespace NewsWebsite.Controllers
{
    [Authorize] // كل العمليات تحتاج تسجيل دخول
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticlesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            var articles = await _context.Articles
                .Include(a => a.Category)
                .Include(a => a.Author)
                .Include(a => a.Media)
                .OrderByDescending(a => a.PublishDate)
                .ToListAsync();
            return View(articles);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View(new Article { PublishDate = DateTime.Now });
        }

        // POST: Articles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Article article, IFormFile CoverImageFile, List<IFormFile> MediaFiles)
        {
            // إزالة التحقق من ModelState للحقول التي قد تكون مشكلة
            ModelState.Remove("AuthorId");
            ModelState.Remove("Author");
            ModelState.Remove("Category");
            ModelState.Remove("Media");
            ModelState.Remove("CoverImage");

            try
            {
                // تعيين البيانات المطلوبة
                article.PublishDate = article.PublishDate == default ? DateTime.Now : article.PublishDate;
                article.AuthorId = _context.Users.First(u => u.UserName == User.Identity.Name).Id;

                // رفع الصورة الرئيسية
                if (CoverImageFile != null && CoverImageFile.Length > 0)
                {
                    article.CoverImage = await UploadFile(CoverImageFile, "CoverImage");
                }

                _context.Add(article);
                await _context.SaveChangesAsync();

                // رفع الملفات المتعددة
                if (MediaFiles != null && MediaFiles.Count > 0)
                {
                    foreach (var file in MediaFiles)
                    {
                        if (file != null && file.Length > 0)
                        {
                            string folderName = GetFolderNameByFileType(file);
                            string fileUrl = await UploadFile(file, folderName);

                            var media = new Media
                            {
                                ArticleId = article.Id,
                                Url = fileUrl,
                                Type = folderName
                            };

                            _context.Media.Add(media);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "تم إضافة المقال بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ أثناء حفظ المقال: " + ex.Message);
                ViewBag.Categories = _context.Categories.ToList();
                return View(article);
            }
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles
                .Include(a => a.Media)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(article);
        }

        // POST: Articles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("Id,Title,Summary,Content,CategoryId,IsPublished")] Article article,
            IFormFile CoverImageFile,
            List<IFormFile> MediaFiles)
        {
            if (id != article.Id) return NotFound();

            // مسح أخطاء ModelState للحقول غير الموجودة في Bind
            ModelState.Clear();

            try
            {
                // التحقق اليدوي من الحقول المهمة
                if (string.IsNullOrEmpty(article.Title))
                {
                    ModelState.AddModelError("Title", "عنوان المقال مطلوب");
                }

                if (string.IsNullOrEmpty(article.Content))
                {
                    ModelState.AddModelError("Content", "محتوى المقال مطلوب");
                }

                if (article.CategoryId <= 0)
                {
                    ModelState.AddModelError("CategoryId", "التصنيف مطلوب");
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(article);
                }

                // باقي الكود كما هو...
                var existingArticle = await _context.Articles
                    .AsNoTracking()
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (existingArticle == null) return NotFound();

                article.AuthorId = existingArticle.AuthorId;
                article.PublishDate = existingArticle.PublishDate;

                // معالجة الصورة
                if (CoverImageFile != null && CoverImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(existingArticle.CoverImage))
                    {
                        DeleteFile(existingArticle.CoverImage);
                    }
                    article.CoverImage = await UploadFile(CoverImageFile, "CoverImage");
                }
                else
                {
                    article.CoverImage = existingArticle.CoverImage;
                }

                _context.Update(article);
                await _context.SaveChangesAsync();

                // رفع الملفات الجديدة
                if (MediaFiles != null && MediaFiles.Count > 0)
                {
                    foreach (var file in MediaFiles)
                    {
                        if (file != null && file.Length > 0)
                        {
                            string folderName = GetFolderNameByFileType(file);
                            string fileUrl = await UploadFile(file, folderName);

                            var media = new Media
                            {
                                ArticleId = article.Id,
                                Url = fileUrl,
                                Type = folderName
                            };

                            _context.Media.Add(media);
                        }
                    }
                    await _context.SaveChangesAsync();
                }

                TempData["Success"] = "تم تحديث المقال بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "حدث خطأ: " + ex.Message);
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(article);
        }

        // POST: Media/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMedia(int mediaId)
        {
            try
            {
                var media = await _context.Media.FindAsync(mediaId);
                if (media == null)
                    return Json(new { success = false, message = "الملف غير موجود" });

                // حذف الملف الفعلي
                DeleteFile(media.Url);

                _context.Media.Remove(media);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "تم حذف الملف بنجاح" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "حدث خطأ: " + ex.Message });
            }
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var article = await _context.Articles
                .Include(a => a.Category)
                .Include(a => a.Author)
                .Include(a => a.Media)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (article == null) return NotFound();

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var article = await _context.Articles
                    .Include(a => a.Media)
                    .FirstOrDefaultAsync(a => a.Id == id);

                if (article == null)
                    return NotFound();

                // حذف الصورة الرئيسية
                if (!string.IsNullOrEmpty(article.CoverImage))
                {
                    DeleteFile(article.CoverImage);
                }

                // حذف جميع ملفات الوسائط
                if (article.Media != null && article.Media.Any())
                {
                    foreach (var media in article.Media.ToList())
                    {
                        DeleteFile(media.Url);
                        _context.Media.Remove(media);
                    }
                }

                _context.Articles.Remove(article);
                await _context.SaveChangesAsync();

                TempData["Success"] = "تم حذف المقال بنجاح";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = "حدث خطأ أثناء حذف المقال: " + ex.Message;
                return RedirectToAction(nameof(Index));
            }
        }

        // دالة مساعدة لرفع الملفات
        private async Task<string> UploadFile(IFormFile file, string folderName)
        {
            try
            {
                // تحديد مسار المجلد
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", folderName);

                // إنشاء المجلد إذا لم يكن موجوداً
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // إنشاء اسم ملف فريد
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // حفظ الملف
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // إرجاع المسار النسبي
                return $"/uploads/{folderName}/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                throw new Exception("فشل رفع الملف: " + ex.Message);
            }
        }

        // دالة مساعدة لحذف الملفات
        private void DeleteFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath)) return;

                string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, filePath.TrimStart('/'));
                if (System.IO.File.Exists(fullPath))
                {
                    System.IO.File.Delete(fullPath);
                }
            }
            catch (Exception ex)
            {
                // سجل الخطأ ولكن لا توقف العملية
                Console.WriteLine($"خطأ في حذف الملف: {ex.Message}");
            }
        }

        // دالة تحديد نوع الملف
        private string GetFolderNameByFileType(IFormFile file)
        {
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp", ".svg" };
            string[] videoExtensions = { ".mp4", ".mov", ".avi", ".wmv", ".mkv", ".flv", ".webm" };
            string[] audioExtensions = { ".mp3", ".wav", ".ogg", ".m4a", ".wma", ".aac" };
            string[] iframeExtensions = { ".html", ".htm", ".pdf", ".txt" };

            if (imageExtensions.Contains(extension))
                return "Image";
            else if (videoExtensions.Contains(extension))
                return "Video";
            else if (audioExtensions.Contains(extension))
                return "Audio";
            else if (iframeExtensions.Contains(extension))
                return "IFrame";
            else
                return "Media";
        }
    }
}