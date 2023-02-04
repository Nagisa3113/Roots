using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

[Serializable]
public struct Point
{
    public int index;
    public Vector3 position;
    public Vector3 rightTangent;
}

public class RootTest : MonoBehaviour
{
    public SpriteShapeRenderer ssr;
    public SpriteShapeController ssc;

    public List<Point> points;
    private readonly float speed = 0.7f;


    private void Awake()
    {
        ssr = GetComponent<SpriteShapeRenderer>();
        ssc = GetComponent<SpriteShapeController>();
    }

    private void Update()
    {
        ShowSplinePoint();

        if (Input.GetKeyDown(KeyCode.A)) CreateNewPoint();
        if (Input.GetKeyUp(KeyCode.A)) CreateNewPoint();
        if (Input.GetKey(KeyCode.A)) ChangeDirection(-0.12f);

        if (Input.GetKeyDown(KeyCode.D)) CreateNewPoint();
        if (Input.GetKeyUp(KeyCode.D)) CreateNewPoint();
        if (Input.GetKey(KeyCode.D)) ChangeDirection(0.12f);
    }

    private void FixedUpdate()
    {
        AutoGrow();
    }


    private void ShowSplinePoint()
    {
        points.Clear();
        for (var i = 0; i < ssc.spline.GetPointCount(); i++)
        {
            var p = new Point();
            p.index = i;
            p.position = ssc.spline.GetPosition(i);
            p.rightTangent = ssc.spline.GetRightTangent(i);
            points.Add(p);
        }
    }

    private void ChangeDirection(float dir)
    {
        var spline = ssc.spline;
        var i = spline.GetPointCount();
        var lastPos = spline.GetPosition(i - 1);
        var lastTangent = spline.GetRightTangent(i - 1);
        // Vector3 offset = new Vector3(bias, -0.4f, 0); 
        var offset = lastTangent.normalized;
        offset = new Vector3(offset.x + dir * 0.3f, offset.y, offset.z);

        var angle = Vector3.Angle(offset, Vector3.right);
        if (angle < 15f || angle > 165f) return;

        spline.SetPosition(i - 1, lastPos + offset * (Time.deltaTime * speed));
        spline.SetRightTangent(i - 1, offset.normalized);

        ssc.RefreshSpriteShape();
    }

    private void AutoGrow()
    {
        var spline = ssc.spline;
        var i = spline.GetPointCount();

        var lastPos = spline.GetPosition(i - 1);
        var llPos = spline.GetPosition(i - 2);
        var lastTangent = spline.GetRightTangent(i - 1);
        var offset = lastTangent.normalized * 1.1f;
        var deltaY = llPos.y - lastPos.y;
        spline.SetPosition(i - 1, lastPos + offset * (Time.fixedDeltaTime * speed));
        spline.SetRightTangent(i - 1, lastTangent.normalized * (deltaY * 10.6f));

        ssc.RefreshSpriteShape();
    }

    private void CreateNewPoint()
    {
        var spline = ssc.spline;
        var i = spline.GetPointCount();
        var lastPos = spline.GetPosition(i - 1);
        var lastTangent = spline.GetRightTangent(i - 1).normalized;
        spline.SetRightTangent(i - 1, lastTangent * 0.07f);
        // spline.SetPosition(i - 1, lastPos - lastTangent * 0.1f);
        var offset = lastTangent.normalized;
        spline.InsertPointAt(i, lastPos + 0.10f * offset);
        spline.SetTangentMode(i, ShapeTangentMode.Continuous);
        spline.SetRightTangent(i, lastTangent * 0.6f);
        spline.SetHeight(i, 0.3f);
        // spline.SetRightTangent(i, new Vector3(cum_bias, -.01f, 0) * 0.01f);
        // spline.SetLeftTangent(i, rotation * Vector3.up * tangentLength);
        ssc.RefreshSpriteShape();
    }
}