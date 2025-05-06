using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DiedScreen : MonoBehaviour, IScreen
{
    [SerializeField]
    public EnumScreen screenType;
    public Button playAgainButton;
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