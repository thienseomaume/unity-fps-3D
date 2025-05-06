using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjecttileCast : ICastMethod
{
    public void Cast(SkillInfor skillInfor, GameObject skillObject, SkillBounder skillBounder,Action useSuccess)
    {
        RaycastHit hit;
        Vector3 directionCam = PlayerInformation.Instance().GetEnvironmentCamera().forward;
        Vector3 positionCam = PlayerInformation.Instance().GetEnvironmentCamera().position;
        Vector3 point;
        Physics.Raycast(positionCam, directionCam, out hit, Mathf.Infinity);
        if(hit.collider != null)
        {
            point = hit.point;
        }
        else
        {
            point = positionCam + directionCam * 999;
        }
        Quaternion rotation = Quaternion.LookRotation(point - skillBounder.rightHand.position);
        skillBounder.SpawnSkill(skillBounder.rightHand.position, rotation);
        useSuccess?.Invoke();
    }
}
