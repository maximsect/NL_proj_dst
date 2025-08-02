using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EyeObjectRandomMovement : MonoBehaviour
{
    public GameObject player, oniProfessor;
    public Vector2 basePoint = new Vector2(0, 0);
    public Vector2 boxSize = new Vector2(10, 10);
    public float movementSpeed = 5f, waitingDuration = 3f;
    public GameObject LaserPref;

    private int movementStage = 0;
    private bool isEnemy = true;
    public float returnEnemyInterval = 10f;
    private float enemyCounter = 0;
    public SpriteRenderer outliner,mainSprite;
    public GameObject observer;
    public Sprite[] eyes;
    Coroutine coroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coroutine = StartCoroutine(RandomMovement());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isEnemy)
        {
            outliner.color = Color.green;
            enemyCounter += Time.deltaTime;
            if(enemyCounter > returnEnemyInterval)
            {
                enemyCounter = 0;
                isEnemy = true;
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(RandomMovement());
                outliner.color = Color.red;
            }
        }
        observer.SetActive(!isEnemy);
        if (oniProfessor == null) isEnemy = true;
        Vector3 relativePos = (isEnemy) ? player.transform.position - transform.position : oniProfessor.transform.position - transform.position;
        float angle = Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg + 360;
        int eyeIndex = (int)((angle + 15) / 30) % 12;
        mainSprite.sprite = eyes[eyeIndex];
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(basePoint, boxSize);
    }
    Vector2 RandomPos()
    {
        return basePoint + new Vector2(UnityEngine.Random.Range(-boxSize.x / 2, boxSize.x / 2), UnityEngine.Random.Range(-boxSize.y / 2, boxSize.y / 2));
    }
    IEnumerator RandomMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitingDuration);
            Vector2 generatedPoint;
            while (true)
            {
                generatedPoint = RandomPos();
                if (!Physics2D.OverlapCircle(generatedPoint, 0.5f) && Physics2D.OverlapCircle(generatedPoint - new Vector2(0,1), 0.5f)) break;
            }
            float distance = (generatedPoint - new Vector2(transform.position.x,transform.position.y)).magnitude;
            for (float _t = 0; _t < distance / movementSpeed; _t += Time.deltaTime)
            {
                transform.position = Vector2.Lerp(transform.position, generatedPoint, 0.02f);
                yield return null;
            }
            yield return new WaitForSeconds(1f);
            if (isEnemy)
            {
                GameObject laserObj = Instantiate(LaserPref, transform.position, Quaternion.identity);
                Vector3 relativePos = player.transform.position - transform.position;
                laserObj.GetComponent<Rigidbody2D>().AddForce(new Vector2(relativePos.x, relativePos.y).normalized * 5f, ForceMode2D.Impulse);
                Destroy(laserObj, 10f);
            }
        }
        
    }
    float someDeg = 0;
    IEnumerator AllyMovement()
    {
        while (true)
        {
            Vector3 relativePos;
            if (oniProfessor != null)
            {
                relativePos = oniProfessor.transform.position - transform.position;
                someDeg = Mathf.Lerp(someDeg, Mathf.Atan2(relativePos.y, relativePos.x) * Mathf.Rad2Deg, 0.05f);

            }
            else
            {
                someDeg += 0.02f;
            }
            
            observer.transform.rotation = Quaternion.Euler(0, 0, someDeg);
            yield return null;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "attack")
        {
            isEnemy = false;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            coroutine = StartCoroutine(AllyMovement());
            enemyCounter = 0;
        }
    }
}
