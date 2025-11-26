using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamageable
{
    void LoseHp(int value);
    void RestoreHp(int value);
}

interface IAttackable
{
    void Attack(PieceType pieceType, int value);

}

public class Character : MonoBehaviour, IDamageable, IAttackable
{
    [Header("Status")]
    [SerializeField] private string nameString;

    public int maxHP;
    public int curHp;

    public int atk;
    public int def;

    public float cooldown;


    public virtual void LoseHp(int value)
    {
        int cul = curHp - value + def;
        curHp = cul > 0 ? cul : 0; 
    }

    public void RestoreHp(int value)
    {
        int cul = curHp + value;
        curHp = cul < maxHP ? cul : maxHP; 
    }

    public void Attack(PieceType pieceType, int value)
    {
        RaycastHit hit;
        Vector3 targetVector = new Vector3(0,-10,0);
        Tile tile;
        for(int i = 0; i < Datas.pieceVectors[pieceType].Vectors.Length; i++)
        {
            for(int count = 0; count < Datas.pieceVectors[pieceType].Count; count++)
            {
                targetVector.x =  transform.position.x + Datas.pieceVectors[pieceType].Vectors[i].x * (count + 1);
                targetVector.z =  transform.position.z + Datas.pieceVectors[pieceType].Vectors[i].y * (count + 1);
                Ray ray = new Ray(targetVector, Vector3.up * 11f);
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.TryGetComponent<Tile>(out tile))
                    {
                        if(tile.character != null )
                        {
                            //tile.character.gameObject.GetComponent<Monster>().TakeDamage(value);
                            tile.character.LoseHp(value);
                        }
                    }
                    ParticleManager.Instance.SpawnAttackParticle(targetVector - Vector3.down * 10, pieceType);
                }                
            }
        }  
    }
}
