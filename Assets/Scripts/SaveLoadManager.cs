using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class SaveLoadManager
{
    public void SaveData(SaveDataModel saveDataModel, string path)
    {
        string json = JsonConvert.SerializeObject(saveDataModel,Formatting.Indented);
        File.WriteAllText(path, json);
    }
    public SaveDataModel LoadData(string path)
    {
        string json = File.ReadAllText(path);
        SaveDataModel saveData = JsonConvert.DeserializeObject<SaveDataModel>(json);
        Debug.Log("player position loaded = " + saveData.playerPosition.ToString());
        return saveData;
    }
    public void SaveSetting(SettingModel setting, string path)
    {
        string json = JsonConvert.SerializeObject(setting, Formatting.Indented);
        Debug.Log(path);
        File.WriteAllText(path, json);
    }
    public SettingModel LoadSetting(string path)
    {
        SettingModel saveSetting;
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveSetting = JsonConvert.DeserializeObject<SettingModel>(json);
        }
        else
        {
            saveSetting = new SettingModel(1, 1);
            SaveSetting(saveSetting, path);
        }
        return saveSetting;
    }
}
