using AIScripts;
using GameEnums;
using RelationMatrix;
using TimerScripts;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    [SerializeField] private SymmetricRelationMatrix matrix;
    [SerializeField] private RelationElement first;
    [SerializeField] private RelationElement second;

    private readonly AIBotBehaviour _bot = new();

    private TimerHandler _decisionTimer;

    private void Start()
    {
        _bot.Initialize(matrix);
        
        _decisionTimer = new TimerHandler(2f, OnTimerEnd);
        _decisionTimer.StartTimer();
    }

    private void OnTimerEnd()
    {
        Debug.Log("Your decision time has ended");
        _decisionTimer.ResetTimer();
        _decisionTimer.StartTimer();
    }

    private void DoComparison()
    {
        if (!Keyboard.current.spaceKey.wasPressedThisFrame) return;

        var botPick = second = _bot.ChooseMove();
        var myPick = first;
        var result = matrix.GetTrueRelation(first, botPick);
        
        Debug.Log("The result of "+first+" and "+botPick+" colliding is :"+result);
        
        //Do the aftermath
        _bot.ProcessPlayerMove(myPick);
        _bot.ProcessResult(botPick, myPick, result);
    }
    private void Update()
    {
        _decisionTimer.Tick(Time.deltaTime);
        DoComparison();
    }
}