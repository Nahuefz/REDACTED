using UnityEngine;

public class SceneEntrance : MonoBehaviour
{
    [SerializeField] string entranceID;

    private void Start()
    {
        string targetID = PlayerPrefs.GetString("NextSpawnID");

        if (targetID != entranceID) return;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
        }
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        PlayerPrefs.SetString("NextSpawnID", ""); //resetear el id para que funcione
    }
}
