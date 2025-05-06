using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventoryView : MonoBehaviour, IScreen
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemName;
    private List<SlotView> listSlots;
    private SlotView selectedSlot;
    public Button closeButton;
    public event Action<bool> toggleAction;
    [SerializeField] private EnumScreen screenType;
    private SlotView draggingSlot;
    [SerializeField] private GameObject grid;
    [SerializeField] private GameObject slotObject;
    [SerializeField] private SubMenu subMenu;
    [SerializeField] private int size;
    [SerializeField] private DragSlotView dragView;
    [SerializeField] private List<Image> listUsableItemImage;
    [SerializeField] private Button removeItemBar;
    public void Initialize()
    {
        subMenu.useButton.onClick.AddListener(UseSelectedItem);
        subMenu.dropButton.onClick.AddListener(DropSelectedItem);
        removeItemBar.onClick.AddListener(InventoryController.Instance().RemoveItemInItemBar);
        closeButton.onClick.AddListener(InventoryController.Instance().SetUsableItemToBar);
        listSlots = new List<SlotView>();
        for(int i = 0; i < size; i++)
        {
            GameObject slotClone = Instantiate(slotObject, grid.transform);
            SlotView slotView = slotClone.GetComponent<SlotView>();
            slotView.Initialize();
            listSlots.Add(slotView);
        }
        EventCenter.Instance().onPointerClick += OnSlotClick;
        EventCenter.Instance().onBeginDrag += OnSlotBeginDrag;
        EventCenter.Instance().onEndDrag += OnSlotEndDrag;
        EventCenter.Instance().onDrop += OnSlotDrop;
    }
    public void Reaload()
    {
        foreach(SlotView slot in listSlots)
        {
            slot.ClearSlot();
        }
        foreach(Image image in listUsableItemImage)
        {
            image.sprite = null;
        }
    }
    public void AddItemToSlot(ItemModel item)
    {
        SlotView slotFound = listSlots.Find(slot => slot.itemType == item.GetItemType());
        if (slotFound == null)
        {
            SlotView emptySlot = listSlots.Find(slot => slot.isEmpty);
            if (emptySlot == null)
            {
                Debug.Log("check111111111111111111");
            }
            else
            {
                Debug.Log("check222222222222222222");
            }
            emptySlot.SetDataSlot(item.GetItemType(), item.GetItemImage(), item.GetAmount());
        }
        else
        {
            Debug.Log("found a slot");
        }
    }
    public void UpdateSlot(int amount, ItemsEnum item)
    {
        SlotView slotFound = listSlots.Find(slot => slot.itemType == item);
        if (slotFound != null)
        {
            slotFound.UpdateSlot(amount);
        }
        if(amount == 0)
        {
            slotFound.ClearSlot();
        }
    }
    public void OnSlotClick(SlotView slot)
    {
        subMenu.Hide();
        if (selectedSlot != null)
        {
            selectedSlot.DeSelect();
        }
        if (slot.isEmpty)
        {
            Debug.Log("clicked to empty slot "+ listSlots.IndexOf(slot));
            return;
        }
        slot.Select();
        selectedSlot = slot;
        InventoryController.Instance().SetSelectItem(slot.itemType);
        if (InventoryController.Instance().IsUsableItem(slot.itemType))
        {
            subMenu.ShowAll();
        }
        else
        {
            subMenu.ShowExceptUseButton();
        }
    }
    public void OnSlotBeginDrag(SlotView slot)
    {
        Debug.Log("slot drag = " + slot.itemType.ToString());
        if (selectedSlot.itemType == slot.itemType)
        {
            InventoryController.Instance().ClearSelected();
        }
        dragView.gameObject.active = true;
        dragView.imageItem.sprite = slot.imageItem.sprite;
        draggingSlot = slot;
    }
    public void OnSlotEndDrag(SlotView slot)
    {
        Debug.Log("checked end drag");
        dragView.gameObject.active = false;
    }

    public void OnSlotDrop(SlotView slot)
    {
        Debug.Log("checked drop");
        if (slot.isEmpty)
        {
            return;
        }
        if (selectedSlot.itemType == slot.itemType)
        {
            InventoryController.Instance().ClearSelected();
        }
        GameObject slotTemp = Instantiate(slotObject);
        slotTemp.SetActive(false);
        SlotView temp = slotTemp.GetComponent<SlotView>();
        if(temp != null)
        {
            temp.SetDataSlot(slot);
            slot.SetDataSlot(draggingSlot);
            draggingSlot.SetDataSlot(temp);
            Destroy(slotTemp);
        }
    }
    public void UseSelectedItem()
    {
        InventoryController.Instance().AddItemToItemBar();
    }
    public void DropSelectedItem()
    {
        InventoryController.Instance().DropSelectedItem();
    }
    public void ShowItemInfor(Sprite sprite, string name)
    {
        if(sprite == null)
        {
            Debug.Log("sprite null");
        }
        if (name == null)
        {
            Debug.Log("name null");
        }
        itemImage.sprite = sprite;
        itemName.text = name;
    }

    public EnumScreen GetScreenType()
    {
        return screenType;
    }
    public void ClearSelected()
    {
        itemImage.sprite = null;
        itemName.text = "";
        selectedSlot.DeSelect();
        subMenu.Hide();
    }
    public void RemoveItemInSelectionBar(int index)
    {
        listUsableItemImage[index].sprite = null;
    }
    public void AddItemToSelectionBar(Sprite image,int index)
    {
        listUsableItemImage[index].sprite = image;
    }
    public void ToggleInventory()
    {
        //toggleAction?.Invoke(gameObject.active);
        EventCenter.Instance().OnToggleInventoryUi(gameObject.active);
    }
}
