using SWGame.Core.Models;
using SWGame.Core.Management;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using SWGame.Core.Models.Items.Cards;
using SWGame.Core.Enums;
using System;
using SWGame.Core.Models.Items;

namespace SWGame.Core.Repositories
{
    public class ItemsRepository
    {
        public List<ClassicalCard> LoadClassicalCards()
        {
            List<ClassicalCard> cards = new List<ClassicalCard>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = "SELECT item.id, item.Name, item.Description, classicalcard.Value, item.SalePrice " +
                    "FROM item INNER JOIN classicalcard ON item.id = classicalcard.ItemId";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    ClassicalCard addition = new ClassicalCard((int)reader[0], (string)reader[1], (int)reader[3]);
                    addition.Descriprion = (string)reader[2];
                    addition.SalePrice = (int)reader[4];
                    cards.Add(addition);
                }
            }
            return cards;
        }

        public List<Card> LoadSystemCards()
        {
            List<Card> cards = new List<Card>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = "SELECT item.id, item.Name, item.Description, systemcard.Value " +
                    "FROM item INNER JOIN systemcard ON item.id = systemcard.ItemId";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Card addition = new Card((int)reader[0], (string)reader[1], (int)reader[3]);
                    addition.Descriprion = (string)reader[2];
                    cards.Add(addition);
                }
            }
            return cards;
        }

        public List<FlippableCard> LoadFlippableCards()
        {
            List<FlippableCard> cards = new List<FlippableCard>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = "SELECT item.id, item.Name, item.Description, flippablecard.Value, item.SalePrice " +
                    "FROM item INNER JOIN flippablecard ON item.id = flippablecard.ItemId";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FlippableCard addition = new FlippableCard((int)reader[0], (string)reader[1], (int)reader[3]);
                    addition.Descriprion = (string)reader[2];
                    addition.SalePrice = (int)reader[4];
                    cards.Add(addition);
                }
            }
            return cards;
        }

        public List<GoldCard> LoadGoldCards()
        {
            List<GoldCard> cards = new List<GoldCard>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = "SELECT item.id, item.Name, item.Description, goldcard.Value, item.SalePrice " +
                    "FROM item INNER JOIN goldcard ON item.id = goldcard.ItemId";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    GoldCard addition = new GoldCard((int)reader[0], (string)reader[1],
                        (GoldCardType)Enum.Parse(typeof(GoldCardType), (string)reader[3]), 0);
                    addition.Descriprion = (string)reader[2];
                    addition.SalePrice = (int)reader[4];
                    cards.Add(addition);
                }
            }
            return cards;
        }

        public List<LootItem> LoadLootItems()
        {
            List<LootItem> lootItems = new List<LootItem>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = "SELECT item.id, item.Name, item.Description, item.SalePrice, lootitem.PrestigeValue, lootitem.WisdomValue, lootitem.ImageIndex, lootitem.FactionId " +
                    "FROM item INNER JOIN lootitem ON item.id = lootitem.ItemId";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LootItem addition = new LootItem((int)reader[0], (string)reader[1], (string)reader[2], (int)reader[4], (int)reader[5], (int)reader[6], (int)reader[7]);
                    try
                    {
                        addition.SalePrice = (int)reader[3];
                    }
                    catch
                    {

                    }
                    lootItems.Add(addition);
                }
            }
            return lootItems;
        }

        public List<QuestItem> LoadQuestItems()
        {
            List<QuestItem> questItems = new List<QuestItem>();
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = "SELECT item.id, item.Name, item.Description, questitem.ImageIndex, item.SalePrice " +
                    "FROM item INNER JOIN questitem ON item.id = questitem.ItemId";
                MySqlCommand command = new MySqlCommand(query, connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuestItem addition = new QuestItem((int)reader[0], (string)reader[1], (string)reader[2], (int)reader[3]);
                    addition.SalePrice = (int)reader[4];
                    questItems.Add(addition);
                }
            }
            return questItems;
        }
    }
}
