namespace BookShop2025.Service.DTOs.Publisher
{
    public class PublisherListDto
    {
        public int PublisherId { get; set; }
        public string Name { get; set; } = null!;
        public string CountryName { get; set; } = null!;
        public string? ImageUrl {  get; set; }

    }
}
