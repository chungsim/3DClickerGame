using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager Instance;

    [SerializeField] private GameObject attackParticlePrefab;
    [SerializeField] private float effectLifeTime;

    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SpawnAttackParticle(Vector3 pos, PieceType pieceType)
    {    
        GameObject obj = Instantiate(attackParticlePrefab, pos, Quaternion.identity, transform);
        obj.transform.rotation = Quaternion.Euler(-90f, 0f, 0f);
        var main = attackParticlePrefab.GetComponent<ParticleSystem>().main;
        main.startColor = Datas.pieceColorFair[pieceType];

        Destroy(obj, effectLifeTime); // 2초 뒤 삭제
    }
}
