using AIScripts;
using GameEnums;
using RelationMatrix;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    [SerializeField] private SymmetricRelationMatrix matrix;
    [SerializeField] private RelationElement first;
    [SerializeField] private RelationElement second;

    private readonly AIBotBehaviour _bot = new();

    private void Start()
    {
        _bot.Initialize(matrix);
    }

    private void DoComparison()
    {
        if (!Keyboard.current.spaceKey.wasPressedThisFrame) return;

        var botPick = second = _bot.ChooseMove();
        var myPick = first;
        var result = matrix.GetTrueRelation(first, botPick);
        
        Debug.Log("The result of "+first+" and "+botPick+" colliding is :"+result);
        _bot.ProcessPlayerMove(myPick);
    }
    private void Update()
    {
        DoComparison();
    }
}