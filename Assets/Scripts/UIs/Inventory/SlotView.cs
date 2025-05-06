using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotView :  MonoBehaviour
{
    private EventTrigger eventTrigger;
    public ItemsEnum itemType;
    public bool isEmpty;
    [SerializeField] public Image imageItem;
    public TMP_Text itemAmount;
    public GameObject hightLightBorder;
    public event Action<SlotView> onPointerClick, onBeginDrag, onEndDrag, onDrop;
    private void Awake()
    {
       
    }
    public void Initialize()
    {
        eventTrigger = GetComponent<EventTrigger>();
        isEmpty = true;
        EventTrigger.Entry clickEntry = new EventTrigger.Entry();
        clickEntry.eventID = EventTriggerType.PointerClick;
        clickEntry.callback.AddListener((data) =>
        {   
            OnPointerClick((PointerEventData)data);
        });
        eventTrigger.triggers.Add(clickEntry);
        EventTrigger.Entry dragEntry = new EventTrigger.Entry();
        dragEntry.eventID = EventTriggerType.BeginDrag;
        dragEntry.callback.AddListener((data) =>
        {
            OnBeginDrag((PointerEventData)data);
        });
        eventTrigger.triggers.Add(dragEntry);
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry();
        endDragEntry.eventID = EventTriggerType.EndDrag;
        endDragEntry.callback.AddListener((data) =>
        {
            OnEndDrag((PointerEventData)data);
        });
        eventTrigger.triggers.Add(endDragEntry);
        EventTrigger.Entry dropEntry = new EventTrigger.Entry();
        dropEntry.eventID = EventTriggerType.Drop;
        dropEntry.callback.AddListener((data) =>
        {
            OnDrop((PointerEventData)data);
        });
        eventTrigger.triggers.Add(dropEntry);
    }
    public void Select()
    {
       hightLightBorder.active = true;
    }
    public void DeSelect()
    {
        hightLightBorder.active = false;
    }
    public void SetDataSlot(ItemsEnum itemType, Sprite spriteItem, int amount)
    {
        this.itemType = itemType;
        imageItem.sprite = spriteItem;
        itemAmount.text = amount.ToString();
        isEmpty = false;
        imageItem.gameObject.active = true;
        itemAmount.gameObject.active = true;
    }
    public void SetDataSlot(SlotView slot)
    {
        itemType = slot.itemType;
        imageItem.sprite = slot.imageItem.sprite;
        itemAmount.text = slot.itemAmount.text;
    }
    public void UpdateSlot(int amount)
    {
        itemAmount.text = amount.ToString();
    }
    public void ClearSlot()
    {
        imageItem.sprite = null;
        isEmpty = true;
        itemType = ItemsEnum.none;
        imageItem.gameObject.active = false;
        itemAmount.gameObject.active = false;
    }
    public  void OnBeginDrag(PointerEventData eventData)
    {
        if(itemType == ItemsEnum.none)
        {
            return;
        }
        EventCenter.Instance().OnSlotBeginDrag(this);
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        EventCenter.Instance().OnSlotEndDrag(this);
    }
    public void OnDrop(PointerEventData eventData)
    {
        EventCenter.Instance().OnSlotDrop(this);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        EventCenter.Instance().OnSlotClick(this);
        Debug.Log("click to " + itemType.ToString());
    }
}
