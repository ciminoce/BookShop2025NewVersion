namespace BookShop2025.Entities.Entities
{
    public class Publisher
    {
        public int PublisherId { get; set; }
        public string Name { get; set; } = null!;
        public int CountryId { get; set; }
        public string? ImageUrl { get; set; }
        public Country? Country { get; set; }

    }
}
