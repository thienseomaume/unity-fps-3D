using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HumanoidBulletPool : MonoBehaviour
{
    private static HumanoidBulletPool instance;
    public Queue<HumanoidBulletScript> bullets = new Queue<HumanoidBulletScript>();
    public GameObject bulletPrefab;

    public static HumanoidBulletPool Instance()
    {
        return instance;
    }
    public HumanoidBulletScript GetBullet()
    {
        return bullets.Dequeue();
    }
    public void ReturnPool(HumanoidBulletScript bullet)
    {
        bullets.Enqueue(bullet);
    }
    public void ScalePool()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject bulletObject = GameObject.Instantiate(bulletPrefab);
            bulletObject.transform.parent = this.transform;
            HumanoidBulletScript bullet = bulletObject.GetComponent<HumanoidBulletScript>();
            bullets.Enqueue(bullet);
            bullet.pool = this;
        }
    }
    void Awake()
    {
        instance = this;
        for (int i = 0; i < 20; i++)
        {
            GameObject bulletObject = GameObject.Instantiate(bulletPrefab);
            bulletObject.transform.parent = this.transform;
            HumanoidBulletScript bullet = bulletObject.GetComponent<HumanoidBulletScript>();
            bullets.Enqueue(bullet);
            bullet.pool = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
