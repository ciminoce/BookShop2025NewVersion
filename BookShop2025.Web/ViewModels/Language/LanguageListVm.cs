using System.ComponentModel;

namespace BookShop2025.Web.ViewModels.Language
{
    public class LanguageListVm
    {
        public int LanguageId { get; set; }
        [DisplayName("Language")]
        public string LanguageName { get; set; } = string.Empty;
    }
}
