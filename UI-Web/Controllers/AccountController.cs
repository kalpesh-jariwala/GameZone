using BLL_Core.DbClasses;
using BLL_Core.Infrastructure.ExtensionMethods;
using BLL_Core.ModelClassess.Account;
using BLL_Core.RavenDocuments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace UI_Web.Controllers
{
    public class AccountController : BaseController
    {
        [HttpGet]
        public ActionResult _Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult _Login(LoginInput model)
        {
            try
            {
                var accountDb = new AccountDb(Session);
                if(ModelState.IsValid && accountDb.Login(model.UserName.ToLower().Trim(), model.Password, model.RememberMe))
                {
                    var authCookie =
                   System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                    if (authCookie != null)
                    {
                        var authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                        if (authTicket != null && !authTicket.Expired)
                        {
                            var newAuthTicket = authTicket;

                            if (FormsAuthentication.SlidingExpiration)
                            {
                                newAuthTicket = FormsAuthentication.RenewTicketIfOld(authTicket);
                            }
                            if (newAuthTicket != null)
                            {
                                var userData = newAuthTicket.UserData;
                                var roles = userData.Split(',');

                                System.Web.HttpContext.Current.User =
                                    new System.Security.Principal.GenericPrincipal(new FormsIdentity(newAuthTicket), roles);
                            }
                        }
                    }
                    return View("_LoginNavigation");
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        UserDocument userDoc = null;
                        try
                        {
                            userDoc = Session.Query<UserDocument>().Where(e => e.Username.Equals(model.UserName.ToLower())).SingleOrDefault();
                            if (userDoc.IsNotNull() && userDoc.Password.IsNullOrEmpty() && userDoc.Salt.IsNullOrEmpty())
                            {
                                TempData["Message"] = "Your account is inactive now, Please register first to activate your account.";
                                ModelState.AddModelError("", "Your account is inactive now, Please register first to activate your account.");
                                return View(model);
                                //return model;
                            }
                        }
                        catch (Exception e)
                        {
                            userDoc = null;
                        }
                    }
                }

                return RedirectToAction("_Login","Account");
            }
            catch (Exception ex)
            {
                return View();
            }
      
        }

        // GetUser
        public ActionResult GetUserInfo(int id)
        {
            return View();
        }

        //UpdateUser
        [HttpPost]
        public ActionResult UpdateUserInfo(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        // POST: Account/Delete/5
        [HttpPost]
        public ActionResult DeleteAccount(string UserName)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
