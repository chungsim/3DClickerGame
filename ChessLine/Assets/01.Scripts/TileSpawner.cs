using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TileSpawner : MonoBehaviour
{

    public static TileSpawner Instance { get; private set; }

    [SerializeField] private GameObject tilePrefeb;
    [SerializeField] private Material[] tileMaterials;
    [SerializeField] private List<GameObject> spwanedTiles;


    void Awake()
    {
        // 싱글톤
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            spawnNewMap();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DespawnMap();
        }
    }

    public void spawnNewMap()
    {
        if(spwanedTiles.Count > 0) return;

        for(int ver = 0; ver < 7; ver++)
        {
            for(int hor = 0; hor < 8; hor++)
            {
                GameObject go = Instantiate(tilePrefeb, transform);
                spwanedTiles.Add(go);
                go.transform.position = new Vector3(hor, -5f, ver + 1);
                if(ver % 2 == 0)
                {
                    if(hor % 2 == 1)
                    {
                        go.GetComponent<Renderer>().material = tileMaterials[0];
                    }
                    else
                    {
                        go.GetComponent<Renderer>().material = tileMaterials[1];
                    }
                }
                else
                {
                    if(hor % 2 == 0)
                    {
                        go.GetComponent<Renderer>().material = tileMaterials[0];
                    }
                    else
                    {
                        go.GetComponent<Renderer>().material = tileMaterials[1];
                    }
                }

                StartCoroutine(RaiseTile(go, -0.1f, (hor + ver * 8) * 0.025f));
            }
        }
    }

    public void DespawnMap()
    {
        if(spwanedTiles.Count > 0)
        {
            for(int i = spwanedTiles.Count - 1; i >= 0; i--)
            {
                StartCoroutine(DropTile(spwanedTiles[i], -5f, (spwanedTiles.Count - i) * 0.025f));
            }

            spwanedTiles.Clear();
        }
    }

    IEnumerator RaiseTile(GameObject tile, float targetY ,float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 targetPos = new Vector3(tile.transform.localPosition.x, targetY, tile.transform.localPosition.z);
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 1.5f;
            tile.transform.localPosition = Vector3.Lerp(tile.transform.localPosition, targetPos, t);
            yield return null;
        }
    }

    IEnumerator DropTile(GameObject tile, float targetY ,float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 targetPos = new Vector3(tile.transform.localPosition.x, targetY, tile.transform.localPosition.z);
        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 1.5f;
            tile.transform.localPosition = Vector3.Lerp(targetPos, tile.transform.localPosition, t);
            yield return null;
        }

        Destroy(tile);
    }

    public void RefreshAllTile()
    {
        foreach(GameObject tile in spwanedTiles)
        {
            Tile tileData = tile.GetComponent<Tile>();
            tileData.isMonsterOn = false;
            tileData.character = null;
        }
    }
}
