using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using SmartParkAPI.Shared.Extensions;

namespace SmartParkAPI.Models
{
    public class AppUserState
    {
        public AppUserState()
        {
            
        }

        public AppUserState(JwtSecurityToken token)
        {
            var claims = token.Claims.ToList();

            Email = claims.FirstOrDefault(x => x.Type.ToLower() == "username")?.Value;
            UserId = claims.FirstOrDefault(x => x.Type.ToLower() == "userid")?.Value.ToNullable<int>();
            Name = claims.FirstOrDefault(x => x.Type.ToLower() == "name")?.Value;

            var photoIdClaim = claims.FirstOrDefault(x => x.Type.ToLower() == "photoid");
            if (photoIdClaim == null || photoIdClaim.Value == "")
            {
                PhotoId = null;
            }
            else
            {
                PhotoId = new Guid(photoIdClaim.Value);
            }

            LastName = claims.FirstOrDefault(x => x.Type.ToLower() == "lastname")?.Value;

            var isAdminClaim = claims.FirstOrDefault(x => x.Type.ToLower() == "isadmin");
            if (isAdminClaim != null)
            {
                IsAdmin = Convert.ToBoolean(isAdminClaim.Value);
            }

            var isSidebarShrinked = claims.FirstOrDefault(x => x.Type.ToLower() == "sidebarshrinked");
            if (isSidebarShrinked != null)
            {
                SidebarShrinked = Convert.ToBoolean(isSidebarShrinked.Value);
            }
        }

        public int? UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public bool SidebarShrinked { get; set; }
        public Guid? PhotoId { get; set; }

        public bool IsAuthenticated()
        {
            return string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Name);
        }

        public override string ToString()
        {
            return $"{Name} {LastName}";
        }
    }
}
