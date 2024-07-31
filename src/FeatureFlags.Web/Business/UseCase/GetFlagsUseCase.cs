using FeatureFlags.Web.Business.UserInfo;
using FeatureFlags.Web.Data;
using FeatureFlags.Web.DTOs;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlags.Web.Business.UseCase {
    public class GetFlagsUseCase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IFlagUserDetails _userDetails;
        public GetFlagsUseCase(ApplicationDbContext applicationDbContext, IFlagUserDetails userDetails)
        {
            _applicationDbContext = applicationDbContext;
            _userDetails = userDetails;
        }

        public async Task<List<FlagDto>> Execute()
        {

            var response = await _applicationDbContext.Flags
                .Where(a => a.UserId == _userDetails.UserId)
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
