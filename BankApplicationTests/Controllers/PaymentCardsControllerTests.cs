using BankApplication.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BankApplication.DAL;
using System;
using System.Web.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Security;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using BankApplication.Models;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Security.Principal;
using System.Security.Claims;
using System.Collections.Generic;

namespace BankApplication.Controllers.Tests
{
    [TestClass()]
    public class PaymentCardsControllerTests
    {
        RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));

        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));

        [TestMethod()]
        public async Task Index_IsAuthorize()
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.Name, "email1@wp.pl")
                                   }));
            var controller = new PaymentCardsController();
            controller.ControllerContext = new ControllerContext
            {

                HttpContext = new MockHttpContextBase { User = user }
            };

            var result = await controller.Index();

            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

    }

    public class MockHttpContextBase : HttpContextBase
    {
        public override IPrincipal User { get; set; }

    }
}