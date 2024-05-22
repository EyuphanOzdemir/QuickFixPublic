namespace Infrastructure.Utility
{
    public class GlobalValues
    {
        public static string AnalyticsAPIBase { get; set; }
        public static string AuthAPIBase { get; set; }
        public static string FixAPIBase { get; set; }

        public static string StripeAPIBase { get; set; }

        public const string RoleAdmin = "ADMIN";
        public const string RoleCustomer = "CUSTOMER";
        public const string TokenCookie = "JWTToken";
        public enum ApiType
        {
            GET,
            POST,
            PUT,
            DELETE
        }
        public enum ContentType
        {
            Json,
            MultipartFormData,
        }

        public static string GoogleClientId { get; set; }
        public static string GoogleClientSecret { get; set;}
    }
}
