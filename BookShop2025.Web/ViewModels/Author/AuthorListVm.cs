using System.ComponentModel;

namespace BookShop2025.Web.ViewModels.Author
{
    public class AuthorListVm
    {
        public int AuthorId { get; set; }
        [DisplayName("Author")]
        public string FullName { get; set; } = null!;
        [DisplayName("Country")]
        public string CountryName { get; set; } = null!;

    }
}
