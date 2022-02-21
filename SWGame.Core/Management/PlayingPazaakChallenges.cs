using SWGame.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace SWGame.Core.Management
{
    static class PlayingPazaakChallenges
    {
        private static List<ChallengeIdentitiesModel> _games;

        static PlayingPazaakChallenges()
        {
            _games = new List<ChallengeIdentitiesModel>();
        }

        public static List<ChallengeIdentitiesModel> Games { get => _games; set => _games = value; }

        public static void AddGame(ChallengeIdentitiesModel model)
        {
            _games.Add(model);
        }

        public static string GetChallengeNameByConnectionId(string connectionId)
        {
            return _games.Where(game => game.PlayersIds.Contains(connectionId)).FirstOrDefault().ChallengeName;
        }

        public static void RemoveChallengeByGroupName(string group)
        {
            ChallengeIdentitiesModel model = _games.Where(game => game.ChallengeName == group).FirstOrDefault();
            try
            {
                _games.Remove(model);
            }
            catch
            {

            }
        }
    }
}
