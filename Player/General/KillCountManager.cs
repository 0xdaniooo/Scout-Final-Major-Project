using UnityEngine;
using TMPro;

//Modifies and mnaages the kill count
public class KillCountManager : MonoBehaviour
{
    public TextMeshProUGUI killText;
    public int kills;

    public static KillCountManager KillManager;

    private void Awake()
    {
        if (KillManager == null)
        {
            KillManager = this;
        }

        else if (KillManager != this)
        {
            Destroy(gameObject);
        }
    }

    public void KillCount(int killsToParse)
    {
        kills += killsToParse;

        killText.SetText("Kills: " + kills.ToString());
    }
}
