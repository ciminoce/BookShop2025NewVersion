using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;

namespace BookShop2025.Web.ViewModels.Author
{
    public class AuthorEditVm
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        [DisplayName("Country")]
        public int CountryId { get; set; }
        [ValidateNever]
        public List<SelectListItem> Countries { get; set; } = null!;

    }
}
