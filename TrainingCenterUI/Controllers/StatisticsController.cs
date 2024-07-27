using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TrainingCenterLib.Entities;
using TrainingCenterLib.Repository;
using TrainingCenterLib.Repository.Interfaces;
using TrainingCenterLib.Repository.Services;

namespace TrainingCenterUI.Controllers
{
    public class StatisticsController : Controller
    {

        private readonly IStudentService _studentService;
        private readonly ICourseService _courseService;
        private readonly IInstructorService _instructorService;

        public StatisticsController() 
        {
            _studentService = new StudentService();
            _courseService = new CourseService();
            _instructorService = new InstructorService();
        }

        // GET: Statistics
        public async Task<ActionResult> Index()
        {
            if (Session["login"] != null)
            {
                var viewModel = new StatisticViewModel
                {
                    NumberOfStudents = await _studentService.GetNumberOfStudentsAsync(),
                    NumberOfCourses = await _courseService.GetNumberOfCoursesAsync(),
                    NumberOfInstructors = await _instructorService.GetNumberOfInstructorsAsync()
                };

                return View(viewModel);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
            
         
        }
    }
}