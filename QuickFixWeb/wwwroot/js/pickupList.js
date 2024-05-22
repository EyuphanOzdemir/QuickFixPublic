function searchTermKeyup(SuggestionType, SuggestionPanel, SuggestionList, SuggestionEdit, SearchText, url)
{
    console.log(SearchText); // Log the text to the console
    $("#" + SuggestionList).empty();

    if (SearchText.length > 2) {
        var searchConfig = {
            SuggestionType,
            SuggestionPanel,
            SuggestionList,
            SuggestionEdit,
            SearchText
        };

        var fullUrl = url + "?" + $.param(searchConfig);
        console.log(fullUrl);

        $.ajax({
            type: "GET",
            url: fullUrl,
            contentType: "application/json",
            success: function (response) {
                $("#" + SuggestionPanel).html(response);
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    }
};

function itemClicked(suggestionList, suggestionEdit, selected)
{
    console.log("Clicked on list item with text: " + selected);
    $("#" + suggestionEdit).val(selected);
    $("#" + suggestionList).empty();
}
