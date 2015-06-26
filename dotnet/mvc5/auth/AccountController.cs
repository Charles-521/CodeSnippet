
    [Authorize]
    public class AccountController : BaseController
    {
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Json(new { result = "Redirect", url = Url.Action("Login", "Account") });
        }

        [AllowAnonymous]
        // Post: /Account/Login
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (User.Identity.IsAuthenticated)
                return Json(new { result = "Redirect", url = Url.Action("Test", "Account") });

            var userRepository = DependencyResolver.Current.GetService<UserRepository>();

            var user = userRepository.GetUsers().SingleOrDefault(o => o.LoginID == model.LoginID);

            if (user == null)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "LoginId/Password is incorrect");

            if (user.Status == EnumDef.AccountStatus.Inactive.ToByte())
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Account is inactived. Please contact administrator");

            if (user.Status == EnumDef.AccountStatus.Suspended.ToByte())
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Account suspended due to maxium retry login. Please contact administrator");

            if (user.RetryCount >= 3)
            {
                user.Status = EnumDef.AccountStatus.Suspended.ToByte();

                userRepository.Complete();
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError,
                    "Account suspended due to maxium retry login. Please contact administrator");
            }
            var securityService = DependencyResolver.Current.GetService<SecurityService>();

            var isAuth = securityService.VerifyHash(model.Password, user.Salt, user.Password);

            if (!isAuth)
            {
                user.RetryCount++;

                userRepository.Complete();

                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "LoginId/Password is incorrect");
            }

            dynamic result;

            var roles = user.T_ePortal_UserRole.Select(o => o.T_ePortal_Role);

            var userData = new UserRoleModel
            {
                UserID = user.UserID,
                Roles = roles.Select(o => new Role { RoleID = o.RoleID, Name = o.Name }).ToList(),
                IsPwdChangeRequired = user.IsPwdChangeRequired
            };

            //create the authentication ticket
            var authTicket = new FormsAuthenticationTicket(
                1,
                model.LoginID, //user id
                DateTime.Now,
                DateTime.Now.AddMinutes(15), // expiry
                model.RememberMe, //true to remember
                userData.ToJson(), //roles
                "/"
                );
            var value = FormsAuthentication.Encrypt(authTicket);
            //encrypt the ticket and add it to a cookie
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, value);
            Response.Cookies.Add(cookie);

            user.LastLoginDate = DateTime.Now;
            user.LastLoginIPAddr = Request.UserHostAddress;
            userRepository.Complete();

            if (user.Status == EnumDef.AccountStatus.Active.ToByte())
                result = new { result = "Redirect", url = Url.Action("Test", "Account") };
            else if (user.IsPwdChangeRequired)
                result = new { result = "Redirect", url = Url.Action("ResetPassword", "User") };
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Invalid operation detected");
            }

            return Json(result);
        }

        [Authorize(Roles = ("Marketing Analysis, Administrator, Guest Relation, Announcement,Content Management"))]
        public ActionResult Test()
        {
            return View();
        }
    }