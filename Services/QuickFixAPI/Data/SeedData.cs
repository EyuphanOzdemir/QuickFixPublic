using QuickFixAPI.Models;

namespace QuickFixAPI.Data;

public static class SeedData
{
  public static List<Fix> Fixes()
  {
    var fixes = new List<Fix> {
				new() { Id = "1", Category = "Category 1", Author = "Author 1", Title = "Title 1", Solution = "Solution 1", Tags = ["tag1", "tag2"] },
				new() { Id = "2", Category = "Category 2", Author = "Author 2", Title = "Title 2", Solution = "Solution 2", Tags = ["tag3", "tag4"] },
				new() { Id = "3", Category = "Category 3", Author = "Author 3", Title = "Title 3", Solution = "Solution 3", Tags = ["tag5", "tag6"] }
				};

	return fixes;
  }
}
