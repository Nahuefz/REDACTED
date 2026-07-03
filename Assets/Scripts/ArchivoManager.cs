using System;
using UnityEngine;

public class ArchivoManager : MonoBehaviour
{
    [SerializeField] private string missionToCheck;
    [SerializeField] private GameObject[] objectsToDisable;
    [SerializeField] private GameObject[] objectsToEnable;
    [SerializeField] private Transform ratTransform;
    [SerializeField] private Transform ratNextPosition;
    private bool _isMissionDone;
    private void Awake()
    {
        if (missionToCheck == null) return;

        if (!GlobalMissions.GetMission(missionToCheck)) return;
        _isMissionDone = true;
    }

    private void Start()
    {
        
        ////////////////////////////////////
        if (!_isMissionDone) return;
        ratTransform.position = ratNextPosition.position;
        foreach (var obj in objectsToDisable) obj.SetActive(false);
        foreach (var obj in objectsToEnable) obj.SetActive(true);
    }
}
