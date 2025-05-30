using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="SkillInfor", fileName ="SkillInfor")]
public class SkillInfor : ScriptableObject
{
    public LayerMask interactionLayerCast;
    public LayerMask interactionLayerSkill;
    public int baseDamage;
    public float baseCooldown;
    public float timeUsing;
    public float baseEffectDuration;
    public float skillRange;
    public float interactionRadius;
    public float speed;
}
