using System.Collections.Generic;

namespace SWGame.Core.Models
{
    public class ChallengeIdentitiesModel
    {
        public string ChallengeName { get; set; }
        public List<string> PlayersIds { get; set;}

        public ChallengeIdentitiesModel()
        {
            PlayersIds = new List<string>();
        }
    }
}
