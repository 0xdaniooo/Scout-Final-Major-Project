using UnityEngine;

[System.Serializable]

[CreateAssetMenu(fileName = "PresetPath", menuName = "Scriptables/Preset Path", order = 2)]
public class PresetPath : ScriptableObject
{
    public Transform[] points;
}
