using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicatorsManager : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private RectTransform _pointerTransform;
    [SerializeField] private Image _pointerImage;
    [SerializeField] private CinemachineCamera _playerCamera;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private Sprite _arrowSprite;
    [SerializeField] private Sprite _arrowSprite2;

    private float _pointerSize;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pointerSize = _pointerTransform.rect.width;  
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetScreenPos = Camera.main.WorldToScreenPoint(_target.position);
        bool offScreen = targetScreenPos.x <= _pointerSize || targetScreenPos.x >= Screen.width - _pointerSize || 
            targetScreenPos.y <= _pointerSize || targetScreenPos.y >= Screen.height - _pointerSize;

        if (offScreen)
        {
            Vector3 fromPosition = _playerCamera.transform.position;
            Vector3 toPosition = _target.position;

            Vector3 direction = (toPosition - fromPosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;

            _pointerTransform.localEulerAngles = new Vector3(0, 0, angle);

            _pointerImage.sprite = _arrowSprite;
            Vector3 cappedTargetScreenPosition = targetScreenPos;
            if (cappedTargetScreenPosition.x <= _pointerSize) cappedTargetScreenPosition.x = _pointerSize;
            if (cappedTargetScreenPosition.x >= Screen.width) cappedTargetScreenPosition.x = Screen.width;
            if (cappedTargetScreenPosition.y <= _pointerSize) cappedTargetScreenPosition.x = _pointerSize;
            if (cappedTargetScreenPosition.y >= Screen.height) cappedTargetScreenPosition.x = Screen.height;
            Debug.Log(cappedTargetScreenPosition);
            
            _pointerTransform.position = cappedTargetScreenPosition;
            _pointerTransform.localPosition = new Vector3(_pointerTransform.localPosition.x, _pointerTransform.localPosition.y, 0);
        }
        else
        {
            _pointerImage.sprite = _arrowSprite2;
            _pointerTransform.position = targetScreenPos;
            _pointerTransform.localPosition = new Vector3(_pointerTransform.localPosition.x, _pointerTransform.localPosition.y, 0);
        }
    }
}
