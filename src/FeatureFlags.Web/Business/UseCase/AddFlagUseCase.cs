using FeatureFlags.Web.Business.UserInfo;
using FeatureFlags.Web.Data;
using FeatureFlags.Web.Data.Entities;

namespace FeatureFlags.Web.Business.UseCase {
    public class AddFlagUseCase
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IFlagUserDetails _userDetails;

        public AddFlagUseCase(ApplicationDbContext applicationDbContext, IFlagUserDetails userDetails)
        {
            _applicationDbContext = applicationDbContext;
            _userDetails = userDetails;
        }
        public async Task<bool> Execute(string flagName, bool isActive)
        {
            FlagEntity entity = new()
            {
                Name = flagName,
                UserId = _userDetails.UserId,
                Value = isActive
            };
            var response = await _applicationDbContext.Flags.AddAsync(entity);
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
    }
}
