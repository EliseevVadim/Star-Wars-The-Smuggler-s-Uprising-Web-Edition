using SWGame.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace SWGame.Core.Management
{
    static class OnlineUsers
    {
        private static List<PlayerIdentitiesModel> _onlineUsersIds;
        static OnlineUsers()
        {
            _onlineUsersIds = new List<PlayerIdentitiesModel>();
        }

        public static void AddUserId(PlayerIdentitiesModel identitiesModel)
        {
            _onlineUsersIds.Add(identitiesModel);
        }

        public static int GetUserIdByConnectionId(string connectionId)
        {
            try
            {
                return _onlineUsersIds.FirstOrDefault(user => user.ConnectionId == connectionId).Id;
            }
            catch
            {
                return 0;
            }
        }

        public static void RemoveUserByPlayersId(int id)
        {
            _onlineUsersIds.RemoveAll(user => user.Id == id);
        }

        public static void RemoveUserByConnectionId(string connectionId)
        {
            _onlineUsersIds.RemoveAll(user => user.ConnectionId == connectionId);
        }

        public static bool IsUserOnline(int id)
        {
            foreach (var user in _onlineUsersIds)
            {
                if (user.Id == id)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
