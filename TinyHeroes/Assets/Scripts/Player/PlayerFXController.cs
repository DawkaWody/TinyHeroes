using UnityEngine;

public class PlayerFXController : MonoBehaviour
{
    [SerializeField] private GameObject _dustFxPrefab;
    

    public void PlayDoubleJumpDust(Vector3 position)
    {
        GameObject dustFx = Instantiate(_dustFxPrefab, position, Quaternion.identity);
        Dust dust = dustFx.GetComponent<Dust>();
        dust.SetType(0);
        dust.Play();
    }

    public void PlayRunDust(Vector3 position)
    {
        GameObject dustFx = Instantiate(_dustFxPrefab, position, Quaternion.identity);
        Dust dust = dustFx.GetComponent<Dust>();
        dust.SetType(1);
        dust.Play();
    }
}
