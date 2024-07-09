using Assets.Scripts.Utlities;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI nextWaveText;
    public TextMeshProUGUI waveTimerText;
    public TextMeshProUGUI countdownText;
    public GameObject gameOverScreen;
    public GameObject gameWonScreen;

    void Start()
    {
        gameOverScreen.SetActive(false);
        gameWonScreen.SetActive(false);

        waveText.text = "Wave 1";
        waveText.gameObject.SetActive(true);
    }

}
