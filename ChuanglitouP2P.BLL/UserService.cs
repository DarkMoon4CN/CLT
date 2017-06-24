using ChuanglitouP2P.Model;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChuanglitouP2P.BLL
{
    public class UserService
    {

        public static ClaimsIdentity CreateIdentity(M_login user, string authenticationType)
        {
            ClaimsIdentity _identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
            _identity.AddClaim(new Claim(ClaimTypes.Name, user.username));
            _identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.userid.ToString()));
            _identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity"));
            _identity.AddClaim(new Claim("DisplayName", user.username));
            

            return _identity;
        }


    }
}
