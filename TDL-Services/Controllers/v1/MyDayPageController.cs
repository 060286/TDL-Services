using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TDL.APIs.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Route("api/v{version:apiVersion}/users")]
    public class MyDayPageController : BaseController
    {
        public MyDayPageController()
        {
        }

        [AllowAnonymous]
        [HttpGet("tesing-my-day")]
        public IActionResult Test()
        {
            var result = new MyDay(1, "test");

            return Ok(result);
        }
    }
    
    public class MyDay
    {
        public MyDay(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
