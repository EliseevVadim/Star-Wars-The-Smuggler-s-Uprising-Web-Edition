namespace SWGame.Core.Models
{
    public class PlayerIdentitiesModel
    {
        private int _id;
        private string _connectionId;

        public PlayerIdentitiesModel(int id, string connectionId)
        {
            _id = id;
            _connectionId = connectionId;
        }

        public int Id { get => _id; set => _id = value; }
        public string ConnectionId { get => _connectionId; set => _connectionId = value; }
    }
}
