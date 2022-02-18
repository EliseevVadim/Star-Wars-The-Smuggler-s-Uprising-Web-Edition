using System;
using System.Collections.Generic;
using System.Linq;
using SWGame.Core.Models;
using SWGame.Core.Exceptions;

namespace SWGame.Core.Management
{
    static class ActivePazaakChallenges
    {
        private static List<PazaakChallenge> _pazaakChallenges;
        private static Dictionary<string, string> _namesAndConnectionsIds;

        static ActivePazaakChallenges()
        {
            _pazaakChallenges = new List<PazaakChallenge>();
            _namesAndConnectionsIds = new Dictionary<string, string>();
        }

        public static List<PazaakChallenge> PazaakChallenges { get => _pazaakChallenges; }

        public static void AddChallenge(string creator, int amount, string connectionId)
        {
            if (ChallengeAlreadyExists(creator))
            {
                throw new ChallengeAlreadyExistException();
            }
            _namesAndConnectionsIds.Add(connectionId, creator);
            PazaakChallenge challenge = new PazaakChallenge()
            {
                Creator = creator,
                Amount = amount,
                Name = creator + "#" + new Guid().ToString()
            };
            _pazaakChallenges.Add(challenge);
        }

        public static void RemoveChallengeByConnectionId(string connectionId)
        {
            string creator = _namesAndConnectionsIds.GetValueOrDefault(connectionId);
            if (creator == null)
                return;
            PazaakChallenge target = _pazaakChallenges.Where(challenge => challenge.Creator == creator).FirstOrDefault();
            if (target != null)
            {
                _pazaakChallenges.Remove(target);
            }
        }

        private static bool ChallengeAlreadyExists(string creator)
        {
            return _pazaakChallenges.Where(challenge => challenge.Creator == creator).Any();
        }
    }
}
