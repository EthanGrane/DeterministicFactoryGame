using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStateUI : MonoBehaviour
{
    [Header("Game Timer")]
    public Image nextRoundTimerImage;
    public TextMeshProUGUI nextRoundTimerText;
    [Header("Game Waves")]
    public TextMeshProUGUI waveCountText;
    public Button nextWaveButton;

    bool timerIsRunning = false;

    const float MaxTime = 60f;

    private void Start()
    {
        EnemyWavesManager.Instance.onPhaseChanged += CheckPhase;
        
        nextWaveButton.onClick.AddListener(() =>
        {
            EnemyWavesManager.Instance.SkipWave();
        });
    }

    void CheckPhase(WavePhase phase)
    {
        if (phase == WavePhase.Planning)
        {
            timerIsRunning = true;
            nextRoundTimerImage.fillAmount = 1f;
            nextRoundTimerText.enabled = true;
            nextWaveButton.gameObject.SetActive(true);
        }
        else
        {
            timerIsRunning = false;
            nextRoundTimerImage.fillAmount = 0f;
            nextRoundTimerText.enabled = false;
            UpdateWaveCount();
            nextWaveButton.gameObject.SetActive(false);
        }
    }

    void UpdateWaveCount()
    {
        waveCountText.text = $"Wave {EnemyWavesManager.Instance.currentWave + 1}/{EnemyWavesManager.Instance.waves.Length + 1}";
    }
    
    private void Update()
    {
        if (!timerIsRunning) return;

        float timer = EnemyWavesManager.Instance.GetPhaseTimerCountdown();
        float normalized = Mathf.Clamp01(timer / MaxTime);
        nextRoundTimerImage.fillAmount = 1f - normalized;
    }

    private void OnDestroy()
    {
        if (EnemyWavesManager.Instance != null)
            EnemyWavesManager.Instance.onPhaseChanged -= CheckPhase;
    }
}