using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    private List<IEffect> listEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ApplyEffect(IEffect effect, SkillInfor skillInfor)
    {
        effect.StartEffect(this.gameObject,skillInfor,this);
        listEffect.Add(effect);
    }
    public void UpdateEffect()
    {
        foreach(IEffect effect in listEffect)
        {
            effect.UpdateEffect(this.gameObject);
        }
    }
    public void RemoveEffect<T>()
    {
        IEffect effect = listEffect.Find(e => e is T);
        if(effect != null)
        {
            effect.EndEffect(this.gameObject);
            listEffect.Remove(effect);
        }
    }
    public void RemoveEffect(IEffect effect)
    {
        if (effect != null)
        {
            effect.EndEffect(this.gameObject);
            listEffect.Remove(effect);
        }
    }
}
