using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicatorsManager : MonoBehaviour
{
    [SerializeField] private PlayerIndicator[] _indicators;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _borderSize;

    private void Awake()
    {
        foreach (PlayerIndicator indicator in _indicators)
        {
            indicator.camera = _camera;
            indicator.borderSize = _borderSize;
        }
    }
}
