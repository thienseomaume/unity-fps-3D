using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth 
{
    public void IncreaseHealth(int amount);
    public void DecreaseHealth(int amount);
}
