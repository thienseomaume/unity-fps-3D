using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlickering : MonoBehaviour
{
    private Light lightComponent;
    public float minIntensity;
    public float maxIntensity;
    public float flickerSpeed;
    // Start is called before the first frame update
    void Start()
    {
        lightComponent = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        if (lightComponent != null)
        {
            lightComponent.intensity = Mathf.Lerp(minIntensity, maxIntensity, Mathf.PerlinNoise(flickerSpeed * Time.time, 0.5f));
        }
    }
}
