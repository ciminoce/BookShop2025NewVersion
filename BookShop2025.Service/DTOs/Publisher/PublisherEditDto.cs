namespace BookShop2025.Service.DTOs.Publisher
{
    public class PublisherEditDto
    {
        public int PublisherId { get; set; }
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
