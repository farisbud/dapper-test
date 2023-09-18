using pusdafi.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace pusdafi.ViewModes.Club
{
    public class EditClubVM
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        public string? Image { get; set; }
       
        [NotMapped]
        public IFormFile? ImagePath { get; set; }
        [Required]
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        //public int? ClubCategory { get; set; }
        [Required(ErrorMessage = "Please select an option")]
        public int? ClubCategory { get; set; }
       // public Club_Category Club_Category { get; set; }


        //[ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        
    }
}
