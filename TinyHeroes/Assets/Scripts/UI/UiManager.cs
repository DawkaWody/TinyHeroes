using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [SerializeField] private GameObject[] _playerUis;

    private Image[] _powerUp1Icons;
    private Image[] _powerUp2Icons;
    private Image[] _playerIcons;

    private void Awake()
    {
        if (Instance != null) return;

        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _powerUp1Icons = new Image[_playerUis.Length];
        _powerUp2Icons = new Image[_playerUis.Length];
        _playerIcons = new Image[_playerUis.Length];

        for (int i = 0; i < _playerUis.Length; i++)
        {
            GameObject playerUi = _playerUis[i];

            foreach (Transform o in playerUi.transform)
            {
                switch (o.name)
                {
                    case "PlayerIcon":
                        _playerIcons[i] = o.GetComponent<Image>();
                        break;
                    case "Powerup1Icon":
                        o.gameObject.SetActive(false);
                        _powerUp1Icons[i] = o.GetComponent<Image>();
                        break;
                    case "Powerup2Icon":
                        _powerUp2Icons[i] = o.GetComponent<Image>();
                        o.gameObject.SetActive(false);
                        break;
                }
            }
        }
    }

    public void ShowPowerUp(int playerIndex, int slot, Sprite icon)
    {
        if (slot == 1)
        {
            _powerUp1Icons[playerIndex].sprite = icon;
            _powerUp1Icons[playerIndex].gameObject.SetActive(true);
        }
        else if (slot == 2)
        {
            _powerUp2Icons[playerIndex].sprite = icon;
            _powerUp2Icons[playerIndex].gameObject.SetActive(true);
        }
    }

    public void HidePowerUp(int playerIndex, int slot)
    {
        if (slot == 1)
        {
            _powerUp1Icons[playerIndex].gameObject.SetActive(false);
        }
        else if (slot == 2)
        {
            _powerUp2Icons[playerIndex].gameObject.SetActive(false);
        }
    }
}
