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
    public class CustomerLoansController : Controller
    {
        private readonly IUnitOfWork _UnitOfWork;
        private readonly IMapper _mapper;
        private readonly FinancialDocumentService _financialDocumentService;


        public CustomerLoansController(IUnitOfWork UnitOfWork, IMapper mapper, FinancialDocumentService financialDocumentService)
        {
            _UnitOfWork = UnitOfWork;
            _mapper = mapper;
            _financialDocumentService = financialDocumentService;
        }
         public IActionResult Details(string id)
        {
            var customer = _UnitOfWork.Repository<MyCustomer>()
                                  .GetSingleIncluding(c => c.Id == id, c => c.Loans);

            if (customer != null)
            {
                if (customer.Loans?.Any() ?? false)
                {
                    var LoanModel = _mapper.Map<List<CustomerLoansViewModel>>(customer.Loans);
                    return View(LoanModel);
                }
                else
                {
                    return RedirectToAction("Details", "CustomerProfile", new { id = customer?.Id });
                }
            }
            else
            {
                return NotFound($"No Customer Exist for id : {id}");
            }

        }

        [HttpGet]
        public IActionResult ApplyLoan(string id)
        {
            var customer = _UnitOfWork.Repository<MyCustomer>()
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
                    FinancialDocuments = new List<FinancialDocumentVM>
                    {
                        new FinancialDocumentVM(), // For Income Statement
                        new FinancialDocumentVM(), // For Asset Declaration
                        new FinancialDocumentVM()  // For Tax Return
                    }
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
            if (!ModelState.IsValid)
            {
                var customer = _UnitOfWork.Repository<MyCustomer>()
                    .GetSingleIncluding(c => c.Id == model.CustomerId, c => c.Accounts);

                model.Accounts = customer.Accounts
                    .Select(a => new SelectListItem
                    {
                        Value = a.Id.ToString(),
                        Text = $"Account No: {a.Number} - {a.AccountType} - Balance: {a.Balance:C}"
                    })
                    .ToList();

                return View(model);
            }

            var loan = new Loan
            {
                AccountId = model.SelectedAccountId.Value,
                CustomerId = model.CustomerId,
                BranchId = model.BranchId.GetValueOrDefault(),
                StartDate = model.StartDate,
                LoanAmount = model.LoanAmount.GetValueOrDefault(),
                CurrentDebt = model.CurrentDebt,
                InterestRate = model.InterestRate,
                DurationInMonth = model.DurationInMonth,
                LoanStatus = model.LoanStatus,
                LoanType = model.LoanType
            };

            _UnitOfWork.Repository<Loan>().Add(loan);

            // Process each financial document
            foreach (var document in model.FinancialDocuments)
            {
                if (document.DocumentFile != null && document.DocumentFile.Length > 0)
                {
                    var result = await _financialDocumentService.UploadFinancialDocument(
                        model.CustomerId,
                        document.DocumentFile,
                        document.DocumentType, // You can specify the document type here
                        document.Description,
                        document.IssueDate
                    );

                    if (!int.TryParse(result, out _))
                    {
                        ModelState.AddModelError("", $"Failed to upload document: {result}");
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
            var customer = _UnitOfWork.Repository<MyCustomer>().GetSingleIncluding(c => c.Id == id, c => c.Loans);

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
                    LoanStatus = loan.LoanStatus,
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
