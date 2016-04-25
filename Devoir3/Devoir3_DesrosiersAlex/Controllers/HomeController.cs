
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Devoir3_DesrosiersAlex.Controllers
{
    public class HomeController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();


        public ActionResult Index(String id, String psw, String rememberme)
        {
          
           

            if (Request.Cookies["userName"] != null)
            {
                if (Request.Cookies["userName"]["Pref"] != null)
                {
                    ViewBag.Name = Request.Cookies["userName"]["Id"];
                    ViewBag.CheckBoxValue = "checked";
                }
            }
            else
            {
                ViewBag.Name = "";
                ViewBag.CheckBoxValue = "";
            }


            if (id != null && psw != null)
            {
                foreach (Users u in db.Users)
                {
                    if (u.Login == id && u.Password == psw)
                    {
                        if (rememberme == "yes")
                        {
                            Response.Cookies["userName"]["Pref"] = "true";
                            Response.Cookies["userName"]["Id"] = u.Login;
                            Response.Cookies["userName"].Expires = DateTime.Now.AddDays(7);
                        }

                        Session["userSession"] = u.Nom + u.Prenom;
                        // Response.Redirect("/AddFile/AddFileView");

                        return RedirectToAction("Index", "Products");
                    }
                }
            }

            


            // Response.Redirect("~/Views/Home/Index.cshtml");

            return View();
        }

    
    }
}