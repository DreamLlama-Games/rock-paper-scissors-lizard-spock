using EventManagerScripts;
using TMPro;

namespace UIScripts.UIViewHandlers
{
    public class ResultViewHandler
    {
        private TMP_Text _resultsText;
        private GameEventManager _eventManager;

        public void Initialize(GameEventManager eventManager, TMP_Text resultsText)
        {
            _resultsText = resultsText;
            _eventManager = eventManager;

            Subscribe();
        }

        private void Subscribe()
        {
            
        }
    }
}