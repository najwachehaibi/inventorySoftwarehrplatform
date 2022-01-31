using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryBeginners.Data;
using InventoryBeginners.Interfaces;
using InventoryBeginners.Models;
using Microsoft.EntityFrameworkCore;
using CodesByAniz.Tools;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace InventoryBeginners.Repositories
{
    public class CandidatRepository : ICandidat
    {
        private readonly InventoryContext _context; // for connecting to efcore.

        public CandidatRepository(InventoryContext context) // will be passed by dependency injection.
        {
            _context = context;
           
        }
        public  Candidat Create(Candidat unit)
        {
            /*string uniqueFileName = GetUploadedFileName(unit);
            unit.PhotoUrl = uniqueFileName;*/
          
            _context.Candidats.Add(unit);
            _context.SaveChanges();
            return unit;
        }
     
        /*private string GetUploadedFileName(Candidat unit)
        {
            string uniqueFileName = null;

            if (unit.ProfilePhoto != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath,"images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + unit.ProfilePhoto.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    unit.ProfilePhoto.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }*/

        public Candidat Delete(Candidat unit)
        {
            _context.Candidats.Attach(unit);
            _context.Entry(unit).State = EntityState.Deleted;
            _context.SaveChanges();
            return unit;
        }

        public Candidat Edit(Candidat unit)
        {
            _context.Candidats.Attach(unit);
            _context.Entry(unit).State = EntityState.Modified;
            _context.SaveChanges();
            return unit;
        }


        private List<Candidat> DoSort(List<Candidat> units, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    units = units.OrderBy(n => n.FirstName).ToList();
                else
                    units = units.OrderByDescending(n => n.FirstName).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    units = units.OrderBy(d => d.LastName).ToList();
                else
                    units = units.OrderByDescending(d => d.LastName).ToList();
            }

            return units;
        }

        public PaginatedList<Candidat> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Candidat> units;

            if (SearchText != "" && SearchText != null)
            {
                units = _context.Candidats.Where(n => n.FirstName.Contains(SearchText) || n.LastName.Contains(SearchText))
                    .ToList();
            }
            else
                units = _context.Candidats.ToList();

            units = DoSort(units, SortProperty, sortOrder);

            PaginatedList<Candidat> retUnits = new PaginatedList<Candidat>(units, pageIndex, pageSize);

            return retUnits;
        }

        public Candidat GetUnit(int id)
        {
            Candidat unit = _context.Candidats.Where(u => u.CandidatId == id).FirstOrDefault();
            return unit;
        }
        public bool IsUnitNameExists(string name)
        {
            int ct = _context.Candidats.Where(n => n.FirstName.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsUnitNameExists(string name, int Id)
        {
            int ct = _context.Candidats.Where(n => n.FirstName.ToLower() == name.ToLower() && n.CandidatId != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public Candidat CreateAsync(Candidat unit)
        {
            throw new NotImplementedException();
        }
    }
}
