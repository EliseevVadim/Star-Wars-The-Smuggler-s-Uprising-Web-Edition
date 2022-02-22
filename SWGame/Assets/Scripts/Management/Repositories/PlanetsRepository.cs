using SWGame.Entities;
using SWGame.GlobalConfigurations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SWGame.Management.Repositories
{
    public class PlanetsRepository
    {
        private ClientManager _clientManager;
        private List<Planet> _planets;

        public PlanetsRepository(ClientManager clientManager)
        {
            _clientManager = clientManager;
            _planets = new List<Planet>();
        }

        public async Task LoadPlanets()
        {
            await _clientManager.LoadAllPlanets();
        }

        public List<Planet> Planets { get => _planets; set => _planets = value; }
    }
}
