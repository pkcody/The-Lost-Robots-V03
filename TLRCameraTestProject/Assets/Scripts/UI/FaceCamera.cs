using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    public Transform lookAt;
    private Transform localTrans;

    void Start()
    {
        localTrans = GetComponent<Transform>();

        if(transform.root.tag != "Player")
        {
            SelectCamera();
        }
    }

    public void SelectCamera()
    {
        lookAt = Camera.main.transform;
    }

    void Update()
    {
        if (lookAt)
        {
            localTrans.LookAt(2 * localTrans.position - lookAt.position);
        }
    }
}
