using System.Linq;
using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Customer;
using BankingSystem.PL.ViewModels.Manager;
using BankingSystem.PL.ViewModels.Teller;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSystem.PL.Controllers.AppManager
{
    public class ManagerCustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ManagerCustomerController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult GetAllCustomers(string id)
        {
            var manager = _unitOfWork.Repository<DAL.Models.Manager>().GetSingleIncluding(x => x.Id == id);
            var branchID = manager?.BranchId;

            if (branchID == null)
                return NotFound("Manager or Branch not found");

            var customers = _unitOfWork.Repository<Customer>()
                .GetAllIncluding(c => c.Branch)
                .Where(c => c.BranchId == branchID)
                .ToList();

            if (customers.Count == 0)
                return NotFound("No customers found for this branch");

            var customerView = _mapper.Map<List<Customer>, List<CustomerDetailsViewModel>>(customers);
            return View(customerView);
        }
        [HttpGet]
        public ActionResult GetCustomerDetails(string id)
        {
            var customer = _unitOfWork.Repository<Customer>()
                .GetAllIncluding(
                    c => c.Transactions,
                    c => c.Loans,
                    c => c.SupportTickets,
                    c => c.Accounts,
                    c => c.Branch
                )
                .FirstOrDefault(c => c.Id == id);

            if (customer == null)
                return NotFound("Customer not found");

            var accountsWithCards = _unitOfWork.Repository<Account>()
                .GetAllIncluding(a => a.Certificates, a => a.Card)
                .Where(a => a.CustomerId == id && a.Card != null && a.Id == a.Card.AccountId)
                .ToList();

            customer.Accounts = accountsWithCards;

            var customerViewModel = _mapper.Map<ManagerCustomerDetailsViewModel>(customer);

            ViewBag.CustomerId = id;
            return View(customerViewModel);
        }
        [HttpGet]
        public ActionResult GetCustomerLoan(string id)
        {
            var loanWithPayment = _unitOfWork.Repository<Loan>()
                .GetAllIncluding(l => l.Payments, l => l.Account).Where(l => l.CustomerId == id).ToList();

            if (loanWithPayment == null)
                return NotFound("No loan found for this customer.");

            var loans = _mapper.Map<List<LoanViewModel>>(loanWithPayment);

            ViewBag.CustomerId = id;
            return View(loans);
        }
        [HttpGet]
        public ActionResult GetCustomerAccount(string id)
        {
            var customerAccount = _unitOfWork.Repository<Account>().GetAll().Where(l => l.CustomerId == id).ToList();
            if (customerAccount == null || customerAccount.Count == 0)
                return NotFound("No accounts found for this customer.");

            var account = _mapper.Map<List<CustomerAccountViewModel>>(customerAccount);
            ViewBag.CustomerId = id;
            return View(account);
        }
        [HttpGet]
        public ActionResult GetCustomerCard(string id)
        {
            var customerCards = _unitOfWork.Repository<VisaCard>()
                .GetAllIncluding(c => c.Account)
                .Where(c => c.Account != null &&
                            c.Account.CustomerId == id &&
                            c.Account.Id == c.AccountId)
                .ToList();

            if (customerCards == null || customerCards.Count == 0)
                return NotFound("No cards found for this customer.");

            var cards = _mapper.Map<List<CustomerCardViewModel>>(customerCards);

            ViewBag.CustomerId = id;
            return View(cards);
        }
        [HttpGet]
        public ActionResult GetCustomerSupportTicket(string id)
        {
            var supportsTickets = _unitOfWork.Repository<SupportTicket>().GetAllIncluding(s => s.Customer.Accounts).Where(s => s.CustomerId == id).ToList();
            if (supportsTickets == null || supportsTickets.Count == 0)
                return NotFound("No support tickets found for this customer.");

            var supportTicketViewModels = _mapper.Map<List<CustomerSupportTicketViewModel>>(supportsTickets);
            ViewBag.CustomerId = id;
            return View(supportTicketViewModels);
        }
        [HttpGet]
        public ActionResult GetCustomerTransaction(string id)
        {
            var transactions = _unitOfWork.Repository<Transaction>().GetAllIncluding(s => s.Customer.Accounts).Where(t => t.CustomerID == id).ToList();
            if (transactions == null || transactions.Count == 0)
                return NotFound("No transactions found for this customer.");

            var customerTransactions = _mapper.Map<List<CustomerTransactionViewModel>>(transactions);
            ViewBag.CustomerId = id;
            return View(customerTransactions);
        }
        [HttpGet]
        public ActionResult GetCustomerCertificate(string id)
        {
            var certificates = _unitOfWork.Repository<Certificate>()
                .GetAllIncluding(c => c.Account)
                .Where(t => t.Account.CustomerId == id)
                .ToList();

            if (certificates == null || certificates.Count == 0)
                return NotFound("No certificates found for this customer.");

            var customerCertificate = _mapper.Map<List<CertificateDetail>>(certificates);
            ViewBag.CustomerId = id;
            return View(customerCertificate);
        }
    }
}
