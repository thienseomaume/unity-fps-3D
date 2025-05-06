using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class ScreenManager : MonoBehaviour
{
    // Start is called before the first frame update
    Dictionary<EnumScreen, GameObject> listScreen;
    private EnumScreen currentScreen;
    private void Awake()
    {
        if (FindObjectsOfType<ScreenManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
        Debug.Log("screen manager awake call");
    }
    private void OnEnable()
    {

    }
    void Start()
    {
        Debug.Log("screen manager start call");
        listScreen = new Dictionary<EnumScreen, GameObject>();
        currentScreen = EnumScreen.StartScreen;
        foreach (Transform transform in transform)
        {
            GameObject screenObject = transform.gameObject;
            if (screenObject.GetComponent<IScreen>() == null)
            {
                break;
            }
            EnumScreen screen = screenObject.GetComponent<IScreen>().GetScreenType();
            screenObject.GetComponent<IScreen>().Initialize();
            listScreen.Add(screen, screenObject);
        }
        foreach (KeyValuePair<EnumScreen, GameObject> screenObject in listScreen)
        {
            if (screenObject.Key == currentScreen)
            {
                screenObject.Value.SetActive(true);
            }
            else
            {
                screenObject.Value.SetActive(false);
            }
        }
        SetStartScreen();
        SetSettingScreen();
        SetPauseScreen();
        SetLoadingScreen();
        SetHudScreen();
        SetDiedScreen();
        SetInventoryScreen();
        EventCenter.Instance().changeSceneAction += (string sceneName) =>
        {
            if (sceneName != "StartScene")
            {
                ChangeScreen(EnumScreen.HudScreen);
            }
        };
    }
    void ChangeScreen(EnumScreen screenToChange)
    {
        if (screenToChange != currentScreen && listScreen.ContainsKey(screenToChange))
        {
            if (listScreen[screenToChange].GetComponent<IBackable>() != null)
            {
                listScreen[screenToChange].GetComponent<IBackable>().SetBackScreen(currentScreen);
            }
            listScreen[currentScreen].SetActive(false);
            listScreen[screenToChange].SetActive(true);
            currentScreen = screenToChange;
        }
    }
    void SetLoadingScreen()
    {
        LoadingScreen loadingScreen = listScreen[EnumScreen.LoadingScreen].GetComponent<LoadingScreen>();
        if (loadingScreen != null)
        {
            EventCenter.Instance().loadSceneAction += (float progress) =>
            {
                if (progress != 1.0f)
                {
                    ChangeScreen(EnumScreen.LoadingScreen);
                }
                else
                {
                    ChangeScreen(EnumScreen.HudScreen);
                }
                loadingScreen.loadingProgress.fillAmount = progress;
            };
        }
    }
    void SetStartScreen()
    {
        StartScreen startScreen = listScreen[EnumScreen.StartScreen].GetComponent<StartScreen>();
        if (startScreen != null)
        {
            startScreen.newGameButton.onClick.AddListener(()=>
            {
                GameManager.Instance().LoadNewGame();
            });
            startScreen.settingButton.onClick.AddListener(()=>
            {
                ChangeScreen(EnumScreen.SettingScreen);
            });
            startScreen.continueButton.onClick.AddListener(GameManager.Instance().LoadContinueGame);
            startScreen.exitButton.onClick.AddListener(ExitGame);
        }
    }
    void SetSettingScreen()
    {
        SettingScreen settingScreen = listScreen[EnumScreen.SettingScreen].GetComponent<SettingScreen>();
        if (settingScreen != null)
        {
            SettingModel mySetting = GameManager.Instance().GetSetting();
            settingScreen.soundFXSlider.value = mySetting.soundFXVolume;
            settingScreen.musicSlider.value = mySetting.musicVolume;
            settingScreen.closeSettingButton.onClick.AddListener(() =>
            {
                ChangeScreen(settingScreen.GetComponent<IBackable>().GetBackScreen());
            });
            settingScreen.soundFXSlider.onValueChanged.AddListener((float value) =>
            {
                mySetting.soundFXVolume = value;
            });
            settingScreen.musicSlider.onValueChanged.AddListener((float value) =>
            {
                mySetting.musicVolume = value;
            });
            settingScreen.saveSettingButton.onClick.AddListener(() =>
            {
                GameManager.Instance().SaveSetting();
            });
        }
    }

    void SetDiedScreen()
    {
        DiedScreen diedScreen = listScreen[EnumScreen.DiedScreen].GetComponent<DiedScreen>();
        if (diedScreen != null)
        {
            diedScreen.playAgainButton.onClick.AddListener(GameManager.Instance().PlayAgain);
            diedScreen.exitButton.onClick.AddListener(ExitGame);
            EventCenter.Instance().healthChangeAction += (int currentHealth, int maxHealth) =>
            {
                if (currentHealth == 0)
                {
                    ChangeScreen(EnumScreen.DiedScreen);
                    GameManager.Instance().Pause();
                }
            };
        }
    }
    void SetPauseScreen()
    {
        PauseScreen pauseScreen = listScreen[EnumScreen.PauseScreen].GetComponent<PauseScreen>();
        if (pauseScreen != null)
        {
            pauseScreen.continueButton.onClick.AddListener(() =>
            {
                ChangeScreen(EnumScreen.HudScreen);
                GameManager.Instance().Resume();
            });
            pauseScreen.saveGameButton.onClick.AddListener(() => { });
            pauseScreen.settingButton.onClick.AddListener(()=>{
                ChangeScreen(EnumScreen.SettingScreen);
            });
            pauseScreen.exitButton.onClick.AddListener(ExitGame);
        }
    }
    void SetHudScreen()
    {

        HudScreen hudScreen = listScreen[EnumScreen.HudScreen].GetComponent<HudScreen>();
        if (hudScreen != null)
        {
            EventCenter.Instance().healthChangeAction += (int current, int max) =>
            {
                float ratio = (float)current / (float)max;
                hudScreen.healthBar.fillAmount = ratio;
            };
            if (SelectionBar.Instance() == null)
            {
                Debug.Log("instance of selection bar is null");
            }
            EventCenter.Instance().magazineAction += (int magazine) =>
            {
                hudScreen.txtMagazine.text = magazine.ToString();
            };
            hudScreen.inventoryButton.onClick.AddListener(() =>
            {
                ChangeScreen(EnumScreen.InventoryScreen);
            });
            hudScreen.buttonPause.onClick.AddListener(() =>
            {
                ChangeScreen(EnumScreen.PauseScreen);
                GameManager.Instance().Pause();
            });
            EventCenter.Instance().onChangeSelectedItem += (GameObject selectedItem) =>
            {
                if(selectedItem != null)
                {
                    if (selectedItem.GetComponent<BaseGun>() == null)
                    {
                        hudScreen.magazineUi.SetActive(false);
                    }
                    else
                    {
                        hudScreen.magazineUi.SetActive(true);
                        EventCenter.Instance().OnMagazineChange(selectedItem.GetComponent<BaseGun>().GetMagazine());
                    }
                    hudScreen.HideAllSkillUI();
                }
                else
                {
                    hudScreen.magazineUi.SetActive(false);
                    hudScreen.HideAllSkillUI();
                }
            };
            EventCenter.Instance().leftClickCooldown += hudScreen.LeftClick.SetProgress;
            EventCenter.Instance().eCooldown += hudScreen.ESkill.SetProgress;
            EventCenter.Instance().qCooldown += hudScreen.QSkill.SetProgress;
            EventCenter.Instance().rCooldown += hudScreen.RSkill.SetProgress;
        }
    }
    void SetInventoryScreen()
    {
        InventoryView inventoryScreen = listScreen[EnumScreen.InventoryScreen].GetComponent<InventoryView>();
        if (inventoryScreen != null)
        {
            inventoryScreen.closeButton.onClick.AddListener(() =>
            {
                ChangeScreen(EnumScreen.HudScreen);
            });
            EventCenter.Instance().inventoryUiToggle += (bool isActive) =>
            {
                if (isActive)
                {
                    ChangeScreen(EnumScreen.HudScreen);
                    inventoryScreen.closeButton.GetComponent<Button>().onClick.Invoke();
                    Cursor.lockState = CursorLockMode.Locked;
                }
                else
                {
                    Debug.Log("checked open inventory");
                    ChangeScreen(EnumScreen.InventoryScreen);
                    Cursor.lockState = CursorLockMode.None;
                }
            };
        }
    }
    void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
