using EventManagerScripts;
using GameEnums;
using GameLogicScripts;
using RelationMatrix;
using ScriptableObjectScripts;
using TMPro;
using UIScripts.UIViewHandlers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UIScripts
{
    public class GameInitializer : MonoBehaviour
    {
        [Header("Game logic")]
        [SerializeField] private SymmetricRelationMatrix matrix;
        [SerializeField] private string mainMenuSceneName = "MainMenuScene";
        
        [Header("Image Data")]
        [SerializeField] private SOButtonImageData buttonImageData;
        [SerializeField] private SOMoveImageData moveImageData;
        
        [Header("Timer")]
        [SerializeField] private float timerDuration;
        [SerializeField] private Slider timerSlider;
        
        [Header("Options carousel")]
        [SerializeField] private GameObject carouselParent;
        [SerializeField] private Button optionButtonPrefab;

        [Header("Win lose screen")]
        [SerializeField] private GameObject winLoseScreen;
        [SerializeField] private TMP_Text winLoseScreenText;
        [SerializeField] private Button homeScreenButton;
        
        [Header("Moves")]
        [SerializeField] private GameObject playerMove;
        [SerializeField] private GameObject opponentMove;
        
        [Header("Results text")]
        [SerializeField] private TMP_Text resultsText;
        
        [Header("Score view")]
        [SerializeField] private TMP_Text scoreViewText;
        [SerializeField] private GameObject scoreViewParent;
        
        //Game Logic
        private readonly GameLogicHandler _gameLogicHandler = new(); 
        
        //All widgets
        private readonly TimerViewHandler _timerViewHandler = new();
        private readonly OptionCarouselHandler _optionCarouselHandler = new();
        private readonly WinLoseScreenHandler _winLoseScreenHandler = new();
        private readonly MoveViewHandler  _moveViewHandler = new();
        private readonly ResultViewHandler _resultViewHandler = new();
        private readonly ScoreViewHandler _scoreViewHandler = new();
        
        //Event Manager
        private readonly GameEventManager _eventManager = new();

        private void Start()
        {
            InitializeHandlers();
            InitializeGameLogic();
            StartGame();
        }

        private void StartGame()
        {
            _eventManager.Raise(GameEvent.GameStarted,this);
        }

        private void EndGame()
        {
            _eventManager.Raise(GameEvent.GameEnded,this);
        }

        private void InitializeGameLogic()
        {
            _gameLogicHandler.Initialize(_eventManager,matrix);
        }

        private void InitializeHandlers()
        {
            _timerViewHandler.Initialize(_eventManager, timerSlider, timerDuration);
            _optionCarouselHandler.Initialize(_eventManager, carouselParent, optionButtonPrefab, matrix.Elements, buttonImageData);
            _winLoseScreenHandler.Initialize(_eventManager, winLoseScreen, winLoseScreenText, homeScreenButton);
            _moveViewHandler.Initialize(_eventManager, playerMove, opponentMove, moveImageData);
            _resultViewHandler.Initialize(_eventManager, resultsText);
            _scoreViewHandler.Initialize(_eventManager, scoreViewParent, scoreViewText);
        }

        private void Update()
        {
            _gameLogicHandler.Tick(Time.deltaTime);
            _timerViewHandler.Tick(Time.deltaTime);
        }
        
        public void LoadMainMenuScene()
        {
            SceneManager.LoadScene(mainMenuSceneName); 
        }
    }
}
