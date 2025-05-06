using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionBarSlot : MonoBehaviour
{
    [SerializeField]private Image icon;
    [SerializeField]private Image background;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Select()
    {
        background.color = new Color(1, 1, 1, (float)255/ 255);
    }
    public void DeSelect()
    {
        background.color = new Color(1, 1, 1, (float)24 / 255);
    }
    public void SetItemImage(Sprite sprite)
    {
        icon.sprite = sprite;
        if (sprite != null)
        {
            icon.gameObject.SetActive(true);
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }
}
