using UnityEngine;

public class BackGround : MonoBehaviour
{

    public float speeds = 0.3f;
    void Update()
    {
        transform.position = Camera.main.gameObject.transform.position * speeds;
    }
}