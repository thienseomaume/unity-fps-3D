using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="SkillInfor", fileName ="SkillInfor")]
public class SkillInfor : ScriptableObject
{
    public int baseDamage;
    public float baseCooldown;
    public float baseEffectDuration;
    public float skillRange;
    public float interactionRadius;
    public float speed;
}
