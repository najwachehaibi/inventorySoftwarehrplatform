using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventoryBeginners.Data;
using InventoryBeginners.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using InventoryBeginners.Interfaces;
using CodesByAniz.Tools;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace InventoryBeginners.Controllers
{
    
    public class CongeController : Controller
    {

        private readonly IConge _unitRepo;
        private readonly UserManager<IdentityUser> _user;
        public CongeController(IConge unitrepo, UserManager<IdentityUser> user) // here the repository will be passed by the dependency injection.
        {
            _unitRepo = unitrepo;
            _user = user;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("StartDay");
            sortModel.AddColumn("EndDay");
            sortModel.AddColumn("type");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<Conge> units = _unitRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(units);
        }

        [Authorize(Roles = "employe,admin")]

        public IActionResult Create()
        {
            Conge unit = new Conge();
            return View(unit);
        }
        [Authorize(Roles = "employe,admin")]
        [HttpPost]
        public IActionResult Create(Conge unit)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (unit.type == null)
                    errMessage = "Unit Description Must have a type";

              

                if (errMessage == "")
                {
                    /*var employeeId = _user.GetUserId(HttpContext.User);
                    IdentityUser user = _user.FindByIdAsync(employeeId).Result;
                    unit.employeeId = user;*/
                    var userId = User.FindFirstValue(ClaimTypes.Name);
                    unit.employeeId = userId;
                    unit = _unitRepo.Create(unit);
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
            }
            else
            {
                TempData["SuccessMessage"] = "Unit  Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        [Authorize(Roles = "employe,admin")]

        public IActionResult Details(int id) //Read
        {
            Conge unit = _unitRepo.GetUnit(id);
            return View(unit);
        }


        public IActionResult Edit(int id)
        {
            Conge unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }
        [Authorize(Roles = "employe,admin")]

        [HttpPost]
        public IActionResult Edit(Conge unit)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if ( unit.StartDay.ToString()==null)
                    errMessage = "Unit Description Must be atleast 4 Characters";

                

                if (errMessage == "")
                {
                    unit = _unitRepo.Edit(unit);
                    TempData["SuccessMessage"] =  ", Unit Saved Successfully";
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }



            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];


            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }
        [Authorize(Roles = "employe,admin")]

        public IActionResult Delete(int id)
        {
            Conge unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }

        [Authorize(Roles = "employe,admin")]

        [HttpPost]
        public IActionResult Delete(Conge unit)
        {
            try
            {
                unit = _unitRepo.Delete(unit);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(unit);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] =  " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }


    }
}
