using UnityEngine;

public class CatDemonAtt : MonoBehaviour
{
    
    public int attackPower = 10;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerData.main.Damage(attackPower);
        }
    }
}
