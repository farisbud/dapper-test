using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace pusdafi.Models
{
    public class Club_Category
    {
        [Key]
        public int Id { get; set; }
        public string Category { get; set; }
       // public IEnumerable<SelectListItem> Category { get; set; }

    }
}
