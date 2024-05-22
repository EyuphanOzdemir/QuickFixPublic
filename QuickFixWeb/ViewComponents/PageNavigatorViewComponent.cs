using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models.Dto;
using Newtonsoft.Json;
using QuickFixWeb.Model;
using QuickFixWeb.Services.IService;
using System.Threading.Tasks;

namespace QuickFixWeb.ViewComponents
{
    public class PageNavigatorViewComponent:ViewComponent
    {
        public PageNavigatorViewComponent()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync(PageNavigatorModel model)
        {
            await Task.Delay(1);
            return View("Default",model);
        }
    }
}
