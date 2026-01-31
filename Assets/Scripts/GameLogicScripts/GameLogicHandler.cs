using AIScripts;
using EventManagerScripts;
using GameEnums;
using RelationMatrix;

namespace GameLogicScripts
{
    public class GameLogicHandler
    {
        private SymmetricRelationMatrix _matrix;
        private readonly AIBotBehaviour _bot = new();
        
        private int _playerScore;

        private RelationElement _botMove;
        private RelationElement _playerMove;
        
        private GameEventManager _eventManager;

        public void Initialize(GameEventManager gameEventManager)
        {
            _eventManager = gameEventManager;
            Subscribe();
        }
        
        private void Subscribe()
        {

        }
        
        private void GameStart()
        {
            SetPlayerScore(0);
            _bot.Initialize(_matrix);
        }

        private void SetPlayerScore(int playerScore)
        {
            _playerScore = playerScore;    
        }

        private void SetPlayerMove(RelationElement move)
        {
            _playerMove = move;
        }

        private void SetBotMove(RelationElement move)
        {
            _botMove = move;
        }

        private int GetResult()
        {
            return _matrix.GetTrueRelation(_playerMove, _botMove);
        }

        private void PerformMoveAftermath(int result)
        {
            _bot.ProcessPlayerMove(_playerMove);
            _bot.ProcessResult(_botMove, _playerMove, result);
        }
    }
}
