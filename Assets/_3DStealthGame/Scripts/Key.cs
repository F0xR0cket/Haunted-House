using UnityEngine;

public class Key : MonoBehaviour
{
    public string KeyName;
    public GameObject key;

    private void OnTriggerEnter(Collider other)
    {
        PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();
        if (player == null) return;

        player.AddKey(KeyName);
        Destroy(key);
    }
}

