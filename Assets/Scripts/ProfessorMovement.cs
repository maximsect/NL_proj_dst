using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public enum DrawFunctions
{
    SinWave,TanWave,Exp,Sqrt,PingPong,Log
}
public class ProfessorMovement : MonoBehaviour
{
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hpDisplay.minValue = 0;
        hpDisplay.maxValue = hp;
        hpDisplay.value = hp;
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
            mainImage.enabled = Mathf.PingPong(10 *_count, 1) < 0.5f;
        }
        else
        {
            mainImage.enabled = true;
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
    //IEnumerator DrawSinWave()
    //{

    //}
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
                    SceneScript.KillEnemy();
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
}
