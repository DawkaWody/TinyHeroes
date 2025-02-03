using System.Collections;
using UnityEngine;

public class IncreaseReach : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _rangeMultiplier = 1.3f;
    [SerializeField] private float _duration = 3f;
    public void Use(PlayerPowerupController player)
    {
        StartCoroutine(ExtendReach(player));
    }

    private IEnumerator ExtendReach(PlayerPowerupController player)
    {
        player.attackRangeMultiplier = _rangeMultiplier;
        yield return new WaitForSeconds(_duration);
        player.attackRangeMultiplier = 1f;

        Destroy(gameObject);
    }
}
