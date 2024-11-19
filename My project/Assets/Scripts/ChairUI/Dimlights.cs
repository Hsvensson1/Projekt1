using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    [Header("References")]
    public List<Light> lights;
    public List<MeshRenderer> lightMeshes;

    [Header("Material")]
    public Material lightMaterial;

    [Header("Effect")]
    public float fadeDuration = 1f;

    //Private Variables
    private bool areLightsOn = true;
    private Material lightMaterialInstance;
    private float oldIntensity;

    void Start()
    {
        lightMaterialInstance = Material.Instantiate(lightMaterial);
        foreach(MeshRenderer lightMesh in lightMeshes)
        {
            lightMesh.gameObject.GetComponent<MeshRenderer>().material = lightMaterialInstance;
        }


        oldIntensity = lights[0].intensity;
    }

    public void ToggleLights()
    {
        if (areLightsOn)
        {
            StartCoroutine(DimLightsOff());
        }
        else
        {
            StartCoroutine(DimLightsOn());
        }

        areLightsOn = !areLightsOn;
    }

    private IEnumerator DimLightsOff()
    {
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            float intensity = Mathf.Lerp(oldIntensity, 0, timeElapsed / fadeDuration);

            lightMaterialInstance.SetFloat("_Light_Intensity", 1f - timeElapsed / fadeDuration);

            foreach (Light light in lights)
            {
                if (light != null)
                {
                    light.intensity = intensity;
                }
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator DimLightsOn()
    {
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            float intensity = Mathf.Lerp(0, oldIntensity, timeElapsed / fadeDuration);

            lightMaterialInstance.SetFloat("_Light_Intensity", timeElapsed / fadeDuration);

            foreach (Light light in lights)
            {
                if (light != null)
                {
                    light.intensity = intensity;
                }
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}
