using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInformation : MonoBehaviour
{
    private static PlayerInformation instance;
    [SerializeField] PlayerController player;
    [SerializeField] private Transform rootTransform;
    [SerializeField] private Transform environmentCamera;
    public event Action<int, int> healthChangeAction;
    public Animator playerAnimator;
    public static PlayerInformation Instance()
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
        DontDestroyOnLoad(player.gameObject);
    }
    public Transform GetTransform()
    {
        return player.transform;
    }
    public void SetPosition(Vector3 position)
    {
        Debug.Log("player position 1 = " + position);
        player.transform.position = position;
        Debug.Log("player position 2 = " + player.transform.position.ToString());
    }
    public void SetActive(bool active)
    {
        player.gameObject.SetActive(active);
    }
    //public void UnlockGun(string gunId)
    //{
    //    PlayerController playerController = GetComponent<PlayerController>();
    //    if (playerController != null)
    //    {
    //        playerController.UnlockGun(gunId);
    //    }
    //}
    public void Heal(int hp)
    {
        if (player != null)
        {
            player.TakeDamage(-hp);
        }
    }
    public void OverrideAnimator(AnimatorOverrideController animatorOverride)
    {
        if(playerAnimator != null)
        {
            playerAnimator.runtimeAnimatorController = animatorOverride;
        }
    }
    public Transform GetEnvironmentCamera()
    {
        return environmentCamera;
    }
    public Transform GetRootTransform()
    {
        return rootTransform;
    }
    public void OnHealthChange(int currentHealth, int maxHealth)
    {
        healthChangeAction?.Invoke(currentHealth, maxHealth);
    }
}
