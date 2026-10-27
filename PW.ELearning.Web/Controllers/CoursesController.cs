using Microsoft.AspNetCore.Mvc;
using PW.Elearning.Application.Courses;

namespace PW.ELearning.Sik.Martonak.Controllers;

public class CoursesController(ICourseService courseService) : Controller
{
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await courseService.GetAllAsync(cancellationToken));
    }
}
