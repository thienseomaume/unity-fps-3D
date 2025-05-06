using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

[System.Serializable]
public class SaveDataModel
{
    public string levelName;
    public int currentHealth;
    public MyVector3 playerPosition;
    public Dictionary<ItemsEnum, int> listItem = new Dictionary<ItemsEnum, int>();
    public Dictionary<ItemsEnum, GunModel> listGun = new Dictionary<ItemsEnum, GunModel>();
    [JsonConverter(typeof(MyEnumListConverter<ItemsEnum>))]
    public List<ItemsEnum> listIdSelectionBar = new List<ItemsEnum>();
}
