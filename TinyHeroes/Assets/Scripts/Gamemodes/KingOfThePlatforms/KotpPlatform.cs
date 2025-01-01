using UnityEngine;

public class KotpPlatform : MonoBehaviour
{
    public string color;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GLOBALS.playerTag))
        {
            color = GLOBALS.playerColors[other.GetComponentInParent<PlayerData>().index];
            Debug.Log(color);
        }
    }
}
