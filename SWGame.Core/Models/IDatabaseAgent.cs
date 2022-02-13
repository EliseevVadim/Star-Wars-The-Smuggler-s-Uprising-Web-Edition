using System.Threading.Tasks;

namespace SWGame.Core.Models
{
    public interface IDatabaseAgent
    {
        public Task AddToDatabase();
    }
}
