using System.ComponentModel;

namespace BookShop2025.Web.ViewModels.Category
{
    public class CategoryEditVm
    {
        public int CategoryId { get; set; }
        [DisplayName("Category Name")]
        public string CategoryName { get; set; } = null!;
        [DisplayName("Category Description")]
        public string? Description { get; set; }
        [DisplayName("Is Active?")]
        public bool IsActive { get; set; }

    }
}
