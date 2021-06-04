using UnityEngine;

//Script used for removing bullet holes
public class BulletHole : MonoBehaviour
{
    private void Update()
    {
        Destroy(gameObject, 10f);
    }
}
