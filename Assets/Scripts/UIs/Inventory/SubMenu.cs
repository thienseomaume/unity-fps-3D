using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubMenu : MonoBehaviour
{
    public Button useButton;
    public Button dropButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ShowAll()
    {
        gameObject.SetActive(true);

    }
    public void ShowExceptUseButton()
    {
        useButton.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        useButton.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
