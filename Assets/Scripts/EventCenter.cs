using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventCenter : MonoBehaviour
{
    private static EventCenter instance;
    public event Action saveAction;
    public event Action<string> changeSceneAction;
    public event Action<float> loadSceneAction;
    public event Action<int, int> healthChangeAction;
    public event Action onUsingItemLeftClick, onUsingItemR, onUsingItemE, onUsingItemQ, onUsingNone;
    public event Action<SlotView> onPointerClick, onBeginDrag, onEndDrag, onDrop;
    public event Action<float> leftClickCooldown, qCooldown, eCooldown, rCooldown;
    public event Action<int> magazineAction;
    public event Action<bool> inventoryUiToggle;
    public event Action<GameObject> onChangeSelectedItem;
    public static EventCenter Instance()
    {
        return instance;
    }
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnHealthChange(int currentHealth, int maxHealth)
    {
        healthChangeAction?.Invoke(currentHealth, maxHealth);
    }
    public void OnChangeScene(string sceneName)
    {
        changeSceneAction?.Invoke(sceneName);
    }
    public void OnLoadScene(float progress)
    {
        loadSceneAction?.Invoke(progress);
    }
    public void OnUsingItemLeftClick()
    {
        onUsingItemLeftClick?.Invoke();
    }
    public void OnUsingItemR()
    {
        onUsingItemR?.Invoke();
    }
    public void OnUsingItemE()
    {
        onUsingItemE?.Invoke();
    }
    public void OnUsingItemQ()
    {
        onUsingItemQ?.Invoke();
    }
    public void OnUsingNone()
    {
        onUsingNone?.Invoke();
    }
    public void OnSlotClick(SlotView slotView)
    {
        onPointerClick?.Invoke(slotView);
    }
    public void OnSlotBeginDrag(SlotView slotView)
    {
        onBeginDrag?.Invoke(slotView);
    }
    public void OnSlotEndDrag(SlotView slotView)
    {
        onEndDrag?.Invoke(slotView);
    }
    public void OnSlotDrop(SlotView slotView)
    {
        onDrop?.Invoke(slotView);
    }
    public void OnMagazineChange(int magazine)
    {
        magazineAction?.Invoke(magazine);
    }
    public void OnToggleInventoryUi(bool isActive)
    {
        inventoryUiToggle?.Invoke(isActive);
    }
    public void OnLeftClickCooldown(float ratio)
    {
        leftClickCooldown?.Invoke(ratio);
    }
    public void OnECooldown(float ratio)
    {
        eCooldown?.Invoke(ratio);
    }
    public void OnQCooldown(float ratio)
    {
        qCooldown?.Invoke(ratio);
    }
    public void OnRCooldown(float ratio)
    {
        rCooldown?.Invoke(ratio);
    }
    public void OnChangeSelectedItem(GameObject selectedItem)
    {
        onChangeSelectedItem?.Invoke(selectedItem);
    }
}
