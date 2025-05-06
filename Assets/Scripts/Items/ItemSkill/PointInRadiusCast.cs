using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PointInRadiusCast : ICastMethod
{
    RaycastHit hit;
    Vector3 direction;
    Vector3 rootcast;
    LayerMask interactionLayer;
    float radius;
    public void Cast(SkillInfor skillInfor, GameObject skillObject, SkillBounder skillBounder, Action useSccess)
    {
        rootcast = PlayerInformation.Instance().GetEnvironmentCamera().position;
        direction = PlayerInformation.Instance().GetEnvironmentCamera().forward;
        interactionLayer = LayerMask.GetMask("Ground");
        radius = skillInfor.skillRange;
        if(Physics.Raycast(rootcast,direction,out hit, radius, interactionLayer))
        {
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward);
            skillBounder.SpawnSkill(hit.point, rotation);
            useSccess?.Invoke();
        }
    }
}
