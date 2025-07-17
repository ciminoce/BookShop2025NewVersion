using System.ComponentModel;

namespace BookShop2025.Web.ViewModels.Category
{
    public class CategoryListVm
    {
        public int CategoryId { get; set; }
        [DisplayName("Category")]
        public string CategoryName { get; set; } = null!;
        [DisplayName("Active")]
        public bool IsActive { get; set; }

    }
}
