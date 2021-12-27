using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject[] powerUpPrefabs;
    public GameObject yellowBall;
    public GameObject blueBall;
    public GameObject redBall;
    private PlayerController playerController;
    private float spawnRange = 9.0f;
    public int enemyCount;
    public int waveNumber = 1;
    public int refNumber = 1;
    public GameObject[] bossPrefab;
    public GameObject[] miniEnemyPrefabs;
    public int bossRound;
    private int index;
    
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        SpawnEnemyWave(waveNumber);
        int randomPowerUp = Random.Range(0, powerUpPrefabs.Length);
        Instantiate(powerUpPrefabs[randomPowerUp], new Vector3(Random.Range(-spawnRange, spawnRange), 0.5f, Random.Range(-spawnRange, spawnRange)), powerUpPrefabs[randomPowerUp].transform.rotation);
        
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;
        
        if (enemyCount == 0)
        {
            waveNumber++;
            refNumber++;
            if (!playerController.gameOver)
            {
                if (waveNumber % bossRound == 0)
                {
                    index = Random.Range(0, bossPrefab.Length);
                    SpawnBossWave(waveNumber);
                }
                else
                {
                    SpawnEnemyWave(waveNumber); 
                }
                
                int randomPowerUp = Random.Range(0, powerUpPrefabs.Length);
                Instantiate(powerUpPrefabs[randomPowerUp], new Vector3(Random.Range(-spawnRange, spawnRange), 0.5f, Random.Range(-spawnRange, spawnRange)), powerUpPrefabs[randomPowerUp].transform.rotation);
            }

        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        if (!playerController.gameOver)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Instantiate(enemyPrefab, GenerateSpawnPosition()  , enemyPrefab.transform.rotation);
            }
            
        }
    }

    void SpawnBossWave(int currentRound)
    {
        int miniEnemysToSpawn;
        //We dont want to divide by 0!
        if (bossRound != 0)
        {
            miniEnemysToSpawn = currentRound / bossRound;
        }
        else
        {
            miniEnemysToSpawn = 1;
        }
        var boss = Instantiate (bossPrefab[index], GenerateSpawnPosition(), bossPrefab[index].transform.rotation);
        boss.GetComponent<Enemy>().miniEnemySpawnCount = miniEnemysToSpawn;      
    }

    public void SpawnMiniEnemy(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int randomMini = Random.Range(0, miniEnemyPrefabs.Length);
            Instantiate (miniEnemyPrefabs[randomMini], GenerateSpawnPosition(), miniEnemyPrefabs[randomMini].transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomSpawn = new Vector3 (spawnPosX,0,spawnPosZ);

        return randomSpawn;
    }
}
