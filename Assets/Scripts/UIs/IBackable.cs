using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBackable
{
    public EnumScreen GetBackScreen();
    public void SetBackScreen(EnumScreen backScreen);
}
