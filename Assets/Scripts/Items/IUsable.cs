using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public interface IUsable
{
    public bool IsUsing();
    public AnimatorOverrideController GetAnimatorOverride();
    public void SetUp();
}
