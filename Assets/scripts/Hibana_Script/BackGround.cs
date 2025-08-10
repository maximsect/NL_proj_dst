using UnityEngine;

public class BackGround : MonoBehaviour
{
    void Update()
    {
        transform.position = Camera.main.gameObject.transform.position * 0.3f;
    }
}