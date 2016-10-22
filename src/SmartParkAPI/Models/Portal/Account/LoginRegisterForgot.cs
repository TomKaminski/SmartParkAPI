namespace SmartParkAPI.Models.Portal.Account
{
    public class LoginRegisterForgot
    {
        public LoginRegisterForgot(string returnUrl)
        {
            LoginViewModel = new LoginViewModel {ReturnUrl = returnUrl};
        }
        public LoginViewModel LoginViewModel { get; set; }
        public RegisterViewModel RegisterViewModel { get; set; }
        public ForgotPasswordViewModel ForgotPasswordViewModel { get; set; }

    }
}
