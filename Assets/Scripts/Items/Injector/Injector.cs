using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injector : MonoBehaviour, IUsable, IItem,ILeftClick
{

    [SerializeField] float healingTime;
    [SerializeField] int hpHealing;
    float healingTimer;
    private bool isHealing;
    private Animator animator;
    [SerializeField] private AnimatorOverrideController armInjectorAnimatorOverride;
    [SerializeField]private ItemsEnum itemType;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(healingTimer >= 0)
        {
            healingTimer -= Time.deltaTime;
        }
        if(healingTimer <= 0 && isHealing)
        {
            isHealing = false;
            animator.Play(AnimationData.ITEM_DEFAULT);
            Heal();
        }
    }
    public bool IsHealing()
    {
        return isHealing;
    }
    public void Heal()
    {
        InventoryController.Instance().RemoveItem(1, ItemsEnum.health);
        PlayerInformation.Instance().Heal(hpHealing);
    }
    public void SetHpHealing(int hp)
    {
        hpHealing = hp;
    }

    public bool Use()
    {
        isHealing = true;
        healingTimer = healingTime;
        animator.Play(AnimationData.ITEM_LEFT_CLICK);
        return true;
    }

    public bool IsUsing()
    {
        return isHealing;
    }

    public AnimatorOverrideController GetAnimatorOverride()
    {
        return armInjectorAnimatorOverride;
    }

    public ItemsEnum GetItemType()
    {
        return itemType;
    }

    public void UseLeftClick(Action actionUseSuccess)
    {
        isHealing = true;
        healingTimer = healingTime;
        animator.Play(AnimationData.ITEM_LEFT_CLICK);
        actionUseSuccess();
    }

    public void SetUp()
    {

    }
}
