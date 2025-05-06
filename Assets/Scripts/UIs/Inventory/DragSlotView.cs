using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlotView : MonoBehaviour
{
    public Image imageItem;
    [SerializeField] private Canvas canvas;
    private Camera uiCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }
    private void OnEnable()
    {
        UpdatePosition();
    }
    void UpdatePosition()
    {
        Vector2 localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, Input.mousePosition, null, out localPosition);
        transform.localPosition = localPosition;
    }
}
