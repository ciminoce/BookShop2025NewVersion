namespace BookShop2025.Service.DTOs.Category
{
    public class CategoryEditDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
