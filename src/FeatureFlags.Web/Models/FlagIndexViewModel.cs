﻿using FeatureFlags.Web.DTOs;

namespace FeatureFlags.Web.Models {
    public class FlagIndexViewModel {
        public Pagination<FlagDto> Pagination { get; set; }
        public List<int> SelectOptions { get; set; } = [5, 10, 15];
    }
}
