using SWGame.Entities.Items;
using SWGame.GlobalConfigurations;
using System.Collections.Generic;

namespace SWGame.Management.Repositories
{
    static class ItemsRepository
    {
        private static List<LootItem> _lootItems;
        private static List<QuestItem> _questItems;
        private static List<LootItem> _sithItems;
        private static List<LootItem> _jediItems;

        static ItemsRepository()
        {
            _lootItems = new List<LootItem>();
            _questItems = new List<QuestItem>();
            _sithItems = new List<LootItem>();
            _jediItems = new List<LootItem>();
        }

        public static List<LootItem> LootItems { get => _lootItems; set => _lootItems = value; }
        public static List<QuestItem> QuestItems { get => _questItems; set => _questItems = value; }
        public static List<LootItem> SithItems { get => _sithItems; set => _sithItems = value; }
        public static List<LootItem> JediItems { get => _jediItems; set => _jediItems = value; }

        public static async void LoadAllItems(ClientManager manager)
        {
            await manager.LoadItems();
        }
    }
}
