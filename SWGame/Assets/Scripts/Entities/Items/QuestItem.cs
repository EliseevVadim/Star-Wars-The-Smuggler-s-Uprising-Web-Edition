using SWGame.Management.Repositories;
using System.Collections.Generic;
using UnityEngine;

namespace SWGame.Entities.Items
{
    public class QuestItem : Item
    {

        public QuestItem(int id, string name, string description, int iconIndex)
        {
            _id = id;
            _name = name;
            _descriprion = description;
            _image = QuestItemsIconsRepository.Icons[iconIndex];
        }

        public override bool Equals(object obj)
        {
            return obj is QuestItem item &&
                   _id == item._id &&
                   _descriprion == item._descriprion &&
                   EqualityComparer<Sprite>.Default.Equals(_image, item._image) &&
                   _salePrice == item._salePrice &&
                   _name == item._name;
        }

        public override int GetHashCode()
        {
            int hashCode = 1406416487;
            hashCode = hashCode * -1521134295 + _id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_descriprion);
            hashCode = hashCode * -1521134295 + EqualityComparer<Sprite>.Default.GetHashCode(_image);
            hashCode = hashCode * -1521134295 + _salePrice.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_name);
            return hashCode;
        }
    }
}
