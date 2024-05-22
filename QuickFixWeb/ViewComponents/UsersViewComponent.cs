using Microsoft.AspNetCore.Mvc;
using Infrastructure.Models.Dto;
using Newtonsoft.Json;
using QuickFixWeb.Services.IService;
using System.Threading.Tasks;

namespace QuickFixWeb.ViewComponents
{
    public class UsersViewComponent:ViewComponent
    {
        IFixService _fixService;
        public UsersViewComponent(IFixService fixService)
        {
            _fixService = fixService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string x)
        {
            List<string> users = new List<string>();
            var response = await _fixService.ListAllAsync();
            if (response != null && response.IsSuccess)
            {
                var FixDtoList = JsonConvert.DeserializeObject<List<FixDto>>(Convert.ToString(response.Result));
                foreach(var fixDto in FixDtoList)
                {
                    users.Add(fixDto.Category);
                }
            }
            return View(users);
        }
    }
}
