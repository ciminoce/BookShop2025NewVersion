namespace BookShop2025.Service.DTOs.Author
{
    public class AuthorListDto
    {
        public int AuthorId { get; set; }
        public string FullName { get; set; } = null!;
        public string CountryName { get; set; } = null!;
    }
}
