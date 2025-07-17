namespace BookShop2025.Entities.Entities
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int CountryId { get; set; }
        public Country? Country { get; set; }

    }
}
