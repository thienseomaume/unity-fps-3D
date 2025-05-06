using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : MonoBehaviour, IScreen
{
    [SerializeField]
    public EnumScreen screenType;
    public Button continueButton;
    public Button saveGameButton;
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
