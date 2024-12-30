using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicatorsManager : MonoBehaviour
{
    [SerializeField] private PlayerIndicator[] _indicators;
    [SerializeField] private Sprite[] _playerSprites;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _borderSize;

    private void Awake()
    {
        for (int i = 0; i < _indicators.Length; i++)
        {
            PlayerIndicator indicator = _indicators[i];
            indicator.targetSprite = _playerSprites[i];
            indicator.camera = _camera;
            indicator.borderSize = _borderSize;
        }
    }
}
