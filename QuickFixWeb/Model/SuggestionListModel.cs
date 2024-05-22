namespace QuickFixWeb.Model
{
    public class SuggestionListModel
    {
        public string SuggestionType { get; set; }
        public string SuggestionEdit { get; set; }
        public string SuggestionList { get; set; }
        public string SuggestionPanel { get; set; }
        public string SearchText { get; set; }
        public List<string> Suggestions { get; set; }
    }
}
