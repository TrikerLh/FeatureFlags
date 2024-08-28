using FeatureFlags.Web.DTOs;

namespace FeatureFlags.Web.Models;

public class SingleFlagViewModel
{
	public FlagDto Flag { get; set; }
	public string? Message { get; set; }
}