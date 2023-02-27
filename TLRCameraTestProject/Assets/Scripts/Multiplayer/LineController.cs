using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lineRenderer;
    public Transform[] lineToAndFrom;
    public Transform target;
    public bool canShowLine = false;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        DisableLine();
    }

    public void DisableLine()
    {
        lineRenderer.enabled = false;
    }
    public void StartLine()
    {
        lineRenderer.enabled = true;
        Transform[] pos = new Transform[2];
        pos[0] = target;
        pos[1] = transform.parent;
        

        SetLinePoints(pos);
        canShowLine = true;
    }

    public void SetLinePoints(Transform[] points)
    {
        lineRenderer.positionCount = points.Length;
        lineToAndFrom = points;
    }

    private void Update()
    {
        if(canShowLine)
        {
            print("showing line");
            for (int i = 0; i < lineToAndFrom.Length; i++)
            {
                lineRenderer.SetPosition(i, lineToAndFrom[i].position);
            }
        }
        
    }
}
