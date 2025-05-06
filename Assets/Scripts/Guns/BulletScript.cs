using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject gun;
    Vector3 startPos;
    Vector3 endPos;
    float velocity=200;
    float time;
    float timer;
    void Start()
    {
        
    }
    void Update()
    {
        if (timer < time)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(startPos, endPos, Mathf.Clamp01(timer / time));
        }
        else
        {
            timer = 0;
            time = 0;
            gun.GetComponent<BaseGun>().ReturnBulletPool(this.gameObject);
            gameObject.SetActive(false);
        }
    }
    public void setBegin(Vector3 start, Vector3 end)
    {
        startPos = start;
        endPos = end;
        timer = 0f;
        float distance = (endPos - startPos).magnitude;
        time = distance / velocity;
        if (time > 0.05f)
        {
            time = 0.05f;
        }
        transform.position = Vector3.Lerp(startPos, endPos, timer / time);
    }
}
