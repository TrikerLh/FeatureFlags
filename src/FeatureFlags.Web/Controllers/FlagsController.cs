﻿using System.Security.Claims;
using FeatureFlags.Web.Business.UseCase;
using FeatureFlags.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ROP;

namespace FeatureFlags.Web.Controllers {
    [Authorize]
    [Route("[controller]")]
    public class FlagsController : Controller
    {
        private readonly AddFlagUseCase _addFlagUseCase;
        private readonly GetFlagsUseCase _getFlagsUseCase;

        public FlagsController(AddFlagUseCase addFlagUseCase, GetFlagsUseCase getFlagsUseCase)
        {
            _addFlagUseCase = addFlagUseCase;
            _getFlagsUseCase = getFlagsUseCase;
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View(new FlagViewModel());
        }

        [HttpPost("create")]
        public async Task<IActionResult> AddFlagToDatabase(FlagViewModel request)
        {
            Result<bool> isCreated = await _addFlagUseCase.Execute(request.Name, request.IsEnabled);
            if (isCreated.Success)
            {
                return RedirectToAction("Index");
            }

            return View("Create", new FlagViewModel()
            {
                Error = isCreated.Errors.First().Message,
                IsEnabled = request.IsEnabled,
                Name = request.Name
            });
            //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            //bool isCreated = await _addFlagUseCase.Execute(request.Name, request.IsEnabled);
            //return RedirectToAction("Index");
        }

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var listFlags = await _getFlagsUseCase.Execute();
            return View(new FlagIndexViewModel()
            {
                Flags = listFlags
            });
        }
    }
}
