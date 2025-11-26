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

    public static Dictionary<PieceType, Color> pieceColorFair = 
    new Dictionary<PieceType, Color>
    {
        {PieceType.Pawn, new Color(255f / 255f, 255f / 255f, 255f / 255f)},
        {PieceType.Rook, new Color(100f / 255f, 100f / 255f, 100f / 255f)},
        {PieceType.Knight, new Color(135f / 255f, 169f / 255f, 255f / 255f)},
        {PieceType.Bishop, new Color(255f / 255f, 143f / 255f, 143f / 255f)},
        {PieceType.Queen, new Color(175f / 255f, 71f / 255f, 255f / 255f)},
        {PieceType.King, new Color(255f / 255f, 215f / 255f, 0f / 255f)},
    };

    public static Dictionary<PieceType, Sprite> piceeIconFair =
    new Dictionary<PieceType, Sprite>
    {
        {PieceType.Pawn, Resources.Load<Sprite>("chess_pawn_white")},
        {PieceType.Rook, Resources.Load<Sprite>("chess_rook_white")},
        {PieceType.Knight, Resources.Load<Sprite>("chess_knight_white")},
        {PieceType.Bishop, Resources.Load<Sprite>("chess_bishop_white")},
        {PieceType.Queen, Resources.Load<Sprite>("chess_queen_white")},
        {PieceType.King, Resources.Load<Sprite>("chess_king_white")}

    };

    public static Dictionary<int, int> levelExpFair = new Dictionary<int, int>
    {
        
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
