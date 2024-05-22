using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;

namespace QuickFixAPI.Models
{
	public class BaseEntity
	{
		[JsonPropertyName("_id")]
		[BsonRepresentation(BsonType.ObjectId)]
		public string Id { get; set; }
	}
}
