using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    SettingModel mySetting;
    SaveLoadManager saveLoadManager;
    public SaveDataModel saveData;
    string defaultDataPath;
    string saveDataPath;
    string saveSettingPath;
    public event Action saveAction;
    public event Action<string> changeSceneAction;
    public event Action<float> loadSceneAction;
    string _currentScene;
    bool isPaused;
    string currentScene
    {
        get
        {
            return _currentScene;
        }
        set
        {
            _currentScene = value;
            //changeSceneAction?.Invoke(currentScene);
            EventCenter.Instance().OnChangeScene(currentScene);
        }
    }

    public static GameManager Instance()
    {
        return instance;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        defaultDataPath = Application.streamingAssetsPath + "/DefaultData.json";
        saveDataPath = Application.persistentDataPath + "/SaveData.json";
        saveSettingPath = Application.persistentDataPath + "/SaveSetting.json";
        saveLoadManager = new SaveLoadManager();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Debug.Log(saveSettingPath);
        mySetting = saveLoadManager.LoadSetting(saveSettingPath);
        currentScene = "StartScene";
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }
    public void LoadScene(int index)
    {

    }
    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;
        float fakeProgress = 0;
        while (!asyncOperation.isDone)
        {
           EventCenter.Instance().OnLoadScene(fakeProgress);
            fakeProgress = Mathf.Clamp(fakeProgress + Time.deltaTime, 0, 0.9f);
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }

        currentScene = sceneName;
        EventCenter.Instance().OnLoadScene(1.0f);
        Debug.Log("player position to set = " + saveData.playerPosition.ToVector3().ToString());
        PlayerInformation.Instance().SetPosition(saveData.playerPosition.ToVector3());
        Debug.Log("player position after setup = " + PlayerInformation.Instance().GetTransform().position.ToString());
        PlayerInformation.Instance().SetActive(true);
    }
    public void LoadContinueGame()
    {
        Debug.Log("load continue game");
        LoadFileToModel(saveDataPath);
        InventoryController.Instance().Initialize();
        SelectionBar.Instance().Initialize();
        LoadScene(saveData.levelName);
    }
    public void LoadNewGame()
    {
        LoadFileToModel(defaultDataPath);
        saveLoadManager.SaveData(saveData, saveDataPath);
        Debug.Log("1 player position to set = " + saveData.playerPosition.ToVector3().ToString());
        InventoryController.Instance().Initialize();
        SelectionBar.Instance().Initialize();
        Debug.Log("load new game");
        LoadScene(saveData.levelName);
    }
    public void LoadAgain()
    {
        LoadFileToModel(saveDataPath);
        InventoryController.Instance().Reload();
        SelectionBar.Instance().Reload();
        LoadScene(saveData.levelName);
    }
    public void NextLevel(string nextSceneName, Vector3 nextPosition)
    {
        Debug.Log("next to " + nextSceneName);
        saveAction?.Invoke();
        saveData.levelName = nextSceneName;
        //saveAction = null;
        saveLoadManager.SaveData(saveData, saveDataPath);
        PlayerInformation.Instance().transform.position = nextPosition;
        PlayerInformation.Instance().gameObject.SetActive(true);
        LoadScene(nextSceneName);
    }
    void LoadFileToModel(string path)
    {
        if (File.Exists(path))
        {
            saveData = saveLoadManager.LoadData(path);
        }
        else
        {
            saveData = saveLoadManager.LoadData(defaultDataPath);
        }
    }
    public void PlayAgain()
    {
       // saveAction = null;
        Resume();
        //LoadScene(currentScene);
        LoadAgain();
    }
    public SettingModel GetSetting()
    {
        return mySetting;
    }
    public void SaveSetting()
    {
        saveLoadManager.SaveSetting(mySetting, saveSettingPath);
    }
    public void SaveFileDefault()
    {
        saveAction.Invoke();
        saveData.levelName = SceneManager.GetActiveScene().name;
        Debug.Log("save in scene = " + SceneManager.GetActiveScene().name);
        saveLoadManager.SaveData(saveData, defaultDataPath);
    }
    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
    }
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
    }
}
