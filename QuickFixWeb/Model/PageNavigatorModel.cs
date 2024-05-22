namespace QuickFixWeb.Model
{
    public class PageNavigatorModel
    {
        public string SearchButtonName { get; set; }
        public int PageCount { get; set; }

        public long RowCount { get; set; }

        public int PageIndex { get; set; } = 1;
        public string PageInputName {  get; set; }
    }
}
