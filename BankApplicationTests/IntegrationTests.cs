using BankApplication;
using BankApplication.Controllers;
using BankApplication.Controllers.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
namespace BankApplication.Tests
{
    [TestClass()]
    public class IntegrationTests
    {
        ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] {
                                        new Claim(ClaimTypes.Role, "Admin")
                                   }));

        [TestMethod()]
        public async Task IntegrationTest()
        {
            var controller = new PaymentCardsController();
            controller.ControllerContext = new ControllerContext
            {

                HttpContext = new MockHttpContextBase { User = user }
            };

            var result = await controller.Index() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);
            Assert.IsTrue(result.ViewName == "Index" || result.ViewName == "");

        }
    }
}