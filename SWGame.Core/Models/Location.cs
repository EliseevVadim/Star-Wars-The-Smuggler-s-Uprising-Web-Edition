namespace SWGame.Core.Models
{
    public class Location
    {
        private int _id;
        private string _name;
        private int _planetId;

        public int Id { get => _id; set => _id = value; }
        public string Name { get => _name; set => _name = value; }
        public int PlanetId { get => _planetId; set => _planetId = value; }
    }
}
