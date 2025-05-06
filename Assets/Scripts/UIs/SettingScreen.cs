using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingScreen : MonoBehaviour, IScreen,IBackable
{
    [SerializeField]
    public EnumScreen screenType;
    private EnumScreen backScreen;
    public Button saveSettingButton;
    public Button closeSettingButton;

    public Slider soundFXSlider;
    public Slider musicSlider;
    public EnumScreen GetBackScreen()
    {
        return backScreen;
    }

    public void SetBackScreen(EnumScreen backScreen)
    {
        this.backScreen = backScreen;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    EnumScreen IScreen.GetScreenType()
    {
        return screenType;
    }

    public void Initialize()
    {
        
    }
}
