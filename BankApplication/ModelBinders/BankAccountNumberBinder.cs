using BankApplication.DAL;
using BankApplication.Models;
using System;
using System.Web.Mvc;

namespace BankApplication.ModelBinders
{
    public class BankAccountNumberBinder : IModelBinder
    {
        private BankContext db = new BankContext();

        public object BindModel(ControllerContext actionContext, ModelBindingContext bindingContext)
        {
            var request = actionContext.HttpContext.Request;

            int ID = int.Parse(request.Form.Get("ID"));
            decimal Balance = decimal.Parse(request.Form.Get("Balance"));
            decimal AvailableFounds = decimal.Parse(request.Form.Get("AvailableFounds"));
            decimal Lock = decimal.Parse(request.Form.Get("Lock"));
            decimal BankAccountNumber = decimal.Parse(request.Form.Get("BankAccountNumber").Trim());
            DateTime CreationDate = DateTime.Parse(request.Form.Get("CreationDate"));
            int BankAccountTypeID = int.Parse(request.Form.Get("BankAccountTypeID"));
            string FileName = request.Form.Get("FileName");

            BankAccountType BankAccountType = db.BankAccountTypes.Find(BankAccountTypeID);

            return new BankAccount {
                ID = ID,
                Balance = Balance,
                AvailableFounds = AvailableFounds,
                Lock = Lock,
                BankAccountNumber = BankAccountNumber,
                CreationDate = CreationDate,
                BankAccountTypeID = BankAccountTypeID,
                FileName = FileName,
                BankAccountType = BankAccountType
            };
        }
    }
}