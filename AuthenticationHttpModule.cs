 public class AuthenticationHttpModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            context.PostAuthenticateRequest += OnRequestAuthenticate;
        }

        public void OnRequestAuthenticate(object sender, EventArgs e)
        {
            var httpApplication = (HttpApplication)sender;
            var authCookie = httpApplication.Context.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                var authTicket = FormsAuthentication.Decrypt(authCookie.Value);
                var username = authTicket.Name;
                var user = authTicket.UserData.FromJson<UserRoleModel>();
                var userRole = user.Roles.Select(o => o.Name);
                HttpContext.Current.User = new GenericPrincipal(
                    new CustomIdentity(username, user.UserID, user.IsPwdChangeRequired), userRole.ToArray());
            }
        }

        public void Dispose()
        {
        }
    }