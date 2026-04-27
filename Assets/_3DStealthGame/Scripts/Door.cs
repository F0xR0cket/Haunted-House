using UnityEngine;

public class Door : MonoBehaviour
{
    public string KeyName;
    public GameObject door;

    private void OnCollisionEnter(Collision collision)
    {
        PlayerMovement player = collision.gameObject.GetComponent<PlayerMovement>();
        if (player == null) return;

        if (player.OwnKey(KeyName))
        {
            Destroy(door);
        }
    }
}