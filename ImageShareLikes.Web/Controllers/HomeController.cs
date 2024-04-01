using ImageShareLikes.Data;
using ImageShareLikes.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ImageShareLikes.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _connectionString;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var repo = new ImageRepository(_connectionString);

            return View(new ImageViewModel
            {
                Images = repo.GetAll()
            });
        }

        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile imagePath, Image image)
        {
            var fileName = $"{Guid.NewGuid()}{imagePath.FileName}";
            var fullImagePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", fileName);
            using FileStream fs = new FileStream(fullImagePath, FileMode.Create);
            imagePath.CopyTo(fs);
 
            var repo = new ImageRepository(_connectionString);
            repo.Add(new() { Title = image.Title, DatePosted = DateTime.Now, ImagePath = fileName, Likes = 0 }); ;
            return Redirect("/");
        }
        
        public IActionResult ViewImage(int id)
        {
            var repo = new ImageRepository(_connectionString);
            return View(new ImageViewModel
            {
                Image = repo.GetById(id)
            });
        }

        [HttpPost]
        public void IncrementLikesForImage(int id)
        {
            var repo = new ImageRepository(_connectionString);
            repo.IncrementLikesForImage(id);
        }

        public IActionResult GetLikesForImage(int id)
        {
            var repo = new ImageRepository(_connectionString);
            var likes = repo.GetLikesForImage(id);
            return Json(likes);
        }
    }
}
