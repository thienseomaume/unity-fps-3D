using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SelectionBar : MonoBehaviour
{
    private static SelectionBar instance;
    [SerializeField]private List<SelectionBarSlot> listSlots;
    private List<GameObject> listUsableItems = new List<GameObject>();
    private GameObject selectedItem;
    public event Action<int> magazineAction;
    public event Action<bool> magazineUiToggle;
    public event Action onUsingItemLeftClick, onUsingItemR, onUsingItemE, onUsingItemQ, onUsingNone;
    public AnimatorOverrideController emptyArmsAnimatorOverrride;
    private void Awake()
    {
        if(instance == null)
        {
            Debug.Log("init instance of selection bar");

            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public static SelectionBar Instance()
    {
        return instance;
    }
    private void OnEnable()
    {
        Debug.Log("selection bar on enable call");
    }
    public void Initialize()
    {
        GameManager.Instance().saveAction += SaveData;
        if(GameManager.Instance().saveData.listIdSelectionBar.Count > 0)
        {
            foreach(ItemsEnum item in GameManager.Instance().saveData.listIdSelectionBar)
            {
                GameObject usableItem;
                if (item == ItemsEnum.none)
                {
                    usableItem = null;
                }
                else
                {
                    Debug.Log(item.ToString());
                    usableItem = InventoryController.Instance().GetUsableItem(item);
                }
                if (usableItem != null)
                {
                    Debug.Log(usableItem.ToString());
                    listUsableItems.Add(usableItem);
                    Sprite sprite = GetItemSprite(item.ToString());
                    listSlots[listUsableItems.IndexOf(usableItem)].SetItemImage(sprite);
                }
            }
        }
        SetSelectedItem(1);
    }
    public void Reload()
    {
        foreach(SelectionBarSlot slot in listSlots)
        {
            slot.SetItemImage(null);
        }
        listUsableItems = new List<GameObject>();
        if (GameManager.Instance().saveData.listIdSelectionBar.Count > 0)
        {
            foreach (ItemsEnum item in GameManager.Instance().saveData.listIdSelectionBar)
            {
                GameObject usableItem;
                if (item == ItemsEnum.none)
                {
                    usableItem = null;
                }
                else
                {
                    Debug.Log(item.ToString());
                    usableItem = InventoryController.Instance().GetUsableItem(item);
                }
                if (usableItem != null)
                {
                    Debug.Log(usableItem.ToString());
                    listUsableItems.Add(usableItem);
                    Sprite sprite = GetItemSprite(item.ToString());
                    listSlots[listUsableItems.IndexOf(usableItem)].SetItemImage(sprite);
                }
            }
        }
        SetSelectedItem(1);
    }
    public Sprite GetItemSprite(string name)
    {
        
        string imagePath = Application.streamingAssetsPath + "/" + name+".png";
        if (File.Exists(imagePath))
        {
            byte[] imageData = File.ReadAllBytes(imagePath);
            Texture2D texture2d = new Texture2D(2, 2);
            if (texture2d.LoadImage(imageData))
            {
                Sprite sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), new Vector2(0.5f, 0.5f));
                return sprite;
            }
            else
            {
                Debug.Log("cannot read image from path: " + imagePath);
                return null;
            }
        }
        else
        {
            Debug.Log("file not exist ");
            return null;
        }
    }
    public void SetListSlots(List<Sprite> listSprites)
    {
        int index = 0;
        foreach(Sprite sprite in listSprites)
        {
            Debug.Log("checked index sprite = " + index);
            listSlots[index].SetItemImage(sprite);
            index += 1;
        }
    }
    public void SetListItems(List<GameObject> listGameObject)
    {
        listUsableItems = listGameObject;
    }
    public void ChangSelectedItemHandle()
    {
        if (selectedItem!=null && selectedItem.GetComponent<IUsable>().IsUsing())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("check input number 1");
            SetSelectedItem(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("check input number 2");
            SetSelectedItem(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("check input number 3");
            SetSelectedItem(3);
        }
    }
    public void SetSelectedItem(int index)
    {
        if (selectedItem != null && selectedItem.GetComponent<IUsable>().IsUsing())
        {
            return;
        }
        if (selectedItem != null)
        {
            selectedItem.SetActive(false);
        }
        index -= 1;
        if (index > listUsableItems.Count-1)
        {
            selectedItem = null;
            PlayerInformation.Instance().OverrideAnimator(emptyArmsAnimatorOverrride);
            SelectSlot(index);
        }
        else if (listUsableItems[index] != null)
        {
            selectedItem = listUsableItems[index];
            PlayerInformation.Instance().OverrideAnimator(selectedItem.GetComponent<IUsable>().GetAnimatorOverride());
            selectedItem.SetActive(true);
            SelectSlot(index);
        }
        else
        {
            selectedItem = null;
            PlayerInformation.Instance().OverrideAnimator(emptyArmsAnimatorOverrride);
            SelectSlot(index);
        }
        EventCenter.Instance().OnChangeSelectedItem(selectedItem);
    }
    public void SelectSlot(int index)
    {
        foreach(SelectionBarSlot slot in listSlots)
        {
            slot.DeSelect();
        }
        listSlots[index].Select();
    }
    public void DeselectAllSlots()
    {
        foreach (SelectionBarSlot slot in listSlots)
        {
            slot.DeSelect();
        }
        if (selectedItem != null)
        {
            selectedItem.SetActive(false);
            selectedItem = null;
        }
            PlayerInformation.Instance().OverrideAnimator(emptyArmsAnimatorOverrride);
    }
    public void PlayerLeftClick()
    {
        if (selectedItem!=null && selectedItem.GetComponent<IUsable>()!=null)
        {
            selectedItem.GetComponent<ILeftClick>()?.UseLeftClick(EventCenter.Instance().OnUsingItemLeftClick);
        }
    }
    public void PlayerPressR()
    {
        if (selectedItem != null)
        {
            selectedItem.GetComponent<IRPress>()?.UseRPress(EventCenter.Instance().OnUsingItemR);
        }
    }
    public void PlayerPressG()
    {
        if (selectedItem != null)
        {
            selectedItem.GetComponent<IQPress>()?.UseQPress(EventCenter.Instance().OnUsingItemQ);
        }
    }
    public void PlayerPressE()
    {
        if (selectedItem != null)
        {
            selectedItem.GetComponent<IEpress>()?.UseEPress(EventCenter.Instance().OnUsingItemE);
        }
    }
    public void PlayerPressQ()
    {
        if (selectedItem != null)
        {
            selectedItem.GetComponent<IQPress>()?.UseQPress(EventCenter.Instance().OnUsingItemQ);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChangSelectedItemHandle();
        if (selectedItem!=null && !selectedItem.GetComponent<IUsable>().IsUsing())
        {
            EventCenter.Instance().OnUsingNone();
        }
    }

    
    private void SaveData()
    {
        List<ItemsEnum> listIDUsable = new List<ItemsEnum>();
        foreach(GameObject item in listUsableItems)
        {
            //if (item != null)
            //{
            //    Debug.Log("item != null");
            //    if (item.GetComponent<IItem>() != null)
            //    {
            //        Debug.Log("iitem != null");
            //    }
            //}
            //IItem iitem = item.GetComponent<IItem>();
            //ItemsEnum id = iitem.GetItemType();
            //listIDUsable.Add(id);
        }
    }

}
