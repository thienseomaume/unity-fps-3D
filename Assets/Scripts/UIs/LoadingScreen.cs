using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour,IScreen
{
    [SerializeField] EnumScreen screenType;
    [SerializeField] public Image loadingProgress;
    public EnumScreen GetScreenType()
    {
        return screenType;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        
    }
}
