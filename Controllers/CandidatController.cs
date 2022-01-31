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
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;


namespace InventoryBeginners.Controllers
{
    
    public class CandidatController : Controller
    {

        private readonly ICandidat _unitRepo;
        private readonly IWebHostEnvironment _webHost;

        public CandidatController(ICandidat unitrepo, IWebHostEnvironment webHost)// here the repository will be passed by the dependency injection.
        {
            _unitRepo = unitrepo;
            _webHost = webHost;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("FirstName"); 
            sortModel.AddColumn("LastName");
            sortModel.AddColumn("DateOfBirth");
            sortModel.AddColumn("Email");
            sortModel.AddColumn("Phone");
            sortModel.AddColumn("Level");
            sortModel.AddColumn("DimplomaTitle");
            sortModel.AddColumn("YearsExperience");
            sortModel.AddColumn("Experience");


            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<Candidat> units = _unitRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(units.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(units);
        }


        public IActionResult Create()
        {
            Candidat unit = new Candidat();
            return View(unit);
        }

        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHost.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
        [HttpPost]
        public async Task<IActionResult> Create(Candidat unit)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (unit.FirstName.Length < 4 || unit.FirstName == null)
                    errMessage = "Unit Description Must be atleast 4 Characters";

                if (_unitRepo.IsUnitNameExists(unit.FirstName) == true)
                    errMessage = errMessage + " " + " Unit Name " + unit.FirstName + " Exists Already";
                if (unit.CoverPhoto != null)
                {
                    string folder = "candidat/cover/";
                    unit.CoverImageUrl = await UploadImage(folder, unit.CoverPhoto);
                }
                if (unit.BookPdf != null)
                {
                    string folder = "candidat/pdf/";
                    unit.BookPdfUrl = await UploadImage(folder, unit.BookPdf);
                }
                if (errMessage == "")
                {
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
                TempData["SuccessMessage"] = "Unit " + unit.FirstName + " Created Successfully";
                return RedirectToAction(nameof(Index));
            }
        }
        //[Authorize(Roles = "Admin")]
        // [Authorize(Policy = "Admin")]
        [Authorize(Roles = "admin")]
        public IActionResult Details(int id) //Read
        {
            Candidat unit = _unitRepo.GetUnit(id);
            return View(unit);
        }


        public IActionResult Edit(int id)
        {
            Candidat unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }

        [HttpPost]
        public IActionResult Edit(Candidat unit)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (unit.FirstName.Length < 4 || unit.FirstName == null)
                    errMessage = "Unit Description Must be atleast 4 Characters";

                if (_unitRepo.IsUnitNameExists(unit.FirstName, unit.CandidatId) == true)
                    errMessage = errMessage + "Unit Name " + unit.FirstName + " Already Exists";

                if (errMessage == "")
                {
                    unit = _unitRepo.Edit(unit);
                    TempData["SuccessMessage"] = unit.FirstName + ", Unit Saved Successfully";
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
        [Authorize(Roles = "admin")]
        public IActionResult Delete(int id)
        {
            Candidat unit = _unitRepo.GetUnit(id);
            TempData.Keep();
            return View(unit);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult Delete(Candidat unit)
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

            TempData["SuccessMessage"] = "Unit " + unit.FirstName + " Deleted Successfully";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }


    }
}
