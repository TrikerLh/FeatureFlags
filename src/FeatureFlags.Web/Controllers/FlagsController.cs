﻿using System.Security.Claims;
using FeatureFlags.Web.Business.UseCase;
using FeatureFlags.Web.DTOs;
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
        private readonly GetFlagsPaginatedUseCase _getFlagsPaginatedUseCase;
        private readonly GetSingleFlagUseCase _getSingleFlagUseCase;
        private readonly UpdateFlagUseCase _updateFlagUseCase;
        private readonly DeleteFlagUseCase _deleteFlagUseCase;

        public FlagsController(AddFlagUseCase addFlagUseCase, GetFlagsPaginatedUseCase getFlagsPaginatedUseCase, GetSingleFlagUseCase getSingleFlagUseCase, UpdateFlagUseCase updateFlagUseCase, DeleteFlagUseCase deleteFlagUseCase)
        {
            _addFlagUseCase = addFlagUseCase;
            _getFlagsPaginatedUseCase = getFlagsPaginatedUseCase;
            _getSingleFlagUseCase = getSingleFlagUseCase;
            _updateFlagUseCase = updateFlagUseCase;
            _deleteFlagUseCase = deleteFlagUseCase;
        }

        [HttpGet("")]
        [HttpGet("{page:int}")]
        public async Task<IActionResult> Index(string? search, int page = 1, int size = 5)
        {
	        var listFlags = (await _getFlagsPaginatedUseCase.Execute(search, page, size)).Throw();
	        return View(new FlagIndexViewModel()
	        {
		        Pagination = listFlags
	        });
        }

        [HttpGet("{flagName}")]
        public async Task<IActionResult> GetSingle(string flagName, string? message)
        {
	        var singleFlag = await _getSingleFlagUseCase.Execute(flagName);
	        return View("SingleFlag", new SingleFlagViewModel()
	        {
		        Flag = singleFlag.Value,
		        Message = message
	        });
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
        }

        [HttpPost("{flagName}")]
        public async Task<IActionResult> Update(FlagDto flag)
        {
	        var singleFlag = await _updateFlagUseCase.Execute(flag);

	        return View("SingleFlag", new SingleFlagViewModel()
	        {
		        Flag = flag,
		        Message = singleFlag.Success ? "Updated correctly" : singleFlag.Errors.First().Message
	        });
        }

        [HttpGet("delete/{flagName}")]
        public async Task<IActionResult> Delete(string flagName)
        {
	        var isDeleted = await _deleteFlagUseCase.Execute(flagName);

	        if (isDeleted.Success)
	        {
		        return RedirectToAction("");
	        }

	        return RedirectToAction("getSingle", new
	        {
		        flagname = flagName,
		        message = "Error to Delete"
	        });
        }
    }
}
