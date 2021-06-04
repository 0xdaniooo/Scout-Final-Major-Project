using UnityEngine;

//Used within daylight cycle for lighting apperarance
[System.Serializable]
[CreateAssetMenu(fileName = "Lighting Preset", menuName ="Scriptables/Lighting Preset", order = 1)]
public class LightingPreset : ScriptableObject
{
    //Variables
    public Gradient AmbientColor;
    public Gradient DirectionalColor;
    public Gradient FogColor;    
}

