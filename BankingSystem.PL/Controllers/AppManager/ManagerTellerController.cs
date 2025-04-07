using AutoMapper;
using Azure.Core;
using BankingSystem.BLL.Interfaces;
using BankingSystem.DAL.Models;
using BankingSystem.PL.ViewModels.Manager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace BankingSystem.PL.Controllers.Manager
{
    [Authorize(Roles = "Manager")]
    public class ManagerTellerController(IUnitOfWork unitOfWork, IMapper mapper) : Controller
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public ActionResult GetAllTellers(string id)
        {
            var manager = _unitOfWork.Repository<MyManager>().GetSingleIncluding(b => b.Id == id);
            if (manager == null)
            {
                return NotFound($"Manager with ID {id} not found.");
            }

            var branchId = manager.BranchId;
            if (branchId == null)
            {
                return NotFound($"Branch with ID {branchId} not found.");
            }

            var employees = _unitOfWork.Repository<Teller>()
                .GetAllIncluding(e => e.Branch, e => e.Department)
                .Where(e => e.BranchId == branchId)
                .ToList();

            if (employees == null || employees.Count == 0)
            {
                return NotFound("No employees found.");
            }

            var tellerViewModels = _mapper.Map<List<TellerDetailsViewModel>>(employees);
            return View(tellerViewModels);
        }
        [HttpGet]
        public ActionResult GetTellerDetails(string id)
        {
            var employee = _unitOfWork.Repository<Teller>()
                .GetSingleIncluding(e => e.Id == id, e => e.Branch, e => e.Department);

            if (employee == null)
            {
                return NotFound();
            }

            var tellerDetailsViewModel = _mapper.Map<TellerDetailsViewModel>(employee);

            tellerDetailsViewModel.BranchName = employee.Branch?.Name;
            tellerDetailsViewModel.DepartmentName = employee.Department?.Name ?? "No Departments";

            return View(tellerDetailsViewModel);
        }
        [HttpGet]
        public ActionResult EditTeller(string id)
        {
            var teller = _unitOfWork.Repository<Teller>()
                .GetSingleIncluding(e => e.Id == id, e => e.Branch, e => e.Branch.MyManager, e => e.Department);
            if (teller == null)
            {
                return NotFound();
            }

            var tellerDetailsViewModel = _mapper.Map<TellerDetailsViewModel>(teller);
            tellerDetailsViewModel.BranchName = teller.Branch?.Name;
            tellerDetailsViewModel.DepartmentName = teller.Department?.Name;

            if (teller.Branch?.MyManager == null)
            {
                return NotFound("The branch does not have a manager assigned.");
            }

            return View(tellerDetailsViewModel);
        }
        [HttpPost]
        public ActionResult EditTeller(TellerDetailsViewModel tellerDetailsViewModel)
        {
            if (ModelState.IsValid)
            {
                var teller = _unitOfWork.Repository<Teller>()
                    .GetSingleIncluding(e => e.Id == tellerDetailsViewModel.Id, e => e.Branch, e => e.Department, e => e.Branch.MyManager);
                if (teller == null)
                {
                    return NotFound();
                }

                _mapper.Map(tellerDetailsViewModel, teller);
                _unitOfWork.Complete();
                return RedirectToAction("GetAllTellers", new { id = teller.Branch.MyManager.Id });
            }
            return View(tellerDetailsViewModel);
        }
        [HttpGet]
        public ActionResult DeleteTeller(string id)
        {
            var teller = _unitOfWork.Repository<Teller>()
                .GetSingleIncluding(e => e.Id == id, e => e.Branch, e => e.Branch.MyManager, e => e.Department);
            if (teller == null)
            {
                return NotFound();
            }
            var tellerDetailsViewModel = _mapper.Map<TellerDetailsViewModel>(teller);
            tellerDetailsViewModel.BranchName = teller.Branch?.Name;
            tellerDetailsViewModel.DepartmentName = teller.Department?.Name;
            return View(tellerDetailsViewModel);
        }
        [HttpPost]
        public ActionResult DeleteTeller(TellerDetailsViewModel tellerDetails)
        {
            var teller = _unitOfWork.Repository<Teller>()
                .GetSingleIncluding(e => e.Id == tellerDetails.Id, e => e.Branch, e => e.Department, e => e.Branch.MyManager);
            if (teller == null)
            {
                return NotFound();
            }
            _unitOfWork.Repository<Teller>().Delete(teller);
            _unitOfWork.Complete();
            return RedirectToAction("GetAllTellers", new { id = teller.Branch.MyManager.Id});
        }
    }
}
