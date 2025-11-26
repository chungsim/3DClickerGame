using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    public static PieceManager Instance { get; private set; }

    public PieceStateMachine[] pieceStateMachines;

    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void startPieceStateRoutine()
    {
        foreach(PieceStateMachine pieceStateMachine in pieceStateMachines)
        {
            pieceStateMachine.StartState();
        }
    }

    public void StopPieceStateRoutine()
    {
        foreach(PieceStateMachine pieceStateMachine in pieceStateMachines)
        {
            pieceStateMachine.StopState();
        }
    }
}
