using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InventoryBeginners.Models;
using CodesByAniz.Tools;

namespace InventoryBeginners.Interfaces
{
    public interface ICandidat
    {
        PaginatedList<Candidat> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5); //read all
        Candidat GetUnit(int id); // read particular item

        Candidat Create(Candidat unit);

        Candidat Edit(Candidat unit);

        Candidat Delete(Candidat unit);

        public bool IsUnitNameExists(string name);
        public bool IsUnitNameExists(string name, int Id);


    }
}

