using System.Security.Claims;
using FeatureFlags.Web.Data;
using FeatureFlags.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlags.Web.UseCase {
    public class GetFlagsUseCase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public GetFlagsUseCase(ApplicationDbContext applicationDbContext, IHttpContextAccessor httpContextAccessor)
        {
            _applicationDbContext = applicationDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<FlagDto>> Execute()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var response = await _applicationDbContext.Flags
                .Where(a => a.UserId == userId)
                .AsNoTracking()
                .ToListAsync();

            return response.Select(a => new FlagDto()
            {
                IsEnabled = a.Value,
                Name = a.Name
            }).ToList();
        }
    }
}
