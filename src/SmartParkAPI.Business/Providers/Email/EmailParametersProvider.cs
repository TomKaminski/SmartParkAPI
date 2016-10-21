using System.Collections.Generic;
using SmartParkAPI.Contracts.DTO.User;

namespace SmartParkAPI.Business.Providers.Email
{
    public static class EmailParametersProvider
    {

        public static Dictionary<string, string> GetParameters(UserBaseDto userData,
            Dictionary<string, string> additionalParameters = null)
        {
            var baseParameters = new Dictionary<string, string>
            {
                {"Name",userData.Name },
                {"LastName",userData.LastName },
                {"Id",userData.Id.ToString() },
            };

            if (additionalParameters != null)
            {
                foreach (var additionalParameter in additionalParameters)
                {
                    baseParameters.Add(additionalParameter.Key, additionalParameter.Value);
                }

            }


            return baseParameters;
        }

        
    }
}
