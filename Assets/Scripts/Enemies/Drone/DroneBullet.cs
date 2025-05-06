using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody bulletRigit;
    [SerializeField] private int damage;
    // Start is called before the first frame update
    void Start()
    {
        bulletRigit = GetComponent<Rigidbody>();
        bulletRigit.velocity = transform.forward * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision != null)
        {
            Debug.Log(collision.collider.name);
            PlayerController playerController;
            playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(damage);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
