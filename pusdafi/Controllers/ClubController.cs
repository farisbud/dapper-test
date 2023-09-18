using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using pusdafi.Data;
using pusdafi.Interface;
using pusdafi.Models;
using pusdafi.ViewModes.Club;
using System.Drawing;
using System.IO;


namespace pusdafi.Controllers
{
    public class ClubController : Controller
    {
        //private readonly ApplicationDBContext _context;
        private readonly IClubRepository _clubRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ClubController(IClubRepository clubRepository, IWebHostEnvironment webHostEnvironment)
        {
            _clubRepository = clubRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            //var club = _context.Clubs.ToList();

            //list menampilkan semua data
            //List<Club> clubs = _context.Clubs.ToList();

            IEnumerable<Club> clubs = await _clubRepository.getAll();

            TempData.Remove("SuccessMessage");
            return View(clubs);
        }

        //router bisa pakai
        //[HttpGet("/{id}")]
        public async Task<IActionResult> Detail(int id)
        {

            //Club club = _context.Clubs.Include(a => a.Address).FirstOrDefault(c => c.Id == id);

            Club clubs = await _clubRepository.GetByAsync(id);
            //var viewData = "";

            //viewData["result"] = await _clubRepository.GetByAsync(id);
            
            return View(clubs);
        }

       

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var cat = await _clubRepository.getCategory();
            ViewBag.Category = new SelectList(cat, "Id", "Category");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubVM clubVM)
        {

            var cek = clubVM;

            if (!ModelState.IsValid)
            {
                return View(clubVM);
            }


            string uniqueImage = getUploadImage(clubVM);
            var club = new Club
            {
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = uniqueImage,
                Club_Category = new Club_Category
                {
                    Id = clubVM.ClubCategory
                },
                Address = new Address
                {
                    Street = clubVM.Address.Street,
                    City = clubVM.Address.City,
                    State = clubVM.Address.State
                }
            };



            _clubRepository.Add(club);
            TempData["SuccessMessage"] = "The operation was successful!";
            return RedirectToAction("Index");
        }

        // [HttpGet("/Edit/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            // var cat = await _clubRepository.getCategory();
            var cat = await _clubRepository.getCategory();
            ViewBag.Category = new SelectList(cat, "Id", "Category");

            Club clubs = await _clubRepository.EditByAsync(id);
            if (clubs == null) return View("Error");


            var clubVM = new EditClubVM
            {
                Id = clubs.Id,
                Title = clubs.Title,
                Description = clubs.Description,
                Image = clubs.Image,
                AddressId = clubs.Address.Id,
                Address = clubs.Address,
                ClubCategory = clubs.ClubCategory,   
            };

          
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubVM clubVM)
        {

            
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed Edit Club");
                return View("Edit",clubVM);
            }

            string uniqueImage = "";
            if (clubVM.ImagePath != null)
            {
                var imageUrl = clubVM.Image;
                 uniqueImage = getEditImage(clubVM, imageUrl);
            }
            else
            {
                uniqueImage = clubVM.Image;
            }

            var club = new Club
            {
                Id = id,
                Title = clubVM.Title,
                Description = clubVM.Description,
                Image = uniqueImage,
                AddressId= clubVM.AddressId,
                Address = clubVM.Address,
                ClubCategory= clubVM.ClubCategory,
                //Club_Category = clubVM.Club_Category
            };

        
            _clubRepository.Update(club);
            TempData["SuccessMessage"] = "The operation was updated!";
            return RedirectToAction("Index");
        }
        
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var club = await _clubRepository.EditByAsync(id);
            //var cek = id;
            if (id == null || club == null)
            {
                return View("Error");
            }

            string imageUrl = club.Image;
            string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\club");
            string oldFilePath = Path.Combine(uploadFolder, imageUrl);
            if (System.IO.File.Exists(oldFilePath))
            {
                // Delete the file if it exists.
                System.IO.File.Delete(oldFilePath);
            }

            _clubRepository.Delete(club);

            TempData["SuccessMessage"] = "Delete Success!";
            return RedirectToAction("Index");
        }

        private string getUploadImage(CreateClubVM clubVM)
        {


            string uniqueFileName = null;
            if (clubVM.ImagePath != null)
            {
                //var cek2 = clubVM.ImagePath;

                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\club");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + clubVM.ImagePath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using(var image = Image.Load(clubVM.ImagePath.OpenReadStream()))
                {
                    string newSize = resizeImage(image, 800, 600);
                    string[] aSize = newSize.Split(',');
                    image.Mutate(h => h.Resize(Convert.ToInt32(aSize[1]), Convert.ToInt32(aSize[0])));
                    image.Save(filePath);
                }

            }
            return uniqueFileName;
        }
        private string getEditImage(EditClubVM clubVM, string imageUrl)
        {


            string uniqueFileName = null;
            if (clubVM.ImagePath != null)
            {
                    //var cek2 = clubVM.ImagePath;

                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images\\club");
                string oldFilePath = Path.Combine(uploadFolder, imageUrl);
                if (System.IO.File.Exists(oldFilePath))
                {
                    // Delete the file if it exists.
                    System.IO.File.Delete(oldFilePath);
                }

                uniqueFileName = Guid.NewGuid().ToString() + "_" + clubVM.ImagePath.FileName;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var image = Image.Load(clubVM.ImagePath.OpenReadStream()))
                {
                    string newSize = resizeImage(image, 800, 600);
                    string[] aSize = newSize.Split(',');
                    image.Mutate(h => h.Resize(Convert.ToInt32(aSize[1]), Convert.ToInt32(aSize[0])));
                    image.Save(filePath);
                }

            }
            return uniqueFileName;
        }
        private string resizeImage(Image img,int maxWidth,int maxHeight)
        {
            if(img.Width>maxWidth || img.Height > maxHeight)
            {
                double widthRatio = (double)img.Width/ (double)maxHeight;
                double heightRatio = (double)img.Height / (double)maxWidth;
                double ratio = Math.Max(widthRatio, heightRatio);
                int newWidth = (int)(img.Width / ratio);
                int newHeight = (int)(img.Height / ratio);
                return newHeight.ToString() + "," + newWidth.ToString();
            }
            else
            {
                return img.Height.ToString() +","+img.Width.ToString();
            }
        }



    }
}
