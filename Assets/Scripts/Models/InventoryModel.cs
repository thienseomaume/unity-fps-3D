using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryModel 
{
    private List<ItemModel> listItems;
    public InventoryModel()
    {
        listItems = new List<ItemModel>();
    }
    public List<ItemModel> GetListItems()
    {
        return this.listItems;
    } 
    public void RemoveItem(int amount, ItemsEnum item)
    {
        ItemModel itemToRemove = listItems.Find(it => it.GetItemType() == item);
        if(itemToRemove != null)
        {
            if (itemToRemove.GetAmount() > amount)
            {
                Debug.Log("amount to remove = " + amount);
                itemToRemove.DownItem(amount);
            }
            else
            {
                return;
            }
            if (itemToRemove.GetAmount() == 0)
            {
               listItems.Remove(itemToRemove);
            }
        }
    }
    public void DropItem(ItemModel item)
    {
        listItems.Remove(item);
    }
    public void AddExistItem(int amount, ItemsEnum item)
    {
        ItemModel itemToAdd = listItems.Find(it => it.GetItemType() == item);
        if (itemToAdd != null)
        {
            itemToAdd.AddItem(amount);
        }
    }
    public void AddNewItem(ItemModel item)
    {
        if(item != null)
        {
            listItems.Add(item);
        }
    }
    public bool ChecExistItem(ItemsEnum item)
    {
        ItemModel itemFound = listItems.Find(it => it.GetItemType() == item);
        if (itemFound != null)
        {
            return true;
        }
        return false;
    }
    public int GetItemAmount(ItemsEnum item)
    {
        ItemModel itemFound = listItems.Find(it => it.GetItemType() == item);
        if (itemFound != null)
        {
            return itemFound.GetAmount();
        }
        return -1;
    }
}
