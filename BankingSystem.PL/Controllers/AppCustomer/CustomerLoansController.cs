using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoMapper;
using BankingSystem.BLL.Services;
using BankingSystem.BLL;

namespace BankingSystem.PL.Controllers.AppCustomer
{
    public class CustomerLoansController(IUnitOfWork UnitOfWork, IMapper mapper, FinancialDocumentService financialDocumentService) : Controller
    {
        private readonly IUnitOfWork _UnitOfWork = UnitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly FinancialDocumentService _financialDocumentService = financialDocumentService;

        public IActionResult Details(string id)
        {
            var customer = _UnitOfWork.Repository<Customer>()
                                  .GetSingleIncluding(c => c.Id == id, c => c.Loans);

            if (customer != null)
            {
                var LoanModel = _mapper.Map<List<CustomerLoansViewModel>>(customer.Loans ?? new List<Loan>());
                ViewBag.id = customer.Id;
                return View(LoanModel);
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }

        [HttpGet]
        public IActionResult ApplyLoan(string id)
        {
            var customer = _UnitOfWork.Repository<Customer>()
                            .GetSingleIncluding(c => c.Id == id, c => c.Accounts);
            if (customer != null)
            {
                var accountSelectList = customer.Accounts
               .Select(a => new SelectListItem
               {
                   Value = a.Id.ToString(), // assuming Id is the PK and is int/long
                   Text = $"Account No: {a.Number} - {a.AccountType} - Balance: {a.Balance:C}"
               })
               .ToList();

                var customerloanvm = new CustomerLoanVM()
                {
                    CustomerId = customer.Id,
                    Accounts = accountSelectList,
                    BranchId = customer.BranchId,
                    FinancialDocuments =
                    [
                        new (), // For Income Statement
                        new (), // For Asset Declaration
                        new ()  // For Tax Return
                    ]
                };
                return View(customerloanvm);

            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyLoan(CustomerLoanVM model)
        {
            var customer = _UnitOfWork.Repository<Customer>()
                .GetSingleIncluding(c => c.Id == model.CustomerId, c => c.Accounts);

            // Prepare account select list
            var accountSelectList = customer.Accounts
                .Select(a => new SelectListItem
                {
                    Value = a.Id.ToString(),
                    Text = $"Account No: {a.Number} - {a.AccountType} - Balance: {a.Balance:C}"
                })
                .ToList();

            ViewBag.AccountSelectList = accountSelectList;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loan = new Loan
            {
                AccountId = model.SelectedAccountId.Value,
                CustomerId = model.CustomerId,
                BranchId = model.BranchId.GetValueOrDefault(),
                StartDate = model.StartDate,
                LoanAmount = model.LoanAmount.GetValueOrDefault(),
                CurrentDebt = model.LoanAmount.GetValueOrDefault(),
                InterestRate = model.InterestRate,
                DurationInMonth = model.DurationInMonth,
                LoanStatus = model.LoanStatus,
                LoanType = model.LoanType
            };

            _UnitOfWork.Repository<Loan>().Add(loan);
            _UnitOfWork.Complete();

            // Process each financial document
            foreach (var document in model.FinancialDocuments)
            {
                if (document.DocumentFile != null && document.DocumentFile.Length > 0)
                {
                    var result = await _financialDocumentService.UploadFinancialDocument(
                        model.CustomerId,
                        document.DocumentFile,
                        document.DocumentType,
                        document.Description,
                        document.IssueDate,
                        loan.Id
                    );

                    if (!int.TryParse(result, out _))
                    {
                        ModelState.AddModelError("", $"Failed to upload document: {result}");

                        ViewBag.AccountSelectList = accountSelectList;
                        return View(model);
                    }
                }
            }

            _UnitOfWork.Complete();

            return RedirectToAction("ThanksLoan", new { id = model.CustomerId });
        }


        public IActionResult ThanksLoan(string id)
        {
            // Retrieve the customer and their loan information
            var customer = _UnitOfWork.Repository<Customer>().GetSingleIncluding(c => c.Id == id, c => c.Loans);

            if (customer == null)
            {
                return NotFound($"No customer found with id: {id}");
            }

            var customerLoanVM = customer.Loans
                .Select(loan => new CustomerLoanVM
                {
                    CustomerId = customer.Id,
                    LoanAmount = loan.LoanAmount,
                    CurrentDebt = loan.CurrentDebt,
                    InterestRate = loan.InterestRate,
                    DurationInMonth = loan.DurationInMonth,
                    LoanStatus = LoanStatus.Pending,
                    LoanType = loan.LoanType,
                    StartDate = loan.StartDate,
                    loanID = loan.Id,
                }).FirstOrDefault();

            // Ensure the loan was found for the customer
            if (customerLoanVM == null)
            {
                return NotFound($"No loan found for customer with id: {id}");
            }

            // Return the view with the customer loan details
            return View(customerLoanVM);
        }
        [HttpGet]
        public IActionResult ViewDocument(int id)
        {
            var document = _financialDocumentService.GetFinancialDocument(id);
            if (document == null)
            {
                return NotFound("Document not found.");
            }

            return View(document);
        }

        [HttpGet]
        public IActionResult DownloadDocument(int id)
        {
            var document = _financialDocumentService.GetFinancialDocument(id);
            if (document == null || document.FileData == null)
            {
                return NotFound("Document not found or file data is missing.");
            }

            return File(document.FileData, "application/pdf", document.FileName);
        }

    }
}
