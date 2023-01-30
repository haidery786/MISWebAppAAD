using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MISWebAppAAD
{
    public partial class About : Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.IsAuthenticated)
            {
                PopulateUserDetail();
            }
        }

        /// </summary>
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.Current.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties { RedirectUri = "/" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        protected void SignIn(object sender, EventArgs e)
        {
            SignIn();
        }

        protected void SignOut(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            SignOut();
        }

        public void PopulateUserDetail()
        {
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;

            //You get the user’s first and last name below:
            lblName.Text = userClaims?.FindFirst("name")?.Value;

            // The 'preferred_username' claim can be used for showing the username
            var userName = userClaims?.FindFirst("preferred_username")?.Value;

            lblUsername.Text = userName;

            lblPersonCode.Text = userName.Substring(0, userName.IndexOf("@"));


            // The subject/ NameIdentifier claim can be used to uniquely identify the user across the web
            lblSubject.Text = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            // TenantId is the unique Tenant Id - which represents an organization in Azure AD
            lblTenantId.Text = userClaims?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;
        }

        /// <summary>
        /// Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            HttpContext.Current.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);

            Response.Redirect("https://localhost:44376/");
        }
    }
}