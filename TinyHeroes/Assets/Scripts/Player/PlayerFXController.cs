using UnityEngine;

public class PlayerFXController : MonoBehaviour
{
    [SerializeField] private GameObject _dustFxPrefab;
    

    public void PlayDoubleJumpDust(Vector2 position)
    {
        GameObject dustFx = Instantiate(_dustFxPrefab, position, Quaternion.identity);
        Dust dust = dustFx.GetComponent<Dust>();
        dust.SetType(0);
        dust.Play();
    }

    public void PlayRunDust(Vector2 position)
    {
        GameObject dustFx = Instantiate(_dustFxPrefab, position, Quaternion.identity);
        Dust dust = dustFx.GetComponent<Dust>();
        dust.SetType(1);
        dust.Play();
    }
}
