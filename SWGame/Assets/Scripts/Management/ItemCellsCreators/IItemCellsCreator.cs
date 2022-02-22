using System.Collections.Generic;

namespace SWGame.Management.ItemCellsCreators
{
    public interface IItemCellsCreator<T> where T : class
    {
        public List<T> ProcessCellsData(List<Dictionary<string, object>> cellsData);
    }
}
