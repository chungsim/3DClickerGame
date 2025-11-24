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
    private string nameString;

    public int maxHP;
    public int curHp;


    public void LoseHp(int value)
    {
        curHp = (curHp - value) > 0 ? curHp - value : 0; 
    }

    public void RestoreHp(int value)
    {
        curHp = (curHp + value) < maxHP ? curHp + value : maxHP; 
    }

    public void Attack(PieceType pieceType, int value)
    {
        RaycastHit hit;
        Vector3 targetVector = new Vector3(0,10,0);
        IDamageable damageable;
        for(int i = 0; i < Datas.pieceVectors[pieceType].Vectors.Length; i++)
        {
            for(int count = 0; count < Datas.pieceVectors[pieceType].Count; count++)
            {
                targetVector.x =  transform.position.x + Datas.pieceVectors[pieceType].Vectors[i].x * (count + 1);
                targetVector.z =  transform.position.z + Datas.pieceVectors[pieceType].Vectors[i].y * (count + 1);
                Ray ray = new Ray(targetVector, Vector3.down * 9f);
                if(Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.TryGetComponent<IDamageable>(out damageable))
                    {
                        damageable.LoseHp(value);
                        Debug.Log(targetVector);
                        Debug.Log($"{name} Attack {hit.collider.name}");
                    }
                }
            }
        }  
    }
}
