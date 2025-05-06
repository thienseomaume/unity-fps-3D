using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldSpace : MonoBehaviour
{
    private static UIWorldSpace instance;
    [SerializeField] GameObject pickIcon;
    public static UIWorldSpace Instance()
    {
        return instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    public void ShowIcon(Vector3 position)
    {
        if(pickIcon.active == false) pickIcon.SetActive(true);
        pickIcon.transform.position = position;
        pickIcon.transform.forward = (position - PlayerInformation.Instance().GetEnvironmentCamera().position).normalized;
    }
    public void HideIcon()
    {
        if(pickIcon.gameObject.active == true)
        pickIcon.SetActive(false);
    }
}
