using System.ComponentModel;

namespace BookShop2025.Web.ViewModels.Country
{
    public class CountryEditVm
    {
        public int CountryId { get; set; }
        [DisplayName("Country")]
        public string CountryName { get; set; } = null!;

    }
}
