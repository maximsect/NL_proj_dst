using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public enum DrawFunctions
{
    SinWave,TanWave,Exp,Sqrt,PingPong,Log
}
public class ProfessorMovement : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer mainImage;
    public LineRenderer rendererPref;
    public EdgeCollider2D edge2D;
    public DrawFunctions drawFunction;
    [Range(2,500)]
    public int numberOfPoints = 10;
    public float xStart = 0, xFinish = 30;
    List<Vector2> linePoints = new List<Vector2>();
    [Header("health")]
    public int hp = 25;
    public Slider hpDisplay;
    public bool isObserved = false;
    public LayerMask groundLayer;
    private Animator animator;
    public Color color;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpDisplay.minValue = 0;
        hpDisplay.maxValue = hp;
        hpDisplay.value = hp;
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        StartCoroutine(ProfessorAnimation());
        //edge2D = rendererPref.gameObject.GetComponent<EdgeCollider2D>();
        //StartCoroutine(DrawSinWave());
    }

    // Update is called once per frame
    float _count = 0;
    void Update()
    {
        Draw();
        if (!isObserved)
        {
            _count += Time.deltaTime;
            color.a = 0.5f + Mathf.Sin(_count * 8) * 0.3f;
            mainImage.material.color = color;
        }
        else
        {
            color.a = 1;
            mainImage.material.color = color;
            selectableMovementIndex.Remove(3);
            selectableMovementIndex.Add(3);
        }
        Vector3 relativePos = player.transform.position - transform.position;
        if(Mathf.Abs(relativePos.y) <= 2)
        {
            transform.localScale = (relativePos.x > 0) ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
        }
    }
    void Draw()
    {
        hpDisplay.value = hp;
        linePoints.Clear();
        rendererPref.positionCount = numberOfPoints;
        for (int currentPoint = 0; currentPoint < numberOfPoints; currentPoint++)
        {
            float progress = (float)currentPoint / (numberOfPoints - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = 0;
            switch (drawFunction)
            {
                case DrawFunctions.SinWave:
                    y = Mathf.Sin(x);
                    break;
                case DrawFunctions.TanWave:
                    y = Mathf.Tan(x);
                    break;
                case DrawFunctions.Exp:
                    y = Mathf.Exp(x);
                    break;
                case DrawFunctions.Sqrt:
                    y = Mathf.Sqrt(x);
                    break;
                case DrawFunctions.PingPong:
                    y = Mathf.PingPong(x,1);
                    break;
                case DrawFunctions.Log:
                    if(x != 0)
                    y = Mathf.Log(x);
                    break;
                default:
                    break;
            }
            rendererPref.SetPosition(currentPoint, new Vector3(x, y, 0));
            linePoints.Add(new Vector2(x, y));
            
        }
        edge2D.SetPoints(linePoints);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        hpDisplay.value = hp;
        if (other.gameObject.tag == "attack" || other.gameObject.tag == "skillattack")
        {
            bool checker = false;
            if (!isObserved) checker = UnityEngine.Random.Range(0f, 1f) > 0.5;
            else checker = true;
            if (checker)
            {
                if (--hp == 0)
                {
                    SceneTransition.main.StageClearReciever();
                    Destroy(rendererPref.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        print(other);
        if(other.gameObject.tag == "observer")
        {
            isObserved = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "observer")
        {
            isObserved = false;
        }
    }
    Vector2 RandomPos()
    {
        while (true)
        {
            Vector2 pos = new Vector2(Random.Range(-3f, 10f), Random.Range(-10f, 10f));
            if (Physics2D.OverlapCircle(pos, 0.5f,groundLayer)) continue;
            if (!Physics2D.OverlapCircle(pos - new Vector2(0, 0.3f), 0.5f,groundLayer)) continue;
            if (Vector3.Distance(transform.position, pos) < 7) continue;
            return pos + new Vector2(0, 0.2f);
        }
    }
    List<int> selectableMovementIndex = new List<int>() { 0, 1, 2, 3 };
    IEnumerator ProfessorAnimation()
    {
        while (true)
        {
            int rand = selectableMovementIndex[Random.Range(0, selectableMovementIndex.Count - 1)] ;
            print(rand);
            selectableMovementIndex.Remove(rand);
            selectableMovementIndex.Add(rand);
            switch (rand)
            {
                case 0://idol
                    AnimeState(0);
                    yield return new WaitForSeconds(2f);
                    break;
                case 1://attack
                    AnimeState(1);
                    yield return new WaitForSeconds(0.2f);
                    break;
                case 2://attackWithSin
                    AnimeState(2);
                    yield return new WaitForSeconds(0.2f);
                    break;
                case 3:// Warp
                    AnimeState(3);
                    yield return new WaitForSeconds(0.4f);
                    transform.position = RandomPos().ToVector3();
                    AnimeState(0);
                    yield return new WaitForSeconds(0.4f);
                    break;
                default:
                    break;
            }
        }
    }
    void AnimeState(int index)
    {
        animator.SetInteger("Mode", index);
    }
}
