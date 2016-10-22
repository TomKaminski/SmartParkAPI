using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace SmartParkAPI.Controllers
{
    [Route("Error")]
    public class ErrorController : Controller
    {
        [Route("Status/{statusCode}/{pathBase}")]
        public IActionResult StatusCode(int statusCode, string pathBase)
        {
            switch (statusCode)
            {
                case (int)HttpStatusCode.NotFound:
                    return RedirectToAction("Index", "Home", new { area = "Portal" , pathBase});

                case (int)HttpStatusCode.InternalServerError:
                default:
                    return RedirectToAction("UnexpectedError");
            }
        }
    }
}
