using MySql.Data.MySqlClient;
using SWGame.Core.Enums;
using SWGame.Core.Management;
using SWGame.Core.Models.Items;
using SWGame.Core.Models.Items.Cards;
using System;
using System.Collections.Generic;

namespace SWGame.Core.Models
{
    public class Shop
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Revenue { get; set; }
        public int LocationId { get; set; }

        public List<ShopSlot> GetAllItems()
        {
            List<ShopSlot> slots = new List<ShopSlot>();
            LoadLootItems(slots);
            LoadQuestItems(slots);
            LoadClassicalCards(slots);
            LoadFlippableCards(slots);
            LoadGoldCards(slots);
            return slots;
        }

        private void LoadLootItems(List<ShopSlot> slots)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("SELECT item.id, item.Name, item.Description, lootitem.PrestigeValue, lootitem.WisdomValue, lootitem.ImageIndex, shopslot.Price, item.SalePrice, lootitem.FactionId " +
                    "FROM item INNER JOIN lootitem ON item.id = lootitem.ItemId " +
                    "INNER JOIN shopslot ON shopslot.ItemId = item.id " +
                    "WHERE shopslot.ShopId = @shopId");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@shopId", Id);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    LootItem lootItem = new LootItem((int)reader[0], (string)reader[1], (string)reader[2],
                        (int)reader[3], (int)reader[4], (int)reader[5], (int)reader[8]);
                    try
                    {
                        lootItem.SalePrice = (int)reader[7];
                    }
                    catch
                    {

                    }
                    slots.Add(new ShopSlot(Id, (int)reader[6], lootItem));
                }
            }
        }

        private void LoadQuestItems(List<ShopSlot> slots)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("SELECT item.id, item.Name, item.Description, questitem.ImageIndex, shopslot.Price, item.SalePrice " +
                    "FROM item INNER JOIN questitem ON item.id = questitem.ItemId " +
                    "INNER JOIN shopslot ON shopslot.ItemId = item.id " +
                    "WHERE shopslot.ShopId = @shopId");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@shopId", Id);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    QuestItem questItem = new QuestItem((int)reader[0], (string)reader[1], (string)reader[2], (int)reader[3]);
                    questItem.SalePrice = (int)reader[5];
                    slots.Add(new ShopSlot(Id, (int)reader[4], questItem));
                }
            }
        }

        private void LoadClassicalCards(List<ShopSlot> slots)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("SELECT item.id, item.Name, classicalcard.Value, item.Description, shopslot.Price, item.SalePrice " +
                    "FROM item INNER JOIN classicalcard ON item.id = classicalcard.ItemId " +
                    "INNER JOIN shopslot ON shopslot.ItemId = item.id " +
                    "WHERE shopslot.ShopId = @shopId");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@shopId", Id);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Card card = new ClassicalCard((int)reader[0], (string)reader[1], (int)reader[2]);
                    card.Descriprion = (string)reader[3];
                    card.SalePrice = (int)reader[5];
                    slots.Add(new ShopSlot(Id, (int)reader[4], card));
                }
            }
        }

        private void LoadFlippableCards(List<ShopSlot> slots)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("SELECT item.id, item.Name, flippablecard.Value, item.Description, shopslot.Price, item.SalePrice " +
                    "FROM item INNER JOIN flippablecard ON item.id = flippablecard.ItemId " +
                    "INNER JOIN shopslot ON shopslot.ItemId = item.id " +
                    "WHERE shopslot.ShopId = @shopId");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@shopId", Id);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Card card = new FlippableCard((int)reader[0], (string)reader[1], (int)reader[2]);
                    card.Descriprion = (string)reader[3];
                    card.SalePrice = (int)reader[5];
                    slots.Add(new ShopSlot(Id, (int)reader[4], card));
                }
            }
        }

        private void LoadGoldCards(List<ShopSlot> slots)
        {
            using (MySqlConnection connection = new MySqlConnection(DatabaseInformation.ConnectionString))
            {
                connection.Open();
                string query = string.Format("SELECT item.id, item.Name, goldcard.Value, item.Description, shopslot.Price, item.SalePrice " +
                    "FROM item INNER JOIN goldcard ON item.id = goldcard.ItemId " +
                    "INNER JOIN shopslot ON shopslot.ItemId = item.id " +
                    "WHERE shopslot.ShopId = @shopId");
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@shopId", Id);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Card card = new GoldCard((int)reader[0], (string)reader[1],
                        (GoldCardType)Enum.Parse(typeof(GoldCardType), (string)reader[2]), 0);
                    card.Descriprion = (string)reader[3];
                    card.SalePrice = (int)reader[5];
                    slots.Add(new ShopSlot(Id, (int)reader[4], card));
                }
            }
        }
    }
}
