using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScreen
{
    public EnumScreen GetScreenType();
    public void Initialize();
}
