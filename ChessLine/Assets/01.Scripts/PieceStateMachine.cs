using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class PieceStateMachine : MonoBehaviour
{
    Piece thisPiece;
    Coroutine curCoroutine;
    DiceData diceData;
    [SerializeField] private PieceType attackPieceType;
    [SerializeField] Image pieceIcon;
    [SerializeField] Image pieceIconCooldown;

    float targetSearchTime = 0.1f;
    void Start()
    {
        thisPiece = GetComponent<Piece>();
    }

    public void StartState()
    {
        curCoroutine = StartCoroutine(DiceState());
        
    }

    public void StopState()
    {
        StopCoroutine(curCoroutine);
    }

    IEnumerator DiceState()
    {
        Debug.Log("Dice State Start");

        if(thisPiece.diceData == null)
        {
            diceData = Resources.Load<DiceData>("NormalDice");
        }
        else
        {
            diceData = thisPiece.diceData;
        }

        attackPieceType = diceData.diceFaces[Random.Range(0, diceData.diceFaces.Length)];

        //주사위 굴리기 연출
        pieceIconCooldown.fillAmount = 0f;
        pieceIcon.sprite = Datas.piceeIconFair[attackPieceType];
        pieceIcon.color = Datas.pieceColorFair[attackPieceType];

        // 대기 상태로 이동 
        curCoroutine = StartCoroutine(IdleState());


        yield return null;
    }

    IEnumerator IdleState()
    {
        Debug.Log("Idle State Start");
        // 공격 주사위가 정해진 상태에서 범위에 적이 올때까지 대기
        while (!targetOn())
        {
            yield return new WaitForSeconds(targetSearchTime);
        }

        curCoroutine = StartCoroutine(AttackState());

        yield return null;
    }

    IEnumerator AttackState()
    {
        Debug.Log("Attack State Start");

        thisPiece.Attack(attackPieceType, thisPiece.atk);

        curCoroutine = StartCoroutine(CooldownState());

        yield return null;
    }

    IEnumerator CooldownState()
    {
        Debug.Log("Cooldown State Start");       

        float t = 0;
        while(t < thisPiece.cooldown)
        {
            t += Time.fixedDeltaTime;

            pieceIconCooldown.fillAmount = (thisPiece.cooldown - t)  / thisPiece.cooldown;

            yield return new WaitForFixedUpdate();
        }

        curCoroutine = StartCoroutine(DiceState());
        yield return null;
    }

    bool targetOn()
    {
        Vector3 targetVector = new Vector3(0, 10, 0);
        RaycastHit hit;
        Monster monster;
        for(int i = 0; i < Datas.pieceVectors[attackPieceType].Vectors.Length; i++)
        {
            for(int count = 0; count < Datas.pieceVectors[attackPieceType].Count; count++)
            {
                targetVector.x =  transform.position.x + Datas.pieceVectors[attackPieceType].Vectors[i].x * (count + 1);
                targetVector.z =  transform.position.z + Datas.pieceVectors[attackPieceType].Vectors[i].y * (count + 1);
                Ray ray = new Ray(targetVector, Vector3.down * 10f);
                if(Physics.Raycast(ray, out hit))
                {
                    if(hit.collider.TryGetComponent<Monster>(out monster))
                    {
                        if(monster.curHp > 0)
                        {

                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

}
