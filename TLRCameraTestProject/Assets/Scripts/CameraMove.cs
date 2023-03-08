using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraMove : MonoBehaviour
{
    public GameObject cineMlow;
    public GameObject cineMHigh;
    public CinemachineTargetGroup cinemachineTargetGroup;
    public List<Transform> allPlayerPos = new List<Transform>();

    public float middle = 20;
    public float averageAllPlayerDist = 0;

    private void Start()
    {
        foreach (var cm in FindObjectsOfType<CharacterMovement>())
        {
            allPlayerPos.Add(cm.transform);
        }
    }

    public void GetCamLow()
    {
        cineMlow.GetComponent<CinemachineVirtualCamera>().Priority = 11;
        cineMHigh.GetComponent<CinemachineVirtualCamera>().Priority = 10;
    }
    public void GetCamHigh()
    {
        cineMHigh.GetComponent<CinemachineVirtualCamera>().Priority = 11;
        cineMlow.GetComponent<CinemachineVirtualCamera>().Priority = 10;
    }
    private void Update()
    {
        try
        {
            foreach (Transform pos in allPlayerPos)
            {
                averageAllPlayerDist += (pos.position - cinemachineTargetGroup.transform.position).magnitude;
            }
            averageAllPlayerDist /= allPlayerPos.Count;
            if (averageAllPlayerDist > middle)
            {
                GetCamHigh();
            }
            else
            {
                GetCamLow();
            }
        }
        catch
        {
            print("hacky");
        }
    }
}
