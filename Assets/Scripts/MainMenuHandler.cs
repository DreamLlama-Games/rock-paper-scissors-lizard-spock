using System.Collections.Generic;
using TMPro;
using UIScripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField] private string gameViewSceneName = "GameViewScene";
    [SerializeField] private GameObject playButton;
    [SerializeField] private List<GameObject> images;
    [SerializeField] private GameObject fancyPanel;
    [SerializeField] private TMP_Text topScoreText;

    private readonly UIEffects.RotateShakeInfo _shakeInfo = new(5f,0.25f,0.1f);
    private readonly UIEffects.PulseInOutInfo _pulseInfo = new(0.8f, 0.8f, 1.2f, -1);

    private void Start()
    {
        var highScore = PlayerPrefs.GetInt("HighScore", 0);
        topScoreText.text = $"Top Score: {highScore}";
        UIEffects.RotateShakeEffect.StartRotateShake(playButton, _shakeInfo);
        UIEffects.PulseInOutEffect.StartPulsingInOut(fancyPanel, _pulseInfo);
        
    }

    public void LoadGameViewScene()
    {
        SceneManager.LoadScene(gameViewSceneName); 
    }
}
