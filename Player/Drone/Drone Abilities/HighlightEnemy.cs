using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

//Highlights enemies in range
public class HighlightEnemy : Ability
{
    //Variables
    public float radius;
    public Transform dronePos;
    private int enemiesFound;
    public LayerMask scanLayer;
    public Transform parent;
    public Image enemyMarker;
    public Image img;
    public TextMeshProUGUI textDescription;

    public override void Cast()
    {
        Scan();
        StartCoroutine(Display());
    }

    private void Scan()
    {
        Collider[] colliders = Physics.OverlapSphere(dronePos.transform.position, radius, scanLayer);

        foreach (Collider hit in colliders)
        {
            EnemyAI enemy = hit.GetComponent<EnemyAI>();

            if (enemy != null)
            {
                ObjectMarker om = enemy.gameObject.AddComponent(typeof(ObjectMarker)) as ObjectMarker;
                om.img = enemyMarker;
                om.ui_parent = parent;

                enemiesFound ++;
            }
        }
    }

    private IEnumerator Display()
    {
        textDescription.SetText(AbilityName + " activated, found " + enemiesFound + " enemies");
        textDescription.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        textDescription.gameObject.SetActive(false);

        img.color = new Color32(255, 0, 0, 255);
        yield return new WaitForSeconds(AbilityCooldown);
        img.color = new Color32(0, 255, 40, 255);

        enemiesFound = 0;
    }
}
