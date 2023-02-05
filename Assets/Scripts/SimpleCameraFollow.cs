using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = this.transform.position - target.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newpos = target.position + offset;
        this.transform.position = new Vector3(transform.position.x, newpos.y, transform.position.z);
    }
}