using AutoMapper;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Teller;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
using System.Security.Claims;

namespace BankingSystem.PL.Controllers.AppTeller
{
    [Authorize(Roles ="Teller")]
    public class HandleCardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;


        //
        private readonly ISearchPaginationRepo<VisaCard> _searchPaginationRepo;

        public HandleCardController(IUnitOfWork unitOfWork ,IMapper mapper, ISearchPaginationRepo<VisaCard> searchPaginationRepo)
        {
            _unitOfWork = unitOfWork;
           _mapper = mapper;

            //
            _searchPaginationRepo = searchPaginationRepo;
        }
        // GET: HandleCardController
        public ActionResult GetAllCards(string? filter)
        {

            var AllCards = _unitOfWork.Repository<VisaCard>().GetAllIncluding(C => C.Account,C=>C.Account.Customer);

            if (filter != null)
                AllCards = AllCards.Where(c => c.CardType.ToString() == filter).ToList();

            var cardsToReturnViewModel = _mapper.Map<List<CardsViewModel>>(AllCards);

            return View(cardsToReturnViewModel);
        }

        // GET: HandleCardController/Details/5
        //public ActionResult Details(int id)
        //{
        //    var card = _unitOfWork.Repository<VisaCard>().GetAllIncluding(id);
        //    return View();
        //}

        // GET: HandleCardController/Create
        public ActionResult Create()
        {
            var accountsWithoutCards = _unitOfWork.Repository<Account>()
                     .GetAllIncluding(a => a.Customer, a => a.Card)
                     .Where(a => a.Card == null &&
                                 a.Customer.Accounts.Count <= 2 )
                     .ToList();

            var accountsVM = accountsWithoutCards.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.Customer.UserName} - {a.Number}"
            }).ToList();

            ViewBag.Accounts = accountsVM;

            return View();
        }


        [HttpPost]
        public IActionResult Create(CreateCardViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                PopulateAccountsDropdown();

            }

            var account = _unitOfWork.Repository<Account>()
                .GetAllIncluding(a => a.Card)
                .FirstOrDefault(a => a.Id == vm.AccountId);
            account.Balance -= 70;

            if (account == null || account.Card != null)
            {
                ModelState.AddModelError("", "Invalid account or card already exists.");
                PopulateAccountsDropdown();
                return View(vm);
            }

            // Generate the Card Number (16 digits)
            var random = new Random();
            string generatedCardNumber = string.Concat(Enumerable.Range(0, 16)
                .Select(_ => random.Next(0, 10).ToString()));

            string generatedCVV = string.Concat(Enumerable.Range(0, 3)
              .Select(_ => random.Next(0, 10).ToString()));


            var creationDate = DateTime.Now;
            var expDate = creationDate.AddYears(7);

            var newCard = new VisaCard
            {
                Number =  generatedCardNumber,
                CVV = generatedCVV, 
                ExpDate = expDate,
                CreationDate = creationDate,
                CardType = vm.CardType,
                AccountId = vm.AccountId
            };

            _unitOfWork.Repository<VisaCard>().Add(newCard);
            _unitOfWork.Complete();

            //return RedirectToAction(nameof(GetAllCards));
            return RedirectToAction("Details", "HandleAccount", new {id = newCard.AccountId});
        }


        //[HttpPost]
        //public IActionResult Create(CreateCardViewModel vm)
        //{

        //    return RedirectToAction("Details", "HandleAccount", new { id = 0 });
        //}


        private void PopulateAccountsDropdown()
        {
            var accountsWithoutCards = _unitOfWork.Repository<Account>()
                .GetAllIncluding(a => a.Customer, a => a.Card)
                .Where(a => a.Card == null && a.Customer.Accounts.Count <= 2)
                .ToList();

            var accountsVM = accountsWithoutCards.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = $"{a.Customer.UserName} - {a.Number}"
            }).ToList();

            ViewBag.Accounts = accountsVM;
        }



        // POST: HandleCardController/Create
        //[HttpPost]
      
    

        // GET: HandleCardController/Edit/5
        // We did not Make Edit Function

   
        // POST: HandleCardController/Delete/5
        [HttpPost]
        public IActionResult Delete(int id, string? returnUrl = null)
        {
            var card = _unitOfWork.Repository<VisaCard>().Get(id);
            if (card != null)
            {
                card.AccountNumber = _unitOfWork.Repository<Account>().Get((int)card.AccountId).Number;
                card.AccountId = null;
                
                    
                _unitOfWork.Repository<VisaCard>().Delete(card);
                _unitOfWork.Complete(); // Save changes to the database

                //return Json(new { success = true });

                if (returnUrl != null)
                    return Redirect(returnUrl);

                return RedirectToAction(nameof(GetAllCards));
            }
            //return Json(new { success = false, message = "Card not found" });
            return NotFound();
        }




        [HttpGet]
        public IActionResult Search(string search)
        {
            var results = _searchPaginationRepo.Search(search);

            var cardsToReturnViewModel = _mapper.Map<List<CardsViewModel>>(results);
            ViewBag.Search = search;

            return View("GetAllCards", cardsToReturnViewModel);
        }
    }
}
