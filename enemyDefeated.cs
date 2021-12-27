using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyDefeated : MonoBehaviour
{
    private GameObject[] enemy;
    private PlayerController playerController;
    private GameObject redEnemy;
    public TextMeshProUGUI defText;
    private int enemyCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        enemy = GameObject.FindGameObjectsWithTag("Enemy");
        redEnemy = GameObject.FindGameObjectWithTag("RedBoss");
        foreach(GameObject e in enemy)
        {
            if(e.transform.position.y < -7 && !playerController.gameOver)
            {
                enemyCount = enemyCount + 1;
                defText.text = "Enemy Defeated : " + enemyCount;
                Destroy(e);
            }
        }
        if(GameObject.FindGameObjectWithTag("RedBoss"))
        {
            if(redEnemy.transform.position.y < -10 && !playerController.gameOver)
            {
                enemyCount = enemyCount + 1;
                defText.text = "Enemy Defeated : " + enemyCount;
                Destroy(redEnemy.gameObject);
            }
            
        }
         
            
        
        
    }
}
