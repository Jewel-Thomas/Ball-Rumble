using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Enemy : MonoBehaviour
{
    public float speed = 3.0f;
    public float blueSpeed = 15.0f;
    
    private Rigidbody enemyRb;

    private GameObject player;
    
    public bool isBoss;
    public float spawnInterval;
    public float nextSpawn;
    public int miniEnemySpawnCount;
    private SpawnManager spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
        if (isBoss)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        

        Vector3 lookDirection = (player.gameObject.transform.position - transform.position).normalized;
        if(!PauseMenu.gameIsPaused && transform.position.y > -4)
        {
            enemyRb.AddForce(lookDirection * speed);
        }
        

        if (isBoss)
        {
            if (Time.time > nextSpawn)
            {
                nextSpawn = Time.time + spawnInterval;
                spawnManager.SpawnMiniEnemy(miniEnemySpawnCount);
            }    
        }

        if (transform.position.y < -10)
        {
            //defText.text = "Enemies defeated : " + enemyCount;
            Destroy(gameObject);
        }
        
    }

    

    /*private void Collision (Collision collision)
    {
        if (gameObject.CompareTag("BlueBoss") && collision.gameObject.CompareTag("Player"))
        {
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            playerRb.AddForce(PushDirection() * blueSpeed, ForceMode.Impulse);
        }
    }*/
}
