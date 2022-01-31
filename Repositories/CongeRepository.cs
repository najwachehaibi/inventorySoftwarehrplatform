using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryBeginners.Data;
using InventoryBeginners.Interfaces;
using InventoryBeginners.Models;
using Microsoft.EntityFrameworkCore;
using CodesByAniz.Tools;

namespace InventoryBeginners.Repositories
{
    public class CongeRepository : IConge
    {
        private readonly InventoryContext _context; // for connecting to efcore.
        public CongeRepository(InventoryContext context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public Conge Create(Conge unit)
        {
            _context.Conges.Add(unit);
            _context.SaveChanges();
            return unit;
        }

        public Conge Delete(Conge unit)
        {
            _context.Conges.Attach(unit);
            _context.Entry(unit).State = EntityState.Deleted;
            _context.SaveChanges();
            return unit;
        }

        public Conge Edit(Conge unit)
        {
            _context.Conges.Attach(unit);
            _context.Entry(unit).State = EntityState.Modified;
            _context.SaveChanges();
            return unit;
        }


        private List<Conge> DoSort(List<Conge> units, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "name")
            {
                if (sortOrder == SortOrder.Ascending)
                    units = units.OrderBy(n => n.StartDay).ToList();
                else
                    units = units.OrderByDescending(n => n.StartDay).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    units = units.OrderBy(d => d.EndDay).ToList();
                else
                    units = units.OrderByDescending(d => d.EndDay).ToList();
            }

            return units;
        }

        public PaginatedList<Conge> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<Conge> units;

            if (SearchText != "" && SearchText != null)
            {
                units = _context.Conges.Where(n => n.type.Contains(SearchText) || n.status.Contains(SearchText))
                    .ToList();
            }
            else
                units = _context.Conges.ToList();

            units = DoSort(units, SortProperty, sortOrder);

            PaginatedList<Conge> retUnits = new PaginatedList<Conge>(units, pageIndex, pageSize);

            return retUnits;
        }

        public Conge GetUnit(int id)
        {
            Conge unit = _context.Conges.Where(u => u.CongeId == id).FirstOrDefault();
            return unit;
        }
        public bool IsUnitNameExists(string name)
        {
            int ct = _context.Conges.Where(n => n.type.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsUnitNameExists(string name, int Id)
        {
            int ct = _context.Conges.Where(n => n.type.ToLower() == name.ToLower() && n.CongeId != Id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

    }
}
