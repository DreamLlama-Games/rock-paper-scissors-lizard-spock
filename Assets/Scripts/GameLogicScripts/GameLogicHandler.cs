using AIScripts;
using EventManagerScripts;
using GameEnums;
using RelationMatrix;
using TimerScripts;

namespace GameLogicScripts
{
    public class GameLogicHandler
    {
        private SymmetricRelationMatrix _matrix;
        private readonly AIBotBehaviour _bot = new();
        
        private int _playerScore;
        private const string MatchTied = "It's a tie!";

        private RelationElement _botMove;
        private RelationElement _playerMove;

        private TimerHandler _newRoundTimer;
        private const float RoundResetDuration = 1.3f;

        private GameEventManager _eventManager;

        public void Initialize(GameEventManager gameEventManager, SymmetricRelationMatrix matrix)
        {
            _matrix = matrix;
            _bot.Initialize(matrix);
            _newRoundTimer = new TimerHandler(RoundResetDuration, StartNewRound);
            _eventManager = gameEventManager;
            Subscribe();
        }
        
        private void Subscribe()
        {
            _eventManager?.Subscribe<RelationElement>(GameEvent.OptionSelected, OnPlayerMoveSelect);
            _eventManager?.Subscribe(GameEvent.FetchResults, OnFetchResults);
        }

        private void OnFetchResults()
        {
            var result = GetResult();
            var winnerName = result switch
            {
                1 => _playerMove.ToString(),
                -1 => _botMove.ToString(),
                _ => MatchTied
            };
            var outcomeEventName = result switch
            {
                1 => GameEvent.PlayerWon,
                -1 => GameEvent.PlayerLost,
                _ => GameEvent.MatchTied
            };
            _eventManager?.Raise(outcomeEventName, this, winnerName);

            if (result == -1) return;
            
            SetPlayerScore(_playerScore + result);
            if(result > 0) 
                _eventManager?.Raise(GameEvent.ScoreUpdated, this, _playerScore);
            
            _newRoundTimer.ResetTimer();
            _newRoundTimer.StartTimer();
        }

        private void StartNewRound()
        { 
            _eventManager?.Raise(GameEvent.NewRoundStarted, this);
        }

        private void OnPlayerMoveSelect(RelationElement move)
        {
            SetPlayerMove(move);
            SetBotMove(_bot.ChooseMove());
            _eventManager?.Raise(GameEvent.BotOptionSelected, this, _botMove);
            _eventManager?.Raise(GameEvent.BobMotion, this);
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

        public void Tick(float dt)
        {
            _newRoundTimer.Tick(dt);
        }
    }
}
