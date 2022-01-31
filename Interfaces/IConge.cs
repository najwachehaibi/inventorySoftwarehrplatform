using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryBeginners.Models;
using CodesByAniz.Tools;

namespace InventoryBeginners.Interfaces
{
    public interface IConge
    {
        PaginatedList<Conge> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Conge GetUnit(int id); // read particular item

        Conge Create(Conge unit);

        Conge Edit(Conge unit);

        Conge Delete(Conge unit);

        public bool IsUnitNameExists(string name);
        public bool IsUnitNameExists(string name, int Id);


    }
}

