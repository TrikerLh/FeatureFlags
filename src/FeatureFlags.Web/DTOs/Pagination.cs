namespace FeatureFlags.Web.DTOs;

public record Pagination<T>(List<T> Items, int TotalItems, int PageSize, int CurrentPage, string? Search);