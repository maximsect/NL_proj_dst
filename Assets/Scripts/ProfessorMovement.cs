using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Random = UnityEngine.Random;
public enum DrawFunctions
{
    LinearF,QuadraticF,CubicF,SinWave,TanWave,Exp,Sqrt,PingPong,Log,Abs,InverseX
}
public class ProfessorMovement : MonoBehaviour
{
    public GameObject player;
    public SpriteRenderer mainImage;
    public GameObject functionRenderObj;
    public DrawFunctions drawFunction;
    [Range(2,500)]
    public int numberOfPoints = 10;
    public float xStart = 0, xFinish = 30;
    [Header("health")]
    public int hp = 25;
    public Slider hpDisplay;
    public bool isObserved = false;
    public LayerMask groundLayer;
    private Animator animator;
    public Color color;

    List<Vector2> GeneratePoints(DrawFunctions drawFunc)
    {
        List<Vector2> list = new List<Vector2>();
        for (int currentPoint = 0; currentPoint < numberOfPoints; currentPoint++)
        {
            float progress = (float)currentPoint / (numberOfPoints - 1);
            float x = Mathf.Lerp(xStart, xFinish, progress);
            float y = 0;
            switch (drawFunc)
            {
                case DrawFunctions.LinearF:
                    y = x;
                    break;
                case DrawFunctions.QuadraticF:
                    y = x * x;
                    break;
                case DrawFunctions.CubicF:
                    y = x * x * x;
                    break;
                case DrawFunctions.Abs:
                    y = Mathf.Abs(x);
                    break;
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
                    y = Mathf.Sqrt(Mathf.Abs(x));
                    break;
                case DrawFunctions.PingPong:
                    y = Mathf.PingPong(x, 1);
                    break;
                case DrawFunctions.Log:
                    if (x != 0)
                        y = Mathf.Log(Mathf.Abs(x));
                    break;
                case DrawFunctions.InverseX:
                    if (x != 0)
                        y = 1 / x;
                    break;
                default:
                    break;
            }
            list.Add(new Vector2(x, y));
        }
        return list;
    }

    void Start()
    {
        hpDisplay.minValue = 0;
        hpDisplay.maxValue = hp;
        hpDisplay.value = hp;
        animator = transform.GetChild(0).gameObject.GetComponent<Animator>();
        StartCoroutine(ProfessorAnimation());
    }

    float _count = 0;
    void Update()
    {
        hpDisplay.value = hp;
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
                    Destroy(this.gameObject);
                }
            }
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
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
            if (!Physics2D.OverlapCircle(pos - new Vector2(0, 0.1f), 0.5f,groundLayer)) continue;
            if (Vector3.Distance(transform.position, pos) < 7) continue;
            return pos + new Vector2(0, 0.8f);
        }
    }
    List<int> selectableMovementIndex = new List<int>() { 0, 1, 2, 3 };
    IEnumerator ProfessorAnimation()
    {
        while (true)
        {
            int rand = selectableMovementIndex[Random.Range(0, selectableMovementIndex.Count - 1)] ;
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
                    StartCoroutine(FuncAttack((DrawFunctions)(Random.Range(0, Enum.GetValues(typeof(DrawFunctions)).Length))));
                    yield return new WaitForSeconds(0.2f);
                    AnimeState(0);
                    yield return new WaitForSeconds(2f);
                    break;
                case 3:// Warp
                    AnimeState(3);
                    yield return new WaitForSeconds(1.0f);
                    transform.position = RandomPos().ToVector3();
                    AnimeState(0);
                    yield return new WaitForSeconds(0.4f);
                    break;
                default:
                    break;
            }
        }
    }
    IEnumerator FuncAttack(DrawFunctions drawFunc)
    {
        GameObject sinWave = Instantiate(functionRenderObj, player.transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)));
        LineRenderer lineRenderer = sinWave.GetComponent<LineRenderer>(); 
        lineRenderer.positionCount = numberOfPoints;
        List<Vector2> drawPoints = GeneratePoints(drawFunc);
        for (int i = 0; i < drawPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, drawPoints[i].ToVector3());
        }
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(0.15f, 0.0f) }
            );
        lineRenderer.colorGradient = gradient;
        yield return new WaitForSeconds(0.6f);
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1f, 0.0f) }
            );
        lineRenderer.colorGradient = gradient;
        EdgeCollider2D edges = sinWave.GetComponent<EdgeCollider2D>();
        edges.SetPoints(drawPoints);
        sinWave.tag = "Enemy";
        yield return new WaitForSeconds(0.6f);
        Destroy(sinWave);
    }
    void AnimeState(int index)
    {
        animator.SetInteger("Mode", index);
    }
}
