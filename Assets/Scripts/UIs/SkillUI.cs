using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image progressCooldown;
    public void SetProgress(float ratio)
    {
        if (!gameObject.active)
        {
            gameObject.active = true;
        }
        if (ratio<=1.0f)
        {
            if (!progressCooldown.gameObject.active)
            {
                progressCooldown.gameObject.SetActive(true);
            }
            progressCooldown.fillAmount = ratio;
        }
        else
        {

            progressCooldown.gameObject.SetActive(false);
        }
    }
}
