using SWGame.Entities.Items.Cards;
using SWGame.GlobalConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWGame.Management.Repositories
{
    static class CardsRepository
    {
        private static List<Card> _systemCards;
        private static List<ClassicalCard> _classicalCards;
        private static List<FlippableCard> _flippableCards;
        private static List<GoldCard> _goldCards;

        static CardsRepository()
        {
            _systemCards = new List<Card>();
            _classicalCards = new List<ClassicalCard>();
            _flippableCards = new List<FlippableCard>();
            _goldCards = new List<GoldCard>();
        }

        public static List<Card> SystemCards { get => _systemCards; set => _systemCards = value; }
        public static List<ClassicalCard> ClassicalCards { get => _classicalCards; set => _classicalCards = value; }
        public static List<FlippableCard> FlippableCards { get => _flippableCards; set => _flippableCards = value; }
        public static List<GoldCard> GoldCards { get => _goldCards; set => _goldCards = value; }

        public static async void LoadAllCards(ClientManager manager)
        {
            await manager.LoadPazaakCards();
        }
    }
}
