using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : MonoBehaviour, IScreen
{
    [SerializeField]
    public EnumScreen screenType;
    public Button newGameButton;
    public Button continueButton;
    public Button settingButton;
    public Button exitButton;
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
