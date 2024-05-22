using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Infrastructure.Models.Dto
{
    public class FixDto
    {
        public string Id { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Author is required")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Solution is required")]
        public string Solution { get; set; }

        [DisplayName("Date Created")]
        public DateTime CreateDate { get; set; }

        public string[] Tags { get; set; }
        public string Email { get; set; }
    }
}
