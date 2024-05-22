using HttpService.Services.IService;
using HttpService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuickFixWeb.Model;
using Microsoft.AspNetCore.Mvc.Rendering;
using Infrastructure;
using Newtonsoft.Json;
using Infrastructure.Models;
using System.Reflection;
using Infrastructure.Dto;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace QuickFixWeb.Pages.Fix
{
    public class SuggestionsModel(AnalyticsHttpService<Tag> tagHttpService,
                                  AnalyticsHttpService<Author> authorHttpService,
                                  AnalyticsHttpService<Category> categoryHttpService
                                  ) : PageModel
    {
        private readonly AnalyticsHttpService<Tag> _tagHttpService = tagHttpService;
        private readonly AnalyticsHttpService<Author> _authorHttpService = authorHttpService;
        private readonly AnalyticsHttpService<Category> _categoryHttpService = categoryHttpService;

        public void OnGet()
        {
        }

        public async Task<PartialViewResult> OnGetSuggestionsPartial(SuggestionListModel model)
        {
            if (model.SuggestionType.Equals("Author"))
                model.Suggestions = await GetSuggestions<Author>(model.SearchText);
            else
            if (model.SuggestionType.Equals("Category"))
                model.Suggestions = await GetSuggestions<Category>(model.SearchText);
            else
                model.Suggestions = await GetSuggestions<Tag>(model.SearchText);

            return Partial("_SuggestionsPartial", model);
        }

        public async Task<List<string>> GetSuggestions<T>(string filter) where T : BaseNamedEntity
        {
            ResponseDto response = typeof(T).Name switch
            {
                "Author" => await _authorHttpService.Get(filter),
                "Tag" => await _tagHttpService.Get(filter),
                "Category" => await _categoryHttpService.Get(filter),
                _ => throw new NotSupportedException($"Type {typeof(T)} is not supported."),
            };
            var entities = JsonConvert.DeserializeObject<List<Author>>(Convert.ToString(response.Result));
            return entities.Select(x => x.Name).ToList();
        }
    }
}
