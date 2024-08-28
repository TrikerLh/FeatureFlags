using FeatureFlags.Web.Business.UserInfo;
using FeatureFlags.Web.Data;
using FeatureFlags.Web.Data.Entities;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FeatureFlags.Web.Business.UseCase;

public class DeleteFlagUseCase(ApplicationDbContext applicationDbContext, IFlagUserDetails userDetails)
{
	public async Task<Result<bool>> Execute(string flagName)
	{
		return await GetEntity(flagName).Bind(DeleteEntity);
	}

	private async Task<Result<FlagEntity>> GetEntity(string flagName)
	{
		return await applicationDbContext.Flags.Where(a =>
			a.Name.Equals(flagName, StringComparison.InvariantCultureIgnoreCase)
			&& a.UserId == userDetails.UserId).SingleAsync();
	}

	private async Task<Result<bool>> DeleteEntity(FlagEntity entity)
	{
		entity.IsDeleted = true;
		entity.DeletedtimeUtc = DateTime.UtcNow;
		await applicationDbContext.SaveChangesAsync();
		return true;
	}
}