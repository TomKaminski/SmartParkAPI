using Microsoft.AspNetCore.Mvc;

namespace SmartParkAPI.Controllers.Admin.Base
{
    [Area("Admin")]
    //[Authorize(Policy = "Admin")]
    [Route("[area]/[controller]/[action]")]
    public class AdminBaseController:BaseApiController
    {
    }
}
