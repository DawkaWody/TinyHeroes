using UnityEngine;

public class PlayerFXController : MonoBehaviour
{
    [SerializeField] private GameObject _dustFxPrefab;
    

    public void PlayDoubleJumpDust(Vector3 position)
    {
        GameObject newDust = Instantiate(_dustFxPrefab, position, Quaternion.identity);
        Dust dust = newDust.GetComponent<Dust>();
        dust.SetType(0);
        dust.Play();
    }
}
