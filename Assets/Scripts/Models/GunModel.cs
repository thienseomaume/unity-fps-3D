using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunModel 
{
    public ItemsEnum id;
    public int currentMagazine;
    public bool isOwned;

    public GunModel()
    {
    }

    public GunModel(ItemsEnum id, int currentMagazine, bool isOwned)
    {
        this.id = id;
        this.currentMagazine = currentMagazine;
        this.isOwned = isOwned;
    }
}
