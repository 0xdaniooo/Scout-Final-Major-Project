using UnityEngine;

//Daylight cycle script which manages time and lighting
[ExecuteAlways]
public class LightingManager : MonoBehaviour
{
    //Variables
    [Range(0, 24)] public float TimeOfDay;
    [Range(0, 10)] public float timeMultiplier;

    //References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;

    private void Update()
    {
        if (Preset == null)
        {
            return;
        }

        if (Application.isPlaying)
        {
            TimeOfDay += Time.deltaTime * timeMultiplier;

            //Clamp between 0-24
            TimeOfDay %= 24;
            UpdateLighting(TimeOfDay / 24f);
        }

        else
        {
            UpdateLighting(TimeOfDay / 24f);
        }
    }

    private void UpdateLighting(float timePercent)
    {
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(timePercent);
        RenderSettings.fogColor = Preset.FogColor.Evaluate(timePercent);

        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(timePercent);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((timePercent * 360f) - 90f, 170f, 0));
        }
    }

    //Try to find a directinoal light to use if we havent set one
    private void OnValidate()
    {
        if (DirectionalLight != null)
        {
            return;
        }

        //Search for lighting tab sun
        if (RenderSettings.sun != null)
        {
            DirectionalLight = RenderSettings.sun;
        }

        //Search for lighting that fits criteria (directional)
        else
        {
            Light[] lights = GameObject.FindObjectsOfType<Light>();

            foreach (Light light in lights)
            {
                if (light.type == LightType.Directional)
                {
                    DirectionalLight = light;
                    return;
                }
            }
        }
    }
}