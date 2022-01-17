using FinalExamMVC.Context;
using FinalExamMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Windows;

namespace FinalExamMVC.Controllers
{
    public class HomeController : Controller
    {
        OnlineShopEntities onlineShopEntities = new OnlineShopEntities();
        public ActionResult Index()
        {
            HomeModel homeModel = new HomeModel();

            homeModel.ListProduct = onlineShopEntities.Products.ToList();
            return View(homeModel);
        }

        [HttpGet]
        public ActionResult Register()
        {


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                var check = onlineShopEntities.Users.FirstOrDefault(s => s.Email == user.Email);
                if (check == null)
                {
                    onlineShopEntities.Configuration.ValidateOnSaveEnabled = false;
                    onlineShopEntities.Users.Add(user);
                    onlineShopEntities.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Email existed";
                    return View();
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var data = onlineShopEntities.Users.Where(s => s.Email.Equals(email) &&
                s.Password.Equals(password)).ToList();
                if (data.Count() > 0)
                {
                    Session["Name"] = data.FirstOrDefault().Name;
                    Session["Email"] = data.FirstOrDefault().Email;
                    Session["UserID"] = data.FirstOrDefault().ID;
                    //Session["IsAdmin"] = data.fi.IsAdmin;
                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.error = "Login failed";

                    return RedirectToAction("Login");
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login");
        }
    }
}