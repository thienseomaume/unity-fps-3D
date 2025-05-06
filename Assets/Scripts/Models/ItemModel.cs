using UnityEngine;
using UnityEngine.UI;

public class ItemModel
{
    public ItemsEnum itemType;
    public int amount;

    public string itemName;
    public Sprite itemImage;

    public ItemModel()
    {

    }
    public ItemModel(ItemsEnum itemType, string itemName, int amount, Sprite itemImage)
    {
        this.itemType = itemType;
        this.itemName = itemName;
        this.amount = amount;
        this.itemImage = itemImage;
        Debug.Log("item " + itemType.ToString() + "set amount = " + amount);
    }
    public void SetItemType(ItemsEnum itemType)
    {
        this.itemType = itemType;
    }
    public ItemsEnum GetItemType()
    {
        return this.itemType;
    }
    public void SetItemName(string itemName)
    {
        this.itemName = itemName;
    }
    public string GetItemName()
    {
        return this.itemName;
    }
    public void SetAmount(int amount)
    {
        this.amount = amount;
    }
    public int GetAmount()
    {
        return this.amount;
    }
    public Sprite GetItemImage()
    {
        return this.itemImage;
    }
    public void SetItemImage(Sprite itemImage)
    {
        this.itemImage = itemImage;
    }
    public void DownItem(int amount)
    {
        Debug.Log("old amount = " + this.amount);
        this.amount -= amount;
        Debug.Log("New amount = " + this.amount);
    }
    public void AddItem(int amount)
    {
        this.amount += amount;
    }
    public void RemoveItem(int amount)
    {
        this.amount -= amount;
    }
}
