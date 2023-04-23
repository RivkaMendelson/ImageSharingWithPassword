using ImageShareWithPassword_Apr_17.Models;
using ImageSharing_Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ImageShareWithPassword_Apr_17.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=ImageUpload; Integrated Security=true;";

        private IWebHostEnvironment _webHostEnvironment;

        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index(int id)
        {

            return View();
        }


        public IActionResult Upload(IFormFile image, string password)
        {
            var fileName = $"{Guid.NewGuid()}-{image.FileName}";
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Uploads", fileName);
            using var fs = new FileStream(filePath, FileMode.CreateNew);
            image.CopyTo(fs);

            Image img = new()
            {
                FileName = fileName,
                Passcode = password
            };

            ImageDatabase db = new ImageDatabase(_connectionString);
            db.AddNewImage(img);

            return View(img);

        }

        public IActionResult ViewImage(int id)
        {
            
            ViewImageViewModel vm = new()
            {
                id=id,
                hasPermissionToView = false
            };
            return View(vm);
        }

        [HttpPost]
        public IActionResult ViewImage(int id, string password)
        {
            ImageDatabase db = new ImageDatabase(_connectionString);
            Image img = db.GetImageById(id);
            if(password == null)
            {
                return Redirect("/home/viewImage");
            }    
            if (img.Passcode != password)
            {
                ViewImageViewModel vm = new()
                {
                    image=img,
                    hasPermissionToView = false
                };
                return View(vm);
            }

            else
            {
                db.IncrementViews(img.Id);
                ViewImageViewModel vm = new()
                {
                    hasPermissionToView = true,
                    image = db.GetImageById(id)
                };
               
                return View(vm);
            }

        }

        


    }
}