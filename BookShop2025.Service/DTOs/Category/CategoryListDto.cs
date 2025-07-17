namespace BookShop2025.Service.DTOs.Category
{
    public class CategoryListDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public bool IsActive { get; set; }

    }
}
