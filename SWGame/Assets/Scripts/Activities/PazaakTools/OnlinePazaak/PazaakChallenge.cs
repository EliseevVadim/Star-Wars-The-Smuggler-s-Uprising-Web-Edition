namespace SWGame.Activities.PazaakTools.OnlinePazaak
{
    public class PazaakChallenge
    {
        private string _creator;
        private int _amount;
        private string _name;

        public int Amount { get => _amount; set => _amount = value; }
        public string Name { get => _name; set => _name = value; }
        public string Creator { get => _creator; set => _creator = value; }
    }
}
