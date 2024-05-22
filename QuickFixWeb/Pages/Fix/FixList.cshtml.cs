using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuickFixWeb.Services.IService;
using Infrastructure;
using Infrastructure.Models.Dto;
using Infrastructure.Dto;
using Newtonsoft.Json;
using System;
using Microsoft.AspNetCore.Mvc;
using QuickFixWeb.Model;
using HttpService.Services.IService;
using Microsoft.AspNetCore.Mvc.Rendering;
using Azure;
using HttpService.Services;
using System.Globalization;
using Azure.Identity;

namespace QuickFixWeb.Pages.Fix
{
    [Authorize]
    public class FixListModel(ILogger<FixListModel> logger, 
                              IFixService fixService,
                              AnalyticsHttpService<Category> categoryHttpService
                              ) : PageModel
    {
        private readonly ILogger<FixListModel> _logger = logger;
        private readonly IFixService _fixService = fixService;
        private readonly AnalyticsHttpService<Category> _categoryHttpService = categoryHttpService;

        [BindProperty(SupportsGet = true)]
        public string Searcher { get; set; }

        [BindProperty]
        public long RowCount { get; set; }

        [BindProperty]
        public int PageCount { get; set; }

        [BindProperty]
        public List<SelectListItem> CategoryList { get; set; }

        [BindProperty]
        public List<FixDto> FixDtoList { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public SearchFixParams SearchFixParams { get; set; }

        public async Task<IActionResult> OnGet()
        {
            await Initialize();

            if (Searcher != "JS")
            {
                Reset();
            }
            Searcher = "";
            var response = await _fixService.Search(SearchFixParams);
            if (response != null && response.IsSuccess)
            {
                var result = JsonConvert.DeserializeObject<SearchFixResult>(Convert.ToString(response.Result));
                FixDtoList = result.Fixes;
                RowCount = result.Count;
                PageCount = (int) Math.Ceiling((double)result.Count / 10d);
            }
            return Page();
        }

        private async Task Initialize()
        {
            //category select list
            CategoryList ??= [];
            var response = await _categoryHttpService.Get();
            var categories = JsonConvert.DeserializeObject<List<Category>>(Convert.ToString(response.Result));
            CategoryList.AddRange(categories.Select(c => new SelectListItem() { Text = c.Name }).ToList());

            //Searc fix params
            SearchFixParams ??= new();
        }

        private void Reset()
        {
            SearchFixParams.PageId = 1;
        }
    }
}
