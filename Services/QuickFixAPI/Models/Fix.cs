using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace QuickFixAPI.Models
{
    public class Fix:BaseEntity
    {
		[BsonElement("category")]
		[Required(ErrorMessage = "Category is required")]
		public string Category { get; set; }

		[BsonElement("author")]
		[Required(ErrorMessage = "Author is required")]
		public string Author { get; set; }

		[BsonElement("title")]
		[Required(ErrorMessage = "Title is required")]
		public string Title { get; set; }

        [BsonElement("email")]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [BsonElement("solution")]
		[Required(ErrorMessage = "Solution is required")]
		public string Solution { get; set; }

		[BsonElement("tags")]
		public string[] Tags { get; set; }

		public DateTime CreateDate { get; set; }

		public string CombinedTags => string.Join(" ", Tags?.ToList() ?? []);
    }
}
