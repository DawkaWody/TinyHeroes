using UnityEngine;

public class KillPlayer : MonoBehaviour
{
    private BoxCollider2D _collider;

    void Start()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GLOBALS.playerTag))
        {
            if (other.transform.position.y < _collider.bounds.min.y)
            {
                Debug.Log(other);
                other.GetComponentInParent<PlayerDeathController>().Die();
            }
        }
    }
}
