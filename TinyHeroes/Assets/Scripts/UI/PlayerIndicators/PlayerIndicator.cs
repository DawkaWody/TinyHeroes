using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotationOffset;
    
    [SerializeField] private RectTransform _arrowTransform;
    [SerializeField] private Image _arrowImage;

    [SerializeField] private RectTransform _circleTransform;
    [SerializeField] private Image _circleImage;
    [SerializeField] private Sprite _targetSprite;
    
    [HideInInspector] public new Camera camera;
    [HideInInspector] public float borderSize;

    private float border;

    private void Start()
    {
        border = borderSize + _arrowTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetScreenPos = camera.WorldToScreenPoint(_target.position);
        bool offScreen = targetScreenPos.x <= border || targetScreenPos.x >= Screen.width - border ||
            targetScreenPos.y <= border || targetScreenPos.y >= Screen.height - border;

        if (offScreen)
        {
            _arrowImage.enabled = true;
            _circleImage.enabled = true;

            Vector3 fromPosition = camera.transform.position;
            Vector3 toPosition = _target.position;

            Vector3 direction = (toPosition - fromPosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            angle += _rotationOffset;

            _arrowTransform.localEulerAngles = new Vector3(0, 0, angle);

            Vector3 cappedTargetScreenPosition = targetScreenPos;
            cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, border, Screen.width - border);
            cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, border, Screen.height - border);

            _circleTransform.position = cappedTargetScreenPosition;
            _circleTransform.localPosition = new Vector3(_circleTransform.localPosition.x, _circleTransform.localPosition.y, 0);
            _arrowTransform.position = _circleTransform.position + direction * _circleTransform.rect.width;
        }
        else
        {
            _arrowImage.enabled = false;
            _circleImage.enabled = false;
        }
    }
}
