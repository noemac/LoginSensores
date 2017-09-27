using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using sensoresapp.Models;
using Newtonsoft.Json;

namespace sensoresapp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private CustomUserManager CustomUserManager { get; set; }

        public AccountController()
            : this(new CustomUserManager())
        {
        }

        public AccountController(CustomUserManager customUserManager)
        {
            CustomUserManager = customUserManager;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                var LoginCorrecto = false;

                /*Implementar llamado a web api*/

                //Login con webAPI
                #region Login con WebAPI

                var usuario = model.UserName;
                var contrasena = model.Password;
                var usuarioId = 0;

                string url = string.Format("http://192.168.0.173:8080/granja/usuarios/login?username={0}&password={1}", usuario, contrasena);

                using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    System.Net.Http.HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        dynamic results = JsonConvert.DeserializeObject<dynamic>(data);
                        var id = Convert.ToInt32(results.id);
                        var name = Convert.ToString(results.userName);

                        usuarioId = id;
                        LoginCorrecto = true;
                    }
                }
                #endregion

                //Traer sensores y volcarlo en una grilla

                

                ApplicationUser user = null;

                if (LoginCorrecto)
                {
                    //user = await CustomUserManager.FindAsync(model.UserName, model.Password);
                    user = new ApplicationUser { Id = usuarioId.ToString(), UserName = usuario };
                }

                if (user != null)
                {
                    await SignInAsync(user, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", "Usuario o contraseña inválidos.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && CustomUserManager != null)
            {
                CustomUserManager.Dispose();
                CustomUserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            var identity = await CustomUserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);

            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Sensor");
            }
        }

        #endregion
    }
}