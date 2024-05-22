function reset() {
    var searcher = document.getElementById("Searcher");
    searcher.value = "";
}

//updatePage('first', '@Model.SearchButtonName', @Model.PageIndex, '@Model.PageInputName', @Model.PageCount)
function updatePage(e, direction, searchButtonName, pageIndex, pageInputName, pageCount)
{
    e.preventDefault();
    // Get the current page number
    var currentPageNumber = pageIndex;
    var targetPageNumber = 1;
    if (direction === "previous")
        targetPageNumber = currentPageNumber - 1 > 0 ? currentPageNumber - 1 : 1;
    else if (direction === "next")
        targetPageNumber = currentPageNumber + 1 <= pageCount ? currentPageNumber + 1 : pageCount;
    else if (direction === "last")
        targetPageNumber = pageCount;

    var pageInput = document.getElementById(pageInputName);
    pageInput.value = targetPageNumber;
    var searcher = document.getElementById("Searcher");
    searcher.value = "JS";

    var searchButton = document.getElementById(searchButtonName);
    searchButton.click();
}

