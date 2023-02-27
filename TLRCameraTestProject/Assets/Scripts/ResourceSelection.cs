using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSelection : MonoBehaviour
{
    public GameObject resource;

    private void Start()
    {
        resource = transform.GetChild(0).gameObject;
        resource.SetActive(false);

    }
    public void GetResourceSource()
    {
        resource.SetActive(true);
    }
}
