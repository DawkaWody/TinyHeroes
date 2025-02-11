using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    private Image _fillImage;

    private Coroutine _fillingCo;

    void Start()
    {
        _fillImage = GetComponent<Image>();

        Hide();
    }

    public void Fill(float duration)
    {
        if (_fillingCo != null)
        {
            StopCoroutine(_fillingCo);
        }

        _fillImage.gameObject.SetActive(true);
        _fillingCo = StartCoroutine(AnimateFilling(1 / duration));
    }

    private IEnumerator AnimateFilling(float speed)
    {
        float time = 0;
        float initialValue = _fillImage.fillAmount;

        while (time < 1)
        {
            _fillImage.fillAmount = Mathf.Lerp(initialValue, 1, time);
            time += Time.deltaTime * speed;

            yield return null;
        }

        _fillImage.fillAmount = 1;
    }

    public void SetColor(Color color)
    {
        _fillImage.color = color;
    }

    public void Hide()
    {
        _fillImage.fillAmount = 0;
        _fillImage.gameObject.SetActive(false);
    }
}
