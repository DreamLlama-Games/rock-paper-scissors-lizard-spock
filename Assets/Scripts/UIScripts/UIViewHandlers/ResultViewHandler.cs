using EventManagerScripts;
using GameEnums;
using TMPro;

namespace UIScripts.UIViewHandlers
{
    public class ResultViewHandler
    {
        private const string BobCountOne = "Ready";
        private const string BobCountTwo = "Set";
        private const string BobCountThree = "Go!";
        private const string OptionSelectText = "Choose an option";
        
        private TMP_Text _resultsText;
        private GameEventManager _eventManager;

        public void Initialize(GameEventManager eventManager, TMP_Text resultsText)
        {
            _resultsText = resultsText;
            _eventManager = eventManager;

            Subscribe();
        }

        private void SetResultText(string text)
        {
            _resultsText.text = text;
        }

        private void Subscribe()
        {
            _eventManager?.Subscribe(GameEvent.GameStarted, GameStarted);
            _eventManager?.Subscribe<int>(GameEvent.BobCount, BobCount);
            _eventManager?.Subscribe<string>(GameEvent.PlayerWon, text =>  SetResultText(text+" wins!"));
            _eventManager?.Subscribe<string>(GameEvent.PlayerLost, text =>  SetResultText(text+" wins!"));
            _eventManager?.Subscribe(GameEvent.NewRoundStarted, GameStarted);
            _eventManager?.Subscribe<string>(GameEvent.MatchTied, SetResultText);
        }

        private void BobCount(int count)
        {
            var text = count switch
            {
                1 => BobCountOne,
                2 => BobCountTwo,
                3 => BobCountThree,
                _ => count.ToString()
            };
            SetResultText(text);
        }

        private void GameStarted()
        {
            SetResultText(OptionSelectText);
        }
    }
}