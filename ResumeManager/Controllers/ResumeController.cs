using Microsoft.AspNetCore.Mvc;
using ResumeManager.Data;
using ResumeManager.Models;

namespace ResumeManager.Controllers
{
    public class ResumeController : Controller
    {
        private readonly ResumeDbContext dbContext;
        public ResumeController(ResumeDbContext dbContext)
        {
            this.dbContext = dbContext;
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
            applicant.Experiences.Add(new Experience() { ApplicantId = 2 });
            applicant.Experiences.Add(new Experience() { ApplicantId = 3 });
            return View(applicant);
        }
        [HttpPost]
        public IActionResult Create(Applicant applicant)
        {
            dbContext.Add(applicant);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
