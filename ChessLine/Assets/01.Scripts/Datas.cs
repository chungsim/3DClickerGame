using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PieceType {Pawn, Rook, Knight, Bishop, Queen, King}
public enum TileType {Normal}
public enum SoundType{Bgm, Sfx}


public static class Datas
{
    public static Dictionary<PieceType, PieceVector> pieceVectors = 
    new Dictionary<PieceType, PieceVector>
    {
        { PieceType.Pawn, 
        new PieceVector(new Vector2[]
        {
           new Vector2(-1, 1),
           new Vector2(1, 1) 
        }, 1)
        },
        { PieceType.Rook, 
        new PieceVector(new Vector2[]
        {
           new Vector2(0, 1) 
        }, 8)
        },
        { PieceType.Knight, 
        new PieceVector(new Vector2[]
        {
            new Vector2(-1, 2),
            new Vector2(1, 2),
            new Vector2(-2, 1),
            new Vector2(2, 1) 
        }, 1)
        },
        { PieceType.Bishop, 
        new PieceVector(new Vector2[]
        {
            new Vector2(-1, 1),
            new Vector2(1, 1) 
        }, 8)
        },
        { PieceType.Queen, 
        new PieceVector(new Vector2[]
        {
            new Vector2(-1, 1),
            new Vector2(0, 1),
            new Vector2(1,1) 
        }, 8)
        },
        { PieceType.King, 
        new PieceVector(new Vector2[]
        {
            new Vector2(-1, 1),
            new Vector2(0, 1),
            new Vector2(1,1) 
        }, 1)
        }
    };   
}

public class PieceVector
{
    public Vector2[] Vectors{get; set;}
    public int Count{get; set;}

    public PieceVector(Vector2[] vectors, int count)
    {
        Count = count;
        Vectors = vectors;
    }
}
