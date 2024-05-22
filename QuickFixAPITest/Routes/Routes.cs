namespace QuickFixAPITest;
public static class Routes
{
	private const string BaseRoute = "/api";

	public static class Fixes
	{
		private const string BaseRoute = Routes.BaseRoute + "/fix";

		public const string List = BaseRoute;
		public const string Add = BaseRoute + "/add";
		public static string Update(string id) => $"{BaseRoute}/update/{id}";

		public static string Find(string id) => $"{BaseRoute}/find/{id}";

		public static string Delete(string id) => $"{BaseRoute}/delete/{id}";
	}
}
