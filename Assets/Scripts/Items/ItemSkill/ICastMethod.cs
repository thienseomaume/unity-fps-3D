using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ICastMethod
{
    public void Cast(SkillInfor skillInfor, GameObject skillObject, SkillBounder skillBounder,Action useSuccess);
}
