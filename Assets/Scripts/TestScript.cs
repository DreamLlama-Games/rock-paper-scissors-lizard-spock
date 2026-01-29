using GameEnums;
using RelationMatrix;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestScript : MonoBehaviour
{
    [SerializeField] private SymmetricRelationMatrix matrix;
    [SerializeField] private RelationElement first;
    [SerializeField] private RelationElement second;

    private void DoComparison()
    {
        if (!Keyboard.current.spaceKey.wasPressedThisFrame) return;
        
        var result = matrix.GetTrueRelation(first, second);
        Debug.Log("The result of "+first+" and "+second+" colliding is :"+result);
    }
    private void Update()
    {
        DoComparison();
    }
}