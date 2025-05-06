using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBounder : MonoBehaviour
{
    [SerializeField] private GameObject skillObject;
    [SerializeField] private SkillInfor skillInfor;
    [SerializeField] private LayerMask interactionMask;
    public Transform rightHand;
    public Transform leftHand;
    private ICastMethod castMethod;
    private Action<float> cooldownAction;
    private float lastTimeUse;
    private float ratio=1.0f;
    public void Active(Action actionUseSuccess)
    {
        if (IsReady())
        {
            castMethod.Cast(skillInfor, skillObject, this, actionUseSuccess);
        }
    }

    public bool IsReady()
    {
        if((Time.time - lastTimeUse) >= skillInfor.baseCooldown)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Setup(Action<float> cooldownAction, ICastMethod castMethod)
    {
        this.cooldownAction = cooldownAction;
        this.castMethod = castMethod;
    }
    public void SpawnSkill(Vector3 position, Quaternion rotation)
    {
        Debug.Log("checkedddddd");
        if (skillObject == null) return;
        GameObject skillObjectSpawned = Instantiate(skillObject);
        ISkill skill = skillObjectSpawned.GetComponent<ISkill>();
        if(skill != null)
        {
            lastTimeUse = Time.time;
            skill.Initialize(position, rotation, skillInfor,interactionMask);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ratio = (Time.time - lastTimeUse) / skillInfor.baseCooldown;
        if (ratio <= 1.0f)
        {
            cooldownAction?.Invoke(ratio);
        }
    }
    private void OnEnable()
    {
        cooldownAction?.Invoke(ratio);
    }
}
