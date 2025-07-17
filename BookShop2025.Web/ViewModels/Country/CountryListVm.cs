using System.ComponentModel;

namespace BookShop2025.Web.ViewModels.Country
{
    public class CountryListVm
    {
        public int CountryId { get; set; }
        [DisplayName("Country")]
        public string CountryName { get; set; } = null!;

    }
}
