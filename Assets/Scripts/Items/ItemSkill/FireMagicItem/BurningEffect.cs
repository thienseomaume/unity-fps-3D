using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurningEffect : IEffect
{
    EffectManager effectManager;
    int burningDamage;
    float startTime;
    float endTime;
    public void EndEffect(GameObject gameObject)
    {
        
    }

    public void StartEffect(GameObject gameObject, SkillInfor skillInfor, EffectManager effectManager)
    {
        this.effectManager = effectManager;
        burningDamage = Mathf.CeilToInt(skillInfor.baseDamage / 10.0f);
        startTime = Time.time;
        endTime = startTime + skillInfor.baseEffectDuration;
    }

    public void UpdateEffect(GameObject gameObject)
    {
        gameObject.GetComponent<IHealth>().DecreaseHealth(burningDamage);
        if(Time.time >= endTime)
        {
            effectManager.RemoveEffect(this);
        }
    }
}
