using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MyVector3
{
    public float x, y, z;
    public MyVector3(Vector3 v)
    {
        x = v.x;
        y = v.y;
        z = v.z;
        Debug.Log("(" + x + ";" + y + ";" + z + ")");
    }
    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
    public override string ToString()
    {
        return "(" + x + ";" + y + ";" + z + ")";
    }
}
