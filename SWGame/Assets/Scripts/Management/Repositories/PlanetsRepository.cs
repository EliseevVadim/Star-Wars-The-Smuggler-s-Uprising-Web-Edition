using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SWGame.GlobalConfigurations;
using SWGame.Entities;

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
