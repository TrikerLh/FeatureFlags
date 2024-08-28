using FeatureFlags.Web.Business.Mappers;
using FeatureFlags.Web.Business.UserInfo;
using FeatureFlags.Web.Data;
using FeatureFlags.Web.Data.Entities;
using FeatureFlags.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FeatureFlags.Web.Business.UseCase;

public class GetSingleFlagUseCase(
    ApplicationDbContext applicationDbContext,
    IFlagUserDetails userDetails
    )
{
    public async Task<Result<FlagDto>> Execute(string flagName) => await GetFromDb(flagName).Map(x => x.ToDto());

    private async Task<Result<FlagEntity>> GetFromDb(string flagName)
        => await applicationDbContext.Flags
            .Where(a => a.UserId == userDetails.UserId
                        && a.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase))
            .AsNoTracking()
            .SingleAsync();

}