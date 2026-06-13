using UnityEngine;

public class InfiniteLoop : MonoBehaviour
{
    public float mapSize = 24f;
    private float limit;
    private Rigidbody rb;

    private void Start()
    {
        limit = mapSize / 2f;
        rb = GetComponent<Rigidbody>();

        if (rb == null) Debug.LogError("Falta el componente Rigidbody");
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        Vector3 pos = rb.position;
        bool huboTeletransporte = false;

        if (pos.x > limit)
        {
            pos.x -= mapSize;
            huboTeletransporte = true;
        }
        else if (pos.x < -limit)
        {
            pos.x += mapSize;
            huboTeletransporte = true;
        }

        if (pos.z > limit)
        {
            pos.z -= mapSize;
            huboTeletransporte = true;
        }
        else if (pos.z < -limit)
        {
            pos.z += mapSize;
            huboTeletransporte = true;
        }

        if (huboTeletransporte)
        {
            rb.position = pos;
        }
    }
}