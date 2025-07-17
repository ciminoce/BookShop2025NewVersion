using System.ComponentModel;

namespace BookShop2025.Web.ViewModels.Publisher
{
    public class PublisherListVm
    {
        public int PublisherId { get; set; }
        [DisplayName("Publisher")]
        public string Name { get; set; } = null!;
        [DisplayName("Country")]
        public string CountryName { get; set; } = null!;
        [DisplayName("Logo")]
        public string? ImageUrl { get; set; }
    }
}
