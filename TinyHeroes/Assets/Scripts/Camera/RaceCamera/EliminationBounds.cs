using UnityEngine;

public class EliminationBounds : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) {
            Debug.Log(collider.name + "eliminated!");

            collider.gameObject.SetActive(false);
        }
    }
}
