using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryBeginners.Models;
using InventoryBeginners.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace InventoryBeginners.Controllers
{
    public class EmployeeController : Controller
    { private readonly InventoryContext _context;
        private readonly IWebHostEnvironment _webHost;

        public EmployeeController(InventoryContext context, IWebHostEnvironment webHost)
            
        {
            _context = context;
            _webHost = webHost;
        }
        public IActionResult Index(int pg =1 )
        {
            const int pageSize = 10;
            if (pg < 1)
                pg = 1;
            int emcount = _context.Employees.Count();
            var pager = new Pager(emcount, pg, pageSize);
            int emSkip = (pg - 1) * pageSize;
            List<Employee> employees = _context.Employees.Skip(emSkip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(employees);
        }

        [Authorize(Roles = "admin,employe")]
        public IActionResult Details(int Id)
        {
            Employee employee = _context.Employees.Where(p => p.Id == Id).FirstOrDefault();
            return View(employee);
        }
        [HttpGet]

        [Authorize(Roles = "admin,employe")]

        public IActionResult Edit(int Id)
        {
            Employee employee = _context.Employees.Where(p => p.Id == Id).FirstOrDefault();
            return View(employee);
        }
        [HttpPost]
        [Authorize(Roles = "admin,employe")]

        public IActionResult Edit(Employee employee)
        {
            _context.Attach(employee);
            _context.Entry(employee).State = EntityState.Modified;
            _context.SaveChanges();
            return RedirectToAction("index");
            
        }
        [Authorize(Roles = "admin")]

        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Employee employee = _context.Employees.Where(p => p.Id == Id).FirstOrDefault();
            return View(employee);
        }
        [HttpPost]
        [Authorize(Roles = "admin")]

        public IActionResult Delete(Employee employee)
        {
            _context.Attach(employee);
            _context.Entry(employee).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("index");

        }
        private async Task<string> UploadImage(string folderPath, IFormFile file)
        {

            folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHost.WebRootPath, folderPath);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folderPath;
        }
        [Authorize(Roles = "admin")]

        [HttpGet]
        public IActionResult Create(int Id)
        {
            Employee employee = _context.Employees.Where(p => p.Id == Id).FirstOrDefault();
            return View(employee);
        }
        [Authorize(Roles = "admin")]

        [HttpPost]
        public async Task<IActionResult> Create(Employee employee)
        {
            /* var emp_id = _context.Employees.Max(empid => empid.Id);


                 emp_id++;


             employee.Id = emp_id;*/

          
                string folder = "employe/cover/";
             employee.CoverImageUrl = await UploadImage(folder, employee.CoverPhoto);
            
            _context.Attach(employee);
            _context.Entry(employee).State = EntityState.Added;
            _context.SaveChanges();
            return RedirectToAction("index");

        }
    }
}
