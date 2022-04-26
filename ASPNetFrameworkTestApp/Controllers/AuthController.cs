using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Mvc;
using System.Linq;
using Owin;
using System.Security.Claims;
using RestSharp;
using System.Threading.Tasks;
using System;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web;

namespace ASPNetFrameworkTestApp.Controllers
{
    public class AuthController : Controller
    {
        //private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="HomeController"/> class.
        /// </summary>
        /// <param name="configuration">IConfiguration instance.</param>
        //public AuthController(
        //    IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        /// <summary>
        /// Authentication start
        /// </summary>
        /// <returns>current View</returns>
        public ActionResult Start()
        {
            ViewBag.AzureClientId = ConfigurationManager.AppSettings["ida:ClientId"];
            return View();
        }

        /// <summary>
        /// Authentication End
        /// </summary>
        /// <returns>current View</returns>
        public ActionResult End()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OnPostConfirmationAsync(string accessToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(accessToken);
            var tokenS = jsonToken as JwtSecurityToken;

            //var scopes = tokenS.Claims.First(claim => claim.Type == "scp").Value;
            //var userId = tokenS.Claims.First(claim => claim.Type == "oid").Value;
            //var tenantId = tokenS.Claims.First(claim => claim.Type == "tid").Value;
            //var userName = tokenS.Claims.First(claim => claim.Type == "upn").Value;
            //var audience = tokenS.Claims.First(claim => claim.Type == "aud").Value;
            //var tokenService = tokenS.Claims.First(claim => claim.Type == "iss").Value;
            //var authClaim = tokenS.Claims.First(claim => claim.Type == "acr").Value;

            var identity = new ClaimsIdentity(tokenS.Claims, "basic");
            HttpContext.User = new ClaimsPrincipal(identity);
            HttpContext.GetOwinContext().Authentication.Challenge(new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);

            return RedirectToAction("About", "Home");
        }

        private static string EnsureTrailingSlash(string value)
        {
            if (value == null)
            {
                value = string.Empty;
            }

            if (!value.EndsWith("/", StringComparison.Ordinal))
            {
                return value + "/";
            }

            return value;
        }
    }
}