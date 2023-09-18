using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using pusdafi.Data.Enum;

namespace pusdafi.Models
{
    public class Club
    {
      

        [Key]
        public int Id { get; set; }
       // [Required]
        public string? Title { get; set; }
       // [Required]
        public string? Description { get; set; }
     
        public string? Image { get; set; }
        //[NotMapped]
        //[Required]
        //public IFormFile ImagePath { get; set; }

        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        
        [ForeignKey("Clubs_Category")] 
        //[Required(ErrorMessage = "Please select an option")]
        public int? ClubCategory { get; set; }
        [NotMapped]
        public Club_Category? Club_Category { get; set; }
 
        //public List<Club_Category>? Club_Category { get; set; }

        [ForeignKey("AppUser")]
        public string? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }

        //[Key]
        //public int Id { get; set; }
        //[Required]
        //public string? Title { get; set; }
        //[Required]
        //public string? Description { get; set; }
        //[Required]
        //public string? Image { get; set; }

        //public string? AddressId { get; set; }
        //public string? ClubCategory { get; set; }
        //public string? AppUserId { get; set; }


    }
}
