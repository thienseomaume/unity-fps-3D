using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffect
{
    public void StartEffect(GameObject gameObject, SkillInfor skillInfor, EffectManager effectManager);
    public void UpdateEffect(GameObject gameObject);
    public void EndEffect(GameObject gameObject);
}
