using SWGame.Entities;
using SWGame.GlobalConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGame.Management.Repositories
{
    public class LocationsRepository
    {
        private ClientManager _clientManager;
        private List<Location> _locations;

        public LocationsRepository(ClientManager clientManager)
        {
            _clientManager = clientManager;
            _locations = new List<Location>();
        }

        public List<Location> Locations { get => _locations; set => _locations = value; }

        public async Task LoadAllLocations()
        {
            await _clientManager.LoadAllLocations();
        }
    }
}
