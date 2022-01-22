using System.ComponentModel.DataAnnotations;

namespace SYT.VendingMachineSystem.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}