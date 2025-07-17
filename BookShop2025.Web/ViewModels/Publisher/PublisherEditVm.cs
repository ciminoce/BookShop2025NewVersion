using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BookShop2025.Web.ViewModels.Publisher
{
    public class PublisherEditVm
    {
        public int PublisherId { get; set; }
        public string Name { get; set; } = null!;
        [DisplayName("Country")]
        public int CountryId { get; set; }

        [DisplayName("Logo")]
        public string? ImageUrl { get; set; }
        [ValidateNever]
        public List<SelectListItem> Countries { get; set; } = null!;

    }
}
