using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI nextWaveText;
    [SerializeField] private TextMeshProUGUI waveTimerText;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject gameWonScreen;


    public TextMeshProUGUI WaveText => waveText;
    public TextMeshProUGUI NextWaveText => nextWaveText;
    public TextMeshProUGUI WaveTimerText => waveTimerText;
    public TextMeshProUGUI CountdownText => countdownText;
    public GameObject GameOverScreen => gameOverScreen;
    public GameObject GameWonScreen => gameWonScreen;

   public void Start()
    {
        waveText.text = "Wave 1";
        waveText.gameObject.SetActive(true);
    }

}
