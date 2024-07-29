using FeatureFlags.Web.DTOs;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace FeatureFlags.Web.Models {
    public class FlagIndexViewModel {
        public List<FlagDto> Flags { get; set; }
    }
}
