using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour, IInteractable
{
    [SerializeField] ItemsEnum type;
    [SerializeField] int number;
    public void Interact()
    {
        InventoryController.Instance().AddItem(number, type);
        Destroy(gameObject);
    }
    public void SetAmount(int amount)
    {
        number = amount;
    }
    public ItemsEnum GetItemType()
    {
        return type;
    }
}
