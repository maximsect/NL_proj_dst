using UnityEngine;

public class BackGround : MonoBehaviour
{
    void Update()
    {
        transform.position = GameManager.main.camera.transform.position * 0.3f;
    }
}