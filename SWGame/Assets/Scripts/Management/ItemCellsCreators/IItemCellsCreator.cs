using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGame.Management.ItemCellsCreators
{
    public interface IItemCellsCreator<T> where T : class
    {
        public List<T> ProcessCellsData(List<Dictionary<string, object>> cellsData);
    }
}
