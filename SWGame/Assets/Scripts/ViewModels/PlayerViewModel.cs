namespace SWGame.ViewModels
{
    public class PlayerViewModel
    {
        public string Name { get; set; }
        public int Prestige { get; set; }
        public int WisdomPoints { get; set; }
        public int AvatarIndex { get; set; }
        public bool StoryFinished { get; set; }
        public string PlanetName { get; set; }
        public string LocationName { get; set; }
        public bool? IsOnline { get; set; }
    }
}
