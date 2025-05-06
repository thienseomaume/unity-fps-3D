using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISkill
{
    public void Initialize(Vector3 position, Quaternion rotation, SkillInfor skillInfor, LayerMask interactionLayer);
}
