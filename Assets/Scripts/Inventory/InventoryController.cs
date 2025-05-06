using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    private static InventoryController instance;
    private InventoryModel inventoryModel;
    [SerializeField]private InventoryView inventoryView;
    [SerializeField] private List<GameObject> listUsableItems;
    [SerializeField] private List<ItemsEnum> listSelectedItems;
    private ItemsEnum selectedItem = ItemsEnum.none;
    public ItemSpawner itemSpawner;
    private void Awake()
    {
        inventoryModel = new InventoryModel();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public static InventoryController Instance()
    {
        return instance;
    }
    private void Start()
    {
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryView.ToggleInventory();
        }
    }
    public void Initialize()
    {
        SaveDataModel saveData = GameManager.Instance().saveData;
        GameManager.Instance().saveAction += SaveInventory;
        foreach (var item in saveData.listItem)
        {
            ItemModel itemExist = inventoryModel.GetListItems().Find(i => i.GetItemType() == item.Key);
            if (itemExist == null)
            {
                ItemModel newItem = CreateNewItem(item.Value, item.Key);
                if (newItem == null)
                {
                    Debug.Log("cannot load and create item from json ");
                    return;
                }
                else
                {
                    inventoryView.AddItemToSlot(newItem);
                    inventoryModel.AddNewItem(newItem);
                }
            }
        }
        foreach(var gun in saveData.listGun)
        {
            GameObject gunObject = GetUsableItem(gun.Key);
            if (gunObject != null)
            {
                BaseGun gunScript = gunObject.GetComponent<BaseGun>();
                if (gunScript != null)
                {
                    gunScript.SetMagazine(gun.Value.currentMagazine);
                }
            }
        }
        for(int i = 0; i < 3; i++)
        {
            if (i> GameManager.Instance().saveData.listIdSelectionBar.Count - 1)
            {
                listSelectedItems.Add(ItemsEnum.none);
            }
            else
            {
                ItemsEnum item = GameManager.Instance().saveData.listIdSelectionBar[i];
                listSelectedItems.Add(item);
                inventoryView.AddItemToSelectionBar(inventoryModel.GetListItems()?.Find(it => it.GetItemType() == item)?.GetItemImage(), i);
            }
        }
    }
    public void Reload()
    {
        inventoryModel = new InventoryModel();
        listSelectedItems = new List<ItemsEnum>();
        inventoryView.Reaload();
        SaveDataModel saveData = GameManager.Instance().saveData;
        foreach (var item in saveData.listItem)
        {
            ItemModel itemExist = inventoryModel.GetListItems().Find(i => i.GetItemType() == item.Key);
            if (itemExist == null)
            {
                ItemModel newItem = CreateNewItem(item.Value, item.Key);
                if (newItem == null)
                {
                    Debug.Log("cannot load and create item from json ");
                    return;
                }
                else
                {
                    inventoryView.AddItemToSlot(newItem);
                    inventoryModel.AddNewItem(newItem);
                }
            }
        }
        foreach (var gun in saveData.listGun)
        {
            GameObject gunObject = GetUsableItem(gun.Key);
            if (gunObject != null)
            {
                BaseGun gunScript = gunObject.GetComponent<BaseGun>();
                if (gunScript != null)
                {
                    gunScript.SetMagazine(gun.Value.currentMagazine);
                }
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (i > GameManager.Instance().saveData.listIdSelectionBar.Count - 1)
            {
                listSelectedItems.Add(ItemsEnum.none);
            }
            else
            {
                ItemsEnum item = GameManager.Instance().saveData.listIdSelectionBar[i];
                listSelectedItems.Add(item);
                inventoryView.AddItemToSelectionBar(inventoryModel.GetListItems()?.Find(it => it.GetItemType() == item)?.GetItemImage(), i);
            }
        }
    }
    public void RemoveItem(int amount, ItemsEnum item)
    {
        inventoryModel.RemoveItem(amount, item);
        inventoryView.UpdateSlot(inventoryModel.GetListItems().Find(it => it.GetItemType() == item).GetAmount(), item);
    }
    public void AddItem(int amount, ItemsEnum item)
    {
        if (inventoryModel.ChecExistItem(item))
        {
            Debug.Log("checked exist item");
            inventoryModel.AddExistItem(amount, item);
            inventoryView.UpdateSlot(inventoryModel.GetListItems().Find(it => it.GetItemType() == item).GetAmount(), item);
        }
        else
        {
            Debug.Log("checked create new item");
            ItemModel newItem = CreateNewItem(amount, item);
            if(newItem != null)
            {
                Debug.Log("checked create new item 1");
                inventoryModel.AddNewItem(newItem);
                inventoryView.AddItemToSlot(newItem);
            }
        }
    }
    private ItemModel CreateNewItem(int amount, ItemsEnum item)
    {
        string itemNameFile = item.ToString();
        string itemName = itemNameFile;
        string imagePath = Application.streamingAssetsPath + "/"+ itemNameFile + ".png";
        if (File.Exists(imagePath))
        {
            byte[] dataImage = File.ReadAllBytes(imagePath);
            Texture2D texture2d = new Texture2D(2, 2);
            Sprite sprite;
            if (texture2d.LoadImage(dataImage))
            {
                sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height),new Vector2(0.5f ,0.5f));
                Debug.Log("create new item");
                return new ItemModel(item, itemName, amount, sprite);
            }
            else
            {
                Debug.Log("failed to load from path " + imagePath);
            }
        }
        else
        {
            Debug.Log("file not found: "+ imagePath);
        }
        return null;
    }
    public List<ItemModel> GetListItem()
    {
        return inventoryModel.GetListItems();
    }
    public void SetSelectItem(ItemsEnum itemSelect)
    {
        selectedItem = itemSelect;
        Debug.Log("selected item = " + itemSelect.ToString());
        ItemModel itemFound = inventoryModel.GetListItems().Find(i => i.GetItemType() == itemSelect);
        if (itemFound == null)
        {
            Debug.Log("item found null");
        }
        inventoryView.ShowItemInfor(itemFound.GetItemImage(), itemFound.GetItemName());
    }
    public void ClearSelected()
    {
        selectedItem = ItemsEnum.none;
        inventoryView.ClearSelected();
    }
    public void DropSelectedItem()
    {
        ItemModel item = inventoryModel.GetListItems().Find(it => it.GetItemType() == selectedItem);
        if( item != null)
        {
            inventoryModel.DropItem(item);
            itemSpawner.SpawnItem(item.GetItemType(), item.GetAmount(), PlayerInformation.Instance().transform.position);
            ClearSelected();
            inventoryView.UpdateSlot(0, item.GetItemType());
        }
    }
    public void UseSelectedItem()
    {
        if (IsUsableItem(selectedItem))
        {
            return;
        }
        else
        {
            
        }
        UseItem(selectedItem);
    }
    public void UseItem(ItemsEnum item)
    {
        switch (selectedItem)
        {
            case ItemsEnum.ammo:
                inventoryModel.GetListItems().Find(it => it.GetItemType() == ItemsEnum.ammo)?.RemoveItem(1);
                break;
            case ItemsEnum.health:
                
                break;
        }
    }
    
    public int GetItemAmount(ItemsEnum item)
    {
       return inventoryModel.GetItemAmount(item);
    }
    public bool IsUsableItem(ItemsEnum item)
    {
        foreach(GameObject itemUsable in listUsableItems)
        {
            IItem itemFound = itemUsable.GetComponent<IItem>();
            if (itemFound != null)
            {
                if (itemFound.GetItemType() == item)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public void RemoveItemInItemBar()
    {
        foreach(ItemsEnum item in listSelectedItems)
        {
            int index = listSelectedItems.IndexOf(item);
            if (item == ItemsEnum.none && index > 0)
            {
                
                listSelectedItems[index - 1] = ItemsEnum.none;
                inventoryView.RemoveItemInSelectionBar(index - 1);
                return;
            }
            else if(index == listSelectedItems.Count-1)
            {
                Debug.Log("checked remove step 1");
                listSelectedItems[index] = ItemsEnum.none;
                inventoryView.RemoveItemInSelectionBar(index);
                return;
            }
        }
    }
    public void AddItemToItemBar()
    {
        if (IsSelectionBarFull() ||selectedItem == ItemsEnum.none || listSelectedItems.Contains(selectedItem))
        {
            return;
        }
        else
        {
            int index = 0;
            foreach(ItemsEnum item in listSelectedItems)
            {
                if (item == ItemsEnum.none)
                {
                    listSelectedItems[index] = selectedItem;
                    inventoryView.AddItemToSelectionBar(inventoryModel.GetListItems()?.Find(it => it.GetItemType() == selectedItem)?.GetItemImage(), index);
                    return;
                }
                index += 1;
            }
        }
    }
    public bool IsSelectionBarFull()
    {
        foreach(ItemsEnum item in listSelectedItems)
        {
            if(item == ItemsEnum.none)
            {
                return false;
            }
        }
        return true;
    }
    public void SetUsableItemToBar()
    {
        List<Sprite> listSprite = new List<Sprite>();
        List<GameObject> listSelectedUsable = new List<GameObject>();
        foreach(ItemsEnum item in listSelectedItems)
        {
            Sprite itemImageFound =  inventoryModel.GetListItems().Find(it => it.GetItemType() == item)?.GetItemImage();
            listSprite.Add(itemImageFound);
            GameObject itemFound = listUsableItems.Find(it => it.GetComponent<IItem>()?.GetItemType() == item);
            listSelectedUsable.Add(itemFound);
        }
        SelectionBar.Instance().SetListSlots(listSprite);
        SelectionBar.Instance().SetListItems(listSelectedUsable);
        SelectionBar.Instance().DeselectAllSlots();
    }
    public GameObject GetUsableItem(ItemsEnum item)
    {
        foreach (GameObject itemUsable in listUsableItems)
        {
            IItem itemFound = itemUsable.GetComponent<IItem>();
            if (itemFound != null)
            {
                if (itemFound.GetItemType() == item)
                {
                    return itemUsable;
                }
            }
        }
        return null;
    }
    //public GameObject GetUsableItem(ItemsEnum item)
    //{
    //    return listUsableItems.Find(it => it.GetComponent<IItem>().GetItemType() == item)?.gameObject;
    //}
    void SaveInventory()
    {
        if (inventoryModel.GetListItems() != null)
        {
            foreach (ItemModel item in inventoryModel.GetListItems())
            {
                GameManager.Instance().saveData.listItem[item.GetItemType()] = item.GetAmount();
                GameObject usableItem = GetUsableItem(item.GetItemType());
                if(usableItem != null)
                {
                    BaseGun gun = usableItem.GetComponent<BaseGun>();
                    if(gun != null)
                    {
                        GunModel gunModel = new GunModel(gun.GetComponent<IItem>().GetItemType(), gun.GetMagazine(), gun.IsOwned());
                        GameManager.Instance().saveData.listGun[gunModel.id] = gunModel;
                    }
                }
            }
        }
    }
}
