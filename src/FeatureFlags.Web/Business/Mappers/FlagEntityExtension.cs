using FeatureFlags.Web.Data.Entities;
using FeatureFlags.Web.DTOs;

namespace FeatureFlags.Web.Business.Mappers;
public static class FlagEntityExtension {
	public static FlagDto ToDto(this FlagEntity entity)
		=> new FlagDto() {
			IsEnabled = entity.Value,
			Name = entity.Name,
			Id = entity.Id
		};

	public static List<FlagDto> ToDto(this List<FlagEntity> entities)
		=> entities.Select(ToDto).ToList();
}