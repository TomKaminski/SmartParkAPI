using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartParkAPI.Contracts.Services;
using SmartParkAPI.Infrastructure.Attributes;
using SmartParkAPI.Models;
using SmartParkAPI.Models.Base;

namespace SmartParkAPI.Controllers
{
    [Route("api/Manage")]
    [ApiHeaderAuthorize]
    public class ManageApiController : BaseApiController
    {
        private readonly IUserService _userService;

        public ManageApiController(IUserService userService)
        {
            _userService = userService;
        }

        [Route("ChangePassword")]
        [HttpPost]
        public async Task<SmartJsonResult<bool>> ChangePassword([FromBody] ChangePasswordApiModel model)
        {
            if (!ModelState.IsValid)
                return SmartJsonResult<bool>.Failure(GetModelStateErrors(ModelState));

            var changePasswordResult =
                await _userService.ChangePasswordAsync(model.Email, model.OldPassword, model.NewPassword);

            return changePasswordResult.IsValid
                ? SmartJsonResult<bool>.Success(true)
                : SmartJsonResult<bool>.Failure(changePasswordResult.ValidationErrors);
        }

        [Route("ChangeEmail")]
        [HttpPost]
        public async Task<SmartJsonResult<bool>> ChangeEmail([FromBody] ChangeEmailApiModel model)
        {
            if (!ModelState.IsValid)
                return SmartJsonResult<bool>.Failure(GetModelStateErrors(ModelState));

            var changeEmailResult = await _userService.ChangeEmailAsync(model.Email, model.NewEmail, model.Password);

            return changeEmailResult.IsValid
                ? SmartJsonResult<bool>.Success(true)
                : SmartJsonResult<bool>.Failure(changeEmailResult.ValidationErrors);
        }
    }
}
