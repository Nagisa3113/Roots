using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Follow : MonoBehaviour
{
    public Vector3 target;

    public SpriteShapeController ssc;
    private Vector3 offset;
    private float xAxis = 0f;

    private void Awake()
    {
        xAxis = this.transform.position.x;
        // Debug.Log(ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1));
        target = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        offset = transform.position - target;
    }

    // Update is called once per frame
    void Update()
    {
        target = ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1);
        // Debug.Log(ssc.spline.GetPosition(ssc.spline.GetPointCount() - 1));
    }

    private void LateUpdate()
    {
        this.transform.position = target + offset;
        // this.transform.position = new Vector3(xAxis, transform.position.y, transform.position.z);
    }
}