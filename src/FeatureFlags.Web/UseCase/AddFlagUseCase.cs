using System.Security.Claims;
using FeatureFlags.Web.Data;
using FeatureFlags.Web.Data.Entities;
using FeatureFlags.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlags.Web.UseCase {
    public class AddFlagUseCase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AddFlagUseCase(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> Execute(string flagName, bool isActive)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            FlagEntity entity = new()
            {
                Name = flagName,
                UserId = userId,
                Value = isActive
            };
            var response = await _applicationDbContext.Flags.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
