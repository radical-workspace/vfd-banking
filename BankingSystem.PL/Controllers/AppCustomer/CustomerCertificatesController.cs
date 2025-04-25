using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.BLL.Services;
using BankingSystem.DAL.Data;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Runtime.ConstrainedExecution;
namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerCertificatesController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;

        public CustomerCertificatesController(IUnitOfWork UnitOfWork, IMapper mapper)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
        }

        public IActionResult Details(string id)
        {
            var customer = _UnitOfWork.Repository<Customer>()
                         .GetSingleDeepIncluding(c => c.Id == id,
                          q => q.Include(c => c.Accounts).ThenInclude(a => a.Certificates)
                          .ThenInclude(c => c.GeneralCertificate));

            if (customer != null)
            {
                var CertificatesModel = _mapper.Map<List<CustomerCertificatesViewModel>>(customer.Accounts?.SelectMany(acc => acc.Certificates) ?? Enumerable.Empty<Certificate>());
                ViewBag.id = customer.Id;
                return View(CertificatesModel);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }


        [HttpGet]
        public IActionResult ApplyCertificate(string id)
        {

            var allcertificates = _UnitOfWork.Repository<GeneralCertificate>().GetAll().ToList();
            var customer = _UnitOfWork.Repository<Customer>().
                GetSingleIncluding(c => c.Id == id, c => c.Accounts);

            var accountSelectList = customer.Accounts
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(), // assuming Id is the PK and is int/long
                    Text = $"Account No: {a.Number} - {a.AccountType} - Balance: {a.Balance:C}"
                })
                .ToList();

            var customercertificatevm = new CustomerCertificateVM()
            {
                CustomerId = customer.Id,
                Accounts = accountSelectList,
                Certificates = allcertificates
            };

            return View(customercertificatevm);
        }

        //Generate unique CERT num
        private async Task<string> UniqueCertNumbAsync(long userId)
        {
            string certnum;
            bool exists;
            do
            {
                // Generate the certificate number using the user ID
                certnum = CertificateGenerator.Create(userId);

                // Check if the generated certificate number already exists in the database
                exists = await _UnitOfWork.Repository<Certificate>().ExistsAsync(c => c.CertificateNumber == certnum);
            }
            while (exists);  // Repeat until a unique number is found

            return certnum;
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCertificate(CustomerCertificateVM model)
        {
            // Access the values submitted by the form
            var selectedCertificateId = model.SelectedCertificateId;
            var amount = model.Amount;
            var selectedAccountId = model.SelectedAccountId;

            // Perform the logic (e.g., validate, process certificate application)
            var certificate = _UnitOfWork.Repository<GeneralCertificate>().Get(selectedCertificateId);
            var account = _UnitOfWork.Repository<Account>().Get(selectedAccountId);

            if (certificate == null || account == null)
            {
                // Handle error if certificate or account is not found
                ModelState.AddModelError("", "Invalid certificate or account.");
                return View(model);
            }

            // Check if account has sufficient balance
            if (account.Balance < amount)
            {
                ModelState.AddModelError("", "Insufficient balance in the selected account.");
                return RedirectToAction("ApplyCertificate", new { id = model.CustomerId });
            }

            var CustomerCertificateId = await UniqueCertNumbAsync(account.Number);

            // Create a new Certificate object
            var newCertificate = new Certificate
            {
                CertificateNumber = CustomerCertificateId,
                IssueDate = DateTime.Now, // Add IssueDate
                ExpiryDate = DateTime.Now.AddMonths(certificate.Duration), // Add ExpiryDate
                AccountId = selectedAccountId,
                GeneralCertificate = certificate,
                Amount = amount
            };

            // Update the account balance
            account.Balance -= amount;
            _UnitOfWork.Repository<Certificate>().Add(newCertificate);
            _UnitOfWork.Repository<Account>().Update(account);
            _UnitOfWork.Complete();

            return RedirectToAction("ThanksCertificate", new { number = newCertificate.CertificateNumber });
        }



        public IActionResult ThanksCertificate(string number)
        {
            var certificate = _UnitOfWork.Repository<Certificate>().GetSingleIncluding(c => c.CertificateNumber == number, c => c.GeneralCertificate, c => c.Account);

            if (certificate == null)
            {
                return NotFound($"No certificate found with number: {number}");
            }

            var customerCertificateVM = new CustomerCertificateVM
            {
                CustomerId = certificate.Account.CustomerId,
                CustomerCertificateNumber = certificate.CertificateNumber,
                Amount = certificate.Amount,
                IssueDate = certificate.IssueDate,
                ExpiryDate = certificate.ExpiryDate,
                Name = certificate.GeneralCertificate.Name,
                Duration = certificate.GeneralCertificate.Duration,
                InterestRate = certificate.GeneralCertificate.InterestRate

            };

            return View(customerCertificateVM);
        }

    }



}



