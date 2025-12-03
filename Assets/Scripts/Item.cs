using UnityEngine;

public class Item
{
    public enum ItemType
    {
        Money,
        Baking_Kit,
        Cookie_Core,
        Creampuff_Core,
        Cupcake_Core,
        Bonbon_Core,
        Muffin_Core,
        Vanilla,
        Cinnamon,
        Sparkling_Water,
        Nutmeg,
        Himalayan_Salt,
        Lemon_Juice,
        Wasabi,
        Rolled_Oats,
        Whipped_Cream,
        Frosting
    }

    private int amountOwned;
    private int price = 0;
    private ItemType type;

    public int GetAmountOwned() { return amountOwned; }
    public ItemType GetItemType() { return type; }
    public int GetPrice() { return price; }

    public void SetAmountOwned(int value) { amountOwned = value; }
    public void SetItemType(ItemType type) { this.type = type; }
    public void SetPrice(int value) {  price = value; }
}
