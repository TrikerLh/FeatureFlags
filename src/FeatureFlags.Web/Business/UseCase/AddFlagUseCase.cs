using FeatureFlags.Web.Business.UserInfo;
using FeatureFlags.Web.Data;
using FeatureFlags.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;

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
        public async Task<Result<bool>> Execute(string flagName, bool isActive)
        {
            return await ValidateFlag(flagName).Bind(x => ADdFlagToDatabase(x, isActive));
        }

        private async Task<Result<string>> ValidateFlag(string flagName)
        {
            var flagExist = await _applicationDbContext.Flags
                .Where(a => a.UserId == _userDetails.UserId
                            && a.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase))
                .AnyAsync();
            if (flagExist)
            {
                return Result.Failure<string>("Flag Name Already Exist");
            }

            return flagName;
        }

        private async Task<Result<bool>> ADdFlagToDatabase(string flagName, bool isActive)
        {
            FlagEntity entity = new() {
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
