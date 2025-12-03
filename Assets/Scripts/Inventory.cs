using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class Inventory
{
    private string filePath = "Assets/SaveData/InventoryData.txt";

    public Item[] items = new Item[(int)(Item.ItemType)Enum.GetValues(typeof(Item.ItemType)).Length];

    public void LoadItems()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }

        string[] lines = File.ReadAllLines(filePath);
        Debug.Log(items.Length);
        Debug.Log(lines.Length);

        for (int i = 0; i < lines.Length - 1; i++)
        {
            items[i] = new Item();
            items[i].SetItemType((Item.ItemType)Enum.GetValues(typeof(Item.ItemType)).GetValue(i));
            items[i].SetAmountOwned(int.Parse(lines[i]));
            if (lines[i] == "Baking_Kit") items[i].SetPrice(250);
            else if (lines[i].Contains("Core")) items[i].SetPrice(100);
            else if (lines[i] == "Money") continue;
            else items[i].SetPrice(50);
        }
    }

    private void SaveItems()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath);
        }

        string itemData = "";

        for (int i = 0; i < items.Length; i++)
        {
            Debug.Log(i);
            itemData += $"{items[i].GetAmountOwned()}\n";
        }

        itemData += $"{items[items.Length - 1].GetAmountOwned()}";

        File.WriteAllText(filePath, itemData);
    }

    public void UseItem(string itemUsed, int amountNeeded)
    {
        foreach (Item item in items)
        {
            if (item.GetItemType().ToString() == itemUsed)
            {
                if (item.GetAmountOwned() >= amountNeeded)
                {
                    item.SetAmountOwned(item.GetAmountOwned() - amountNeeded);
                }
                else continue;
            }
        }

        SaveItems();
    }

    public void GainItem(string itemGained, int amountGained)
    {
        int price = 0;
        foreach (Item item in items)
        {
            if (item.GetItemType().ToString() == itemGained)
            {
                price = amountGained * item.GetPrice();
                Debug.Log($"Price: {price}");
                item.SetAmountOwned(item.GetAmountOwned() + amountGained);
            }
        }

        for (int i = 0; i < items.Length - 1; i++)
        {
            if (items[i].GetItemType() == Item.ItemType.Money && items[i].GetAmountOwned() >= price)
            {
                items[i].SetAmountOwned(items[i].GetAmountOwned() - price);
            }
        }

        SaveItems();
    }
}
