using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SmartParkAPI.Controllers.Admin.Base
{
    [Authorize(Policy = "AdminUser")]
    [Route("api/Admin/[controller]/[action]")]
    public class AdminBaseController : BaseApiController
    {
    }
}
