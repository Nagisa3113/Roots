using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[System.Serializable]
public struct Point
{
    public int index;
    public Vector3 position;
    public Vector3 rightTangent;
}

public class RootController : MonoBehaviour
{
    public SpriteShapeRenderer ssr;
    public SpriteShapeController ssc;

    public List<Point> points;
    float speed = 0.4f;

    private void Awake()
    {
        ssr = GetComponent<SpriteShapeRenderer>();
        ssc = GetComponent<SpriteShapeController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateNewPoint();
    }

    // Update is called once per frame
    void Update()
    {
        points.Clear();
        for (int i = 0; i < ssc.spline.GetPointCount(); i++)
        {
            Point p = new Point();
            p.index = i;
            p.position = ssc.spline.GetPosition(i);
            p.rightTangent = ssc.spline.GetRightTangent(i);
            points.Add(p);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            CreateNewPoint();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            CreateNewPoint();
        }

        if (Input.GetKey(KeyCode.A))
        {
            ChangeDirection(-0.12f);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            CreateNewPoint();
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            CreateNewPoint();
        }

        if (Input.GetKey(KeyCode.D))
        {
            ChangeDirection(0.12f);
        }
    }

    private void FixedUpdate()
    {
        AutoGrow();
    }

    void ChangeDirection(float dir)
    {
        Spline spline = ssc.spline;
        Quaternion rotation = Quaternion.identity;
        int i = spline.GetPointCount();

        Vector3 lastPos = spline.GetPosition(i - 1);
        Vector3 lastTangent = spline.GetRightTangent(i - 1);
        // Vector3 offset = new Vector3(bias, -0.4f, 0); 
        Vector3 offset = lastTangent.normalized;
        offset = new Vector3(offset.x + dir * 0.5f, offset.y, offset.z);
        spline.SetPosition(i - 1, lastPos + offset * (Time.deltaTime * speed));
        spline.SetRightTangent(i - 1, offset.normalized);

        ssc.RefreshSpriteShape();
    }

    void AutoGrow()
    {
        Spline spline = ssc.spline;
        int i = spline.GetPointCount();

        Vector3 lastPos = spline.GetPosition(i - 1);
        Vector3 llPos = spline.GetPosition(i - 2);
        Vector3 lastTangent = spline.GetRightTangent(i - 1);
        Vector3 offset = lastTangent.normalized * 1.1f;
        float deltaY = llPos.y - lastPos.y;
        spline.SetPosition(i - 1, lastPos + offset * (Time.deltaTime * speed));
        spline.SetRightTangent(i - 1, lastTangent.normalized * (deltaY * 7.6f));
        ssc.RefreshSpriteShape();
    }

    void CreateNewPoint()
    {
        Spline spline = ssc.spline;
        int i = spline.GetPointCount();

        Vector3 lastPos = spline.GetPosition(i - 1);
        Vector3 lastTangent = spline.GetRightTangent(i - 1).normalized;
        spline.SetRightTangent(i - 1, lastTangent * 0.1f);
        // spline.SetPosition(i - 1, lastPos - lastTangent * 0.1f);
        Vector3 offset = lastTangent.normalized;
        spline.InsertPointAt(i, lastPos + 0.1f * offset);
        spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        spline.SetRightTangent(i, lastTangent * 0.1f);
        spline.SetHeight(i, 0.3f);
        // spline.SetRightTangent(i, new Vector3(cum_bias, -.01f, 0) * 0.01f);
        // spline.SetLeftTangent(i, rotation * Vector3.up * tangentLength);
        ssc.RefreshSpriteShape();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name);
    }
}