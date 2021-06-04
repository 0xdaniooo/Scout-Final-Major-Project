using UnityEngine.UI;
using UnityEngine;

//Creates a tracker on top of enemy
public class ObjectMarker : MonoBehaviour
{
    //References
    public Image img;
    public Image marker;
    public Transform targetLocation;
    public Transform ui_parent;
    private Camera cam;
    private Vector2 pos;

    private void Awake()
    {
        cam = Camera.main;
        targetLocation = transform;
    }

    private void Start()
    {
        marker = Instantiate(img, targetLocation.transform.position, Quaternion.identity);
        marker.transform.SetParent(ui_parent);
    }

    void Update()
    {
        //Ensures indicator displays within UI space
        float minX = marker.GetPixelAdjustedRect().width / 2;
        float maxX = Screen.width - minX;

        float minY = marker.GetPixelAdjustedRect().height / 2;
        float maxY = Screen.height - minY;

        //Prevent missing ref exceptions
        if (targetLocation != null)
        {
            pos = cam.WorldToScreenPoint(targetLocation.position);

            if (Vector3.Dot((targetLocation.position - Camera.main.transform.position), Camera.main.transform.forward) < 0)
            {
                //Target is behind player
                if (pos.x < Screen.width / 2)
                {
                    pos.x = maxX;
                }

                else
                {
                    pos.x = minX;
                }
            }

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            marker.transform.position = pos;
        }
    }

    public void DestroyMarker()
    {
        Destroy(marker);
        Destroy(this);
    }
}
