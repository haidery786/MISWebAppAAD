using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Notifications;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace MISWebAppAAD.App_Start
{
    public partial class Startup
	{
		// The Client ID is used by the application to uniquely identify itself to Azure AD.
		string clientId = ConfigurationManager.AppSettings["ClientId"];

		// RedirectUri is the URL where the user will be redirected to after they sign in.
		string redirectUri = ConfigurationManager.AppSettings["RedirectUri"];

		// Tenant is the tenant ID (e.g. contoso.onmicrosoft.com, or 'common' for multi-tenant)
		static string tenant = ConfigurationManager.AppSettings["Tenant"];

		// Authority is the URL for authority, composed by Microsoft identity platform endpoint and the tenant name (e.g. https://login.microsoftonline.com/contoso.onmicrosoft.com/v2.0)
		string authority = String.Format(System.Globalization.CultureInfo.InvariantCulture, System.Configuration.ConfigurationManager.AppSettings["Authority"], tenant);

		/// <summary>
		/// Configure OWIN to use OpenIdConnect 
		/// </summary>
		/// <param name="app"></param>
		public void ConfigureAuth(IAppBuilder app)
		{
			app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

			app.UseCookieAuthentication(new CookieAuthenticationOptions());
			app.UseOpenIdConnectAuthentication(
			new OpenIdConnectAuthenticationOptions
			{
				// Sets the ClientId, authority, RedirectUri as obtained from web.config
				ClientId = clientId,
				Authority = authority,
				RedirectUri = redirectUri,
				// PostLogoutRedirectUri is the page that users will be redirected to after sign-out. In this case, it is using the home page
				PostLogoutRedirectUri = redirectUri,
				Scope = OpenIdConnectScope.OpenIdProfile,
				// ResponseType is set to request the code id_token - which contains basic information about the signed-in user
				ResponseType = OpenIdConnectResponseType.CodeIdToken,
				// OpenIdConnectAuthenticationNotifications configures OWIN to send notification of failed authentications to OnAuthenticationFailed method
				Notifications = new OpenIdConnectAuthenticationNotifications
				{
					AuthenticationFailed = OnAuthenticationFailed
				}
			}
		);
		}

		/// <summary>
		/// Handle failed authentication requests by redirecting the user to the home page with an error in the query string
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> context)
		{
			context.HandleResponse();
			context.Response.Redirect("/?errormessage=" + context.Exception.Message);
			return Task.FromResult(0);
		}
	}

	//   public partial class Startup
	//{
	//	private static string clientId = ConfigurationManager.AppSettings["ida:ClientId"];
	//	private static string aadInstance = EnsureTrailingSlash(ConfigurationManager.AppSettings["ida:AADInstance"]);
	//	private static string tenantId = ConfigurationManager.AppSettings["ida:TenantId"];
	//	private static string postLogoutRedirectUri = ConfigurationManager.AppSettings["ida:PostLogoutRedirectUri"];

	//	string authority = aadInstance + tenantId;

	//	public void ConfigureAuth(IAppBuilder app)
	//	{
	//		app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

	//		app.UseCookieAuthentication(new CookieAuthenticationOptions());

	//		app.UseOpenIdConnectAuthentication(
	//		new OpenIdConnectAuthenticationOptions
	//		{
	//			ClientId = clientId,
	//			Authority = authority,
	//			PostLogoutRedirectUri = postLogoutRedirectUri,

	//			Notifications = new OpenIdConnectAuthenticationNotifications()
	//			{
	//				AuthenticationFailed = (context) =>
	//				{
	//					return System.Threading.Tasks.Task.FromResult(0);
	//				},
	//				SecurityTokenValidated = (context) =>
	//				{
	//					var claims = context.AuthenticationTicket.Identity.Claims;
	//					var groups = from c in claims
	//								 where c.Type == "groups"
	//								 select c;

	//					foreach (var group in groups)
	//					{
	//						context.AuthenticationTicket.Identity.AddClaim(new Claim(ClaimTypes.Role, group.Value));
	//					}
	//					return Task.FromResult(0);
	//				}
	//			}

	//		}
	//		);

	//		// This makes any middleware defined above this line run before the Authorization rule is applied in web.config
	//		app.UseStageMarker(PipelineStage.Authenticate);
	//	}

	//	private static string EnsureTrailingSlash(string value)
	//	{
	//		if (value == null)
	//		{
	//			value = string.Empty;
	//		}

	//		if (!value.EndsWith("/", StringComparison.Ordinal))
	//		{
	//			return value + "/";
	//		}

	//		return value;
	//	}
	//}
}