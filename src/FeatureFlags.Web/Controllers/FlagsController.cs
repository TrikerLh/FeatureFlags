using System.Security.Claims;
using FeatureFlags.Web.UseCase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FeatureFlags.Web.Controllers {
    [Authorize]
    [Route("[controller]")]
    public class FlagsController : Controller
    {
        private readonly IAddFlagUseCase _addFlagUseCase;

        public FlagsController(IAddFlagUseCase addFlagUseCase)
        {
            _addFlagUseCase = addFlagUseCase;
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new FlagViewModel());
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddFlagToDatabase(FlagViewModel request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isCreated = await _addFlagUseCase.Execute(request.Name, request.IsEnabled);
            return RedirectToAction("Index");
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }
    }

    public class FlagViewModel
    {
        public string Name { get; set; }
        public bool IsEnabled { get; set; }
    }
}
