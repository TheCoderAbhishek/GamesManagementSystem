using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace GamesManagementSystem.Application.ViewModels
{
    public class GameViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Game name is required.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        public string DevelopedBy { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public string Description { get; set; }

        [Display(Name = "Game Banner")]
        public IFormFile? BannerImageFile { get; set; }
    }
}
