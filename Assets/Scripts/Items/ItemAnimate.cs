using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimate : MonoBehaviour
{
    [SerializeField] float speedUpDown;
    [SerializeField] float amplitude;
    [SerializeField] float speedRotate;
    float angel;
    Vector3 rootPosition;
    float t;
    // Start is called before the first frame update

    void Start()
    {
        rootPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        t = Mathf.Repeat(t += speedUpDown * Time.deltaTime, 2 * 3.14f);
        transform.position = rootPosition + Vector3.up * amplitude * Mathf.Sin(t);
        angel += speedRotate * Time.deltaTime;
        angel = Mathf.Repeat(angel, 360);
        transform.rotation = Quaternion.Euler(0, angel, 0);
    }
}
