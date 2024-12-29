using UnityEngine;

public class EliminationBounds : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.CompareTag("Player")) 
        {
            GameObject playerObject = collider.transform.root.gameObject;
            Debug.Log(playerObject.name + " eliminated!");
            playerObject.SetActive(false);
        }
    }
}
