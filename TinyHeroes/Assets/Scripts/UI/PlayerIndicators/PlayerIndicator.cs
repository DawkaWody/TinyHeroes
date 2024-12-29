using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _rotationOffset;
    
    [HideInInspector] public new Camera camera;
    [HideInInspector] public float borderSize;

    private float border;

    private RectTransform _pointerTransform;
    private Image _pointerImage;

    private void Start()
    {
        _pointerTransform = GetComponent<RectTransform>();
        _pointerImage = GetComponent<Image>();

        border = borderSize + _pointerTransform.rect.width;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetScreenPos = camera.WorldToScreenPoint(_target.position);
        bool offScreen = targetScreenPos.x <= border || targetScreenPos.x >= Screen.width - border ||
            targetScreenPos.y <= border || targetScreenPos.y >= Screen.height - border;

        if (offScreen)
        {
            _pointerImage.enabled = true;
            Vector3 fromPosition = camera.transform.position;
            Vector3 toPosition = _target.position;

            Vector3 direction = (toPosition - fromPosition).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if (angle < 0) angle += 360;
            angle += _rotationOffset;

            _pointerTransform.localEulerAngles = new Vector3(0, 0, angle);

            Vector3 cappedTargetScreenPosition = targetScreenPos;
            cappedTargetScreenPosition.x = Mathf.Clamp(cappedTargetScreenPosition.x, border, Screen.width - border);
            cappedTargetScreenPosition.y = Mathf.Clamp(cappedTargetScreenPosition.y, border, Screen.height - border);

            _pointerTransform.position = cappedTargetScreenPosition;
            _pointerTransform.localPosition = new Vector3(_pointerTransform.localPosition.x, _pointerTransform.localPosition.y, 0);
        }
        else
        {
            _pointerImage.enabled = false;
        }
    }
}
