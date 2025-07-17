namespace BookShop2025.Service.DTOs.Author
{
    public class AuthorEditDto
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public int CountryId { get; set; }

    }
}
