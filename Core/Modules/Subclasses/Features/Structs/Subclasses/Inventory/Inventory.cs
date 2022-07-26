using System.Collections.Generic;
using UnityEngine;

namespace Core.Modules.Subclasses.Features.Structs.Subclasses.Inventory
{
    public class Inventory
    {
        public readonly List<InventoryItem> Slot1 = new List<InventoryItem> {new InventoryItem { ItemType = ItemType.None, Chance = 100 }};
        public readonly List<InventoryItem> Slot2 = new List<InventoryItem> {new InventoryItem { ItemType = ItemType.None, Chance = 100 }};
        public readonly List<InventoryItem> Slot3 = new List<InventoryItem> {new InventoryItem { ItemType = ItemType.None, Chance = 100 }};
        public readonly List<InventoryItem> Slot4 = new List<InventoryItem> {new InventoryItem { ItemType = ItemType.None, Chance = 100 }};
        public readonly List<InventoryItem> Slot5 = new List<InventoryItem> {new InventoryItem { ItemType = ItemType.None, Chance = 100 }};
        public readonly List<InventoryItem> Slot6 = new List<InventoryItem> {new InventoryItem { ItemType = ItemType.None, Chance = 100 }};
        public readonly List<InventoryItem> Slot7 = new List<InventoryItem> {new InventoryItem { ItemType = ItemType.None, Chance = 100 }};
        public readonly List<InventoryItem> Slot8 = new List<InventoryItem> {new InventoryItem { ItemType = ItemType.None, Chance = 100 }};

        public List<ItemType> ToList()
        {
            var finalList = new List<ItemType>();
            
            foreach (var item in Slot1)
            {
                if (Random.Range(0, 101) > item.Chance) 
                    continue;
                finalList.Add(item.ItemType);
                break;
            }
            foreach (var item in Slot2)
            {
                if (Random.Range(0, 101) > item.Chance) 
                    continue;
                finalList.Add(item.ItemType);
                break;
            }
            foreach (var item in Slot3)
            {
                if (Random.Range(0, 101) > item.Chance) 
                    continue;
                finalList.Add(item.ItemType);
                break;
            }
            foreach (var item in Slot4)
            {
                if (Random.Range(0, 101) > item.Chance) 
                    continue;
                finalList.Add(item.ItemType);
                break;
            }
            foreach (var item in Slot5)
            {
                if (Random.Range(0, 101) > item.Chance) 
                    continue;
                finalList.Add(item.ItemType);
                break;
            }
            foreach (var item in Slot6)
            {
                if (Random.Range(0, 101) > item.Chance) 
                    continue;
                finalList.Add(item.ItemType);
                break;
            }
            foreach (var item in Slot7)
            {
                if (Random.Range(0, 101) > item.Chance) 
                    continue;
                finalList.Add(item.ItemType);
                break;
            }
            foreach (var item in Slot8)
            {
                if (Random.Range(0, 101) > item.Chance) 
                    continue;
                finalList.Add(item.ItemType);
                break;
            }
            

            return finalList;
        }
    }
}