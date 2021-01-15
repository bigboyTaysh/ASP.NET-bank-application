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
using System.Net;
using System.Web.Helpers;

namespace BankApplication.Controllers.Tests
{
    [TestClass()]
    public class PaymentCardsControllerTests
    {

        /*
        RoleManager<IdentityRole> roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));

        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(new ApplicationDbContext()));
        */

        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.Name, "email1@wp.pl")
                                   }));

        [TestMethod()]
        public async Task IndexTest()
        {

            var controller = new PaymentCardsController();
            controller.ControllerContext = new ControllerContext
            {

                HttpContext = new MockHttpContextBase { User = user }
            };

            var result = await controller.Index();

            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        [TestMethod()]
        public async Task EditTest()
        {
            var controller = new PaymentCardsController();
            var result = await controller.Edit(new PaymentCard());

            Assert.IsInstanceOfType(result, typeof(ActionResult));
        }

        [TestMethod()]
        public void CardSecuredTest_BadApiKey()
        {

            var controller = new PaymentCardsController();
            var result = controller.CardSecured("badkey", "cardnumber", "code") as HttpStatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.Forbidden, result.StatusCode);
        }

        [TestMethod()]
        public void CardSecuredTest_BadCardNumber()
        {

            var controller = new PaymentCardsController();
            var result = controller.CardSecured("06b9e986-9609-4892-933f-9ced84f3e1c8", "cardnumber", "code") as HttpStatusCodeResult;
            Assert.AreEqual((int)HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod()]
        public void CardSecuredTest_StatusAsJSON()
        {

            var controller = new PaymentCardsController();
            var result = controller.CardSecured("06b9e986-9609-4892-933f-9ced84f3e1c8", "1234 1234 1234 1230", "0321");
            Assert.IsInstanceOfType(result, typeof(JsonResult));
        }

        [TestMethod()]
        public async Task CardPaymentTest_NullParametr()
        {
            var controller = new PaymentCardsController();
            var result = await controller.CardPayment(0, "key", "") as RedirectToRouteResult;
            //Assert.IsTrue(result.RouteValues.Any(r => r.Value == "Index"));
            Assert.IsTrue((string)result.RouteValues["action"] == "Index");
        }

        [TestMethod()]
        public async Task CardPaymentTest_BadApiKey()
        {
            var controller = new PaymentCardsController();
            var result = await controller.CardPayment(0, "badkey", "cardnumber") as RedirectToRouteResult;
            Assert.IsTrue((string)result.RouteValues["action"] == "Index");
        }

        [TestMethod()]
        public void CardPaymentConfirmationTest_NullTempData()
        {
            var controller = new PaymentCardsController();
            var result = controller.CardPaymentConfirmation() as RedirectToRouteResult;
            Assert.IsTrue((string)result.RouteValues["action"] == "Index");
        }

        [TestMethod()]
        public void CardPaymentConfirmationTest_DefiniedTempData()
        {
            var controller = new PaymentCardsController();
            controller.TempData.Add("orderId", 0);
            controller.TempData.Add("cardNumber", "cardNumber");
            controller.TempData.Add("price", 1m);
            controller.TempData.Add("currency", "currency");
            controller.TempData.Add("date", new DateTime());
            controller.TempData.Add("acquirer", new Acquirer());

            var result = controller.CardPaymentConfirmation() as ViewResult;
            Assert.IsTrue(result.ViewName == "Payment");
        }

        [TestMethod()]
        public void StatusTest_NullTempData()
        {
            var controller = new PaymentCardsController();
            var result = controller.Status(false) as RedirectToRouteResult;
            Assert.IsTrue((string)result.RouteValues["action"] == "Index");
        }
    }

    public class MockHttpContextBase : HttpContextBase
    {
        public override IPrincipal User { get; set; }

    }
}