using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InventoryBeginners.Models
{
    public static class Helper
    {
        public static string GetTypeName(string fullTypeName)
        {
            string retstring = "";
            try
            {
                int lastIndex = fullTypeName.LastIndexOf('.') + 1;
                retstring = fullTypeName.Substring(lastIndex, fullTypeName.Length - lastIndex);
            }
            catch
            {
                retstring = fullTypeName;
            }
            retstring = retstring.Replace("]", "");
            return retstring;
        }
    }
}
