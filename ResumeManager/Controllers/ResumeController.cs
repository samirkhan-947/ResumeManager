using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ResumeManager.Data;
using ResumeManager.Models;

namespace ResumeManager.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ResumeDbContext dbContext;
		private readonly IWebHostEnvironment _webHost;
		public ResumeController(ResumeDbContext dbContext, IWebHostEnvironment webHost)
        {
            this.dbContext = dbContext;
            _webHost = webHost;

		}
        public IActionResult Index()
        {
            List<Applicant> applicants;
            applicants = dbContext.Applicants.ToList();
            return View(applicants);
        }
        [HttpGet]
        public IActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.Experiences.Add(new Experience() { ApplicantId = 1 });
            ViewBag.Gender = GetGender();
            return View(applicant);
        }
        [HttpPost]
        public IActionResult Create(Applicant applicant)
        {
			string uniqueFileName = GetUploadedFileName(applicant);
			applicant.PhotoUrl = uniqueFileName;

			dbContext.Add(applicant);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
		private string GetUploadedFileName(Applicant applicant)
		{
			string uniqueFileName = null;

			if (applicant.ProfilePhoto != null)
			{
				string uploadsFolder = Path.Combine(_webHost.WebRootPath, "Images");
				uniqueFileName = Guid.NewGuid().ToString() + "_" + applicant.ProfilePhoto.FileName;
				string filePath = Path.Combine(uploadsFolder, uniqueFileName);
				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					applicant.ProfilePhoto.CopyTo(fileStream);
				}
			}
			return uniqueFileName;
		}

        public IActionResult Details(int id)
        {
            Applicant applicant= dbContext.Applicants.Include(a => a.Experiences).Where(x=>x.Id==id).FirstOrDefault();
            return View(applicant);
        }
        [HttpGet]
		public IActionResult Delete(int id)
		{
			Applicant applicant = dbContext.Applicants.Include(a => a.Experiences).Where(x => x.Id == id).FirstOrDefault();
			return View(applicant);
		}
		[HttpPost]
		public IActionResult Delete(Applicant applicant)
		{
			dbContext.Attach(applicant);
			dbContext.Entry(applicant).State= EntityState.Deleted;
			dbContext.SaveChanges();
			return RedirectToAction(nameof(Index));
		}
		private List<SelectListItem> GetGender()
		{
			List<SelectListItem> selGender = new List<SelectListItem>();
			var SetItem =new SelectListItem()
			{
				Value="",
				Text="Select Gender"
			};
			selGender.Insert(0, SetItem);
			SetItem = new SelectListItem()
			{
				Value="Male",
				Text="Male"
			};
			selGender.Add(SetItem);			
			SetItem = new SelectListItem()
			{
				Value = "FeMale",
				Text = "FeMale"
			};
			selGender.Add(SetItem);
			return selGender;
		}
	}
}
