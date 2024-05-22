using HttpService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Infrastructure.Dto;
using Infrastructure.Models;
using Infrastructure;
using Newtonsoft.Json;
using QuickFixWeb.Model;
using QuickFixWeb.Pages.Fix;
using System.Reflection;
using Microsoft.AspNetCore.Authorization;

namespace QuickFixWeb.Pages
{
    [Authorize]
    public class IndexModel(AnalyticsHttpService<Tag> tagHttpService,
                            AnalyticsHttpService<Author> authorHttpService) : PageModel
    {
        private readonly AnalyticsHttpService<Tag> _tagHttpService = tagHttpService;
        private readonly AnalyticsHttpService<Author> _authorHttpService = authorHttpService;

        public void OnGet()
        {
        }

        public async Task<PartialViewResult> OnGetSuggestionsPartial(SuggestionListModel model)
        {
            if (model.SuggestionType.Equals("Author"))
                model.Suggestions =await GetSuggestions<Author>(model.SearchText);
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
                _ => throw new NotSupportedException($"Type {typeof(T)} is not supported."),
            };
            var entities = JsonConvert.DeserializeObject<List<Author>>(Convert.ToString(response.Result));
            return entities.Select(x => x.Name).ToList();
        }
    }
}
