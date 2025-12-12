using System;
using System.IO;
using System.Linq;
using UnityEngine;

public class Inventory
{
    private string filePath;

    public Item[] items = new Item[(int)(Item.ItemType)Enum.GetValues(typeof(Item.ItemType)).Length];

    public void LoadItems()
    {
        filePath = Path.Combine(Application.persistentDataPath, "InventoryData.txt");

        // Create a file if it doesn't exist
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "");
            return;   // nothing to load yet
        }

        string[] lines = File.ReadAllLines(filePath);

        // Prevent reading more lines than items available
        int count = Mathf.Min(lines.Length, items.Length);

        for (int i = 0; i < count; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line))
                continue; // skip broken or empty lines safely

            // Create a fresh item
            items[i] = new Item();

            // Assign item type by index or other logic
            items[i].SetItemType((Item.ItemType)Enum.GetValues(typeof(Item.ItemType)).GetValue(i));

            // Safely parse the amount
            if (int.TryParse(line, out int amount))
                items[i].SetAmountOwned(amount);
            else
                items[i].SetAmountOwned(0); // fallback so game doesn't break

            // Safely set prices
            if (items[i].GetItemType() == Item.ItemType.Baking_Kit)
                items[i].SetPrice(250);
            else if (items[i].GetItemType().ToString().Contains("Core"))
                items[i].SetPrice(100);
            else if (items[i].GetItemType() == Item.ItemType.Money)
                continue;
            else
                items[i].SetPrice(50);
        }
    }

    public void SaveItems()
    {
        filePath = Path.Combine(Application.persistentDataPath, "InventoryData.txt");

        // Use StringBuilder for efficient, safe string creation
        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int i = 0; i < items.Length; i++)
        {
            // Skip null items instead of crashing
            int amount = (items[i] != null) ? items[i].GetAmountOwned() : 0;

            // Write one line per item
            sb.AppendLine(amount.ToString());
        }

        // Write the full inventory file in one atomic operation
        File.WriteAllText(filePath, sb.ToString());
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
