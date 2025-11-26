using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : Character
{
    //private Dice curDice;

    public DiceData diceData;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Attack(PieceType.Bishop, 5);
        }
    }

}
