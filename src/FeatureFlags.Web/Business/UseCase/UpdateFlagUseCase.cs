using FeatureFlags.Web.Business.Mappers;
using FeatureFlags.Web.Business.UserInfo;
using FeatureFlags.Web.Data;
using FeatureFlags.Web.Data.Entities;
using FeatureFlags.Web.DTOs;
using Microsoft.EntityFrameworkCore;
using ROP;

namespace FeatureFlags.Web.Business.UseCase {
	public class UpdateFlagUseCase(ApplicationDbContext applicationDbContext,
		IFlagUserDetails userDetails) {
		public async Task<Result<FlagDto>> Execute(FlagDto flagDto)
		=> await VerifyIsTheOnlyOneWhitThatName(flagDto)
				.Bind(x => GetFromDb(x.Id))
				.Bind(x => Update(x, flagDto))
				.Map(x => x.ToDto());
		
		private async Task<Result<FlagDto>> VerifyIsTheOnlyOneWhitThatName(FlagDto dto)
		{
			var alreadyExist = await applicationDbContext.Flags
				.AnyAsync(a => a.UserId == userDetails.UserId
				               && a.Name.Equals(dto.Name, StringComparison.CurrentCultureIgnoreCase)
				               && a.Id != dto.Id);

			if (alreadyExist)
			{
				return Result.Failure<FlagDto>("Flag with the same name already exist");
			}

			return dto;
		}

		private async Task<Result<FlagEntity>> GetFromDb(int id)
		{
			return await applicationDbContext.Flags.Where(f => f.Id == id && f.UserId == userDetails.UserId).SingleAsync();
		}

		private async Task<Result<FlagEntity>> Update(FlagEntity entity, FlagDto flagDto) {
			entity.Value = flagDto.IsEnabled;
			entity.Name = flagDto.Name;
			await applicationDbContext.SaveChangesAsync();
			return entity;
		}
	}
}
