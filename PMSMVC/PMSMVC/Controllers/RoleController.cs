using PMSMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace PMSMVC.Controllers
{
    public class RoleController : Controller
    {
        // GET: Roles
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            //Populate DropDown List
            var context = new ApplicationDbContext();
            var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = roleList;

            var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();

            ViewBag.Users = userList;
            ViewBag.Message = "";
            return View();
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(string RoleName)
        {
            var context = new ApplicationDbContext();
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            context.Roles.Remove(thisRole);
            context.SaveChanges();
            return RedirectToAction("Index","Role");
        }

        //Edit Get
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(string RoleName)
        {
            var context = new ApplicationDbContext();
            var thisRole = context.Roles.Where(r => r.Name.Equals(RoleName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
            return View(thisRole);
        }

        // Edit POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(IdentityRole role)
        {
            try
            {
                var context = new ApplicationDbContext();
                context.Entry(role).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Index","Role");
            }
            catch
            {
                return View();
            }
        }

        //Create GET
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        //Create POST
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                var context = new ApplicationDbContext();
                context.Roles.Add(new IdentityRole()
                {
                    Name = collection["RoleName"]
                });
                context.SaveChanges();
                ViewBag.Message = "Role Created Succfully";
                return RedirectToAction("Index","Role");
            }
            catch
            {
                return View();
            }
        }

        //GET User Roles
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult GetRoles(string UserName)
        {
            if (!string.IsNullOrWhiteSpace(UserName))
            {
                var context = new ApplicationDbContext();
                ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

                var userStore = new UserStore<ApplicationUser>(context);
                var userManger = new UserManager<ApplicationUser>(userStore);
                ViewBag.RolesForThisuser = userManger.GetRoles(user.Id);

                //Populate DropDown List
                var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

                ViewBag.Roles = roleList;

                var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();

                ViewBag.Users = userList;

                ViewBag.Message = "Roles Retrieved Successfully!";
            }

            return View("Index");
        }

        //Adding Roles to a user
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult RoleAddToUser(string UserName, string RoleName)
        {
            var context = new ApplicationDbContext();
            if (context == null)
            {
                throw new ArgumentException("context", "Context must not be null");
            }
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManger = new UserManager<ApplicationUser>(userStore);
            userManger.AddToRole(user.Id, RoleName);

            ViewBag.Message = "Role Assigned Successfully";

            //Populate DropDown List
            var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = roleList;

            var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();

            ViewBag.Users = userList;

            return View("Index");

        }

        //Deleting a User From a Roles
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRoleForUser(string UserName, string RoleName)
        {
            var account = new AccountController();
            var context = new ApplicationDbContext();
            ApplicationUser user = context.Users.Where(u => u.UserName.Equals(UserName, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();

            var userStore = new UserStore<ApplicationUser>(context);
            var userManger = new UserManager<ApplicationUser>(userStore);

            if (userManger.IsInRole(user.Id, RoleName))
            {
                userManger.RemoveFromRole(user.Id, RoleName);
                ViewBag.Message = "Role Removed from this user Successfully";
            }
            else
            {
                ViewBag.Message = "This user does't belgong to selected role!!!";
            }
            //Populate DropDown List
            var roleList = context.Roles.OrderBy(r => r.Name).ToList().Select(rr => new SelectListItem { Value = rr.Name.ToString(), Text = rr.Name }).ToList();

            ViewBag.Roles = roleList;

            var userList = context.Users.OrderBy(u => u.UserName).ToList().Select(uu => new SelectListItem { Value = uu.UserName.ToString(), Text = uu.UserName }).ToList();

            ViewBag.Users = userList;

            return View("Index", "Role");


        }
    }
}