using UnityEngine;

public class PichoActivator : MonoBehaviour
{
    [Header("Ajustes de la trampa")]
    public GameObject trapToActivate;

    public void Awake()
    {
        SetTrapState(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           ActivatePincho();
        }
    }

    void ActivatePincho()
    {
        if (trapToActivate != null)
        {
            SetTrapState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetTrapState(false);
        }
    }

    void SetTrapState(bool state)
    {
        if (trapToActivate != null)
        {
            trapToActivate.SetActive(state);
        }
    }
}
