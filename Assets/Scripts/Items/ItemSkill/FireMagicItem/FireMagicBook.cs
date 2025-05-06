using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMagicBook : MonoBehaviour, IItem, IUsable, ILeftClick, IEpress
{
    [SerializeField] private ItemsEnum itemType;
    [SerializeField] private AnimatorOverrideController animatorOverride;
    [SerializeField] private SkillBounder leftClickSkill;

    public AnimatorOverrideController GetAnimatorOverride()
    {
        return animatorOverride;
    }

    public ItemsEnum GetItemType()
    {
        return itemType;
    }

    public bool IsUsing()
    {
        return true;
    }

    public void SetUp()
    {
        leftClickSkill.Setup(EventCenter.Instance().OnLeftClickCooldown, new ProjecttileCast());
    }

    public void UseEPress(Action actionUseSuccess)
    {
        
    }

    public void UseLeftClick(Action actionUseSuccess)
    {
        leftClickSkill.Active(actionUseSuccess);
    }
    private void Awake()
    {
        SetUp();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
