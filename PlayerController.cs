using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public float speed = 5.0f;
    private float forwardInput;
    private float powerUpStrength = 15.0f;
    private float redBossStrength = 5.0f;
    public bool hasPowerUp = false;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public GameObject powerUpIndicator;
    public GameObject gameOverText;
    public AudioClip rocketAudio;
    public AudioClip smashAudio;
    private AudioSource audioSource;
    public GameObject restartButton;
    public bool gameOver = false;
    public PowerUpType currentPowerUp = PowerUpType.None;
    public GameObject rocketPrefab;
    private GameObject tmpRocket;
    public TextMeshProUGUI timeSurvived;
    private Coroutine powerUpCountDown;
    public float hangTime;
    public float smashSpeed;
    private float explosionForce = 75.0f;
    public float explosionRadius;
    public float timeClock;
    bool smashing = false;
    bool isPlaying = true;
    float floorY;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        focalPoint = GameObject.Find("Focal Point");
    }

    // Update is called once per frame
    void Update()
    {
        forwardInput = Input.GetAxis("Vertical");
        if(!PauseMenu.gameIsPaused)
        {
            playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);
        }
        
        powerUpIndicator.gameObject.transform.position = transform.position + new Vector3 (0, -0.5f, 0);
        if (transform.position.y < -5 || gameOver)
        {
            gameOver = true;
            gameOverText.gameObject.SetActive(true);
            restartButton.gameObject.SetActive(true);
        }
        if (PauseMenu.gameIsPaused)
        {
            gameOverText.gameObject.SetActive(false);
            restartButton.gameObject.SetActive(false);
        }

        if (!gameOver)
        {
            Timer();
        }

        if (currentPowerUp == PowerUpType.Rockets && Input.GetKeyDown(KeyCode.F))
        {
            LaunchRockets();
            audioSource.PlayOneShot(rocketAudio, 1.0f);

        }

        if (currentPowerUp == PowerUpType.Smash && Input.GetKeyDown(KeyCode.Space) && !smashing)
        {
            isPlaying = true;
            smashing = true;
            StartCoroutine(Smash());
            
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && !gameOver && Onground())
        {
            playerRb.AddForce(Vector3.up*9,ForceMode.Impulse);
        }
    }

    private bool Onground()
    {
        if(transform.position.y > 0.5f)
        {
            return false;
        }
        else
        {
            return true;
        }   
    }

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            currentPowerUp = other.gameObject.GetComponent<Powerup>().powerUpType;
            powerUpIndicator.gameObject.SetActive(true);
            //transform.localScale = new Vector3(3,3,3);
            //transform.position = new Vector3(transform.position.x, 0.8f, transform.position.z);
            //playerRb.mass = 15;
            Destroy(other.gameObject);
            if (powerUpCountDown != null)
            {
                StopCoroutine(powerUpCountDown);
            }
            StartCoroutine(PowerUpCountDownRoutine());
        }
    }

    IEnumerator PowerUpCountDownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerUp = false;
        currentPowerUp = PowerUpType.None;
        powerUpIndicator.gameObject.SetActive(false);
        //transform.localScale = new Vector3(1.5f,1.5f,1.5f);
        //transform.position = new Vector3(transform.position.x, 0.09f, transform.position.z);
        //playerRb.mass = 1;
    }

    IEnumerator Smash()
    {
        var enemies = FindObjectsOfType<Enemy>();
        floorY = transform.position.y;

        float jumpTime = Time.time + hangTime;
        while (Time.time < jumpTime)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, smashSpeed);
            yield return null;
        }
        
        while (transform.position.y > floorY)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, -smashSpeed * 2);
            yield return null;
            if (isPlaying)
            {
                audioSource.PlayOneShot(smashAudio, 1.5f);
                isPlaying = false;
            }
        }
        

        for (int i = 0; i < enemies.Length; i++)
        {
            if (enemies[i] != null)
            {
                enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRadius, 0.0f, ForceMode.Impulse);   
            }
        }
        smashing = false;
    }

    private void OnCollisionEnter (Collision collision)
    {
        if ((collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("RedBoss")) && currentPowerUp == PowerUpType.PushBack)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            Debug.Log("Collided with: " + collision.gameObject.name + " with powerup set to " + currentPowerUp.ToString());
            enemyRigidbody.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("RedBoss"))
        {
            //Rigidbody playerRigidbody = gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = transform.position - collision.gameObject.transform.position;

            
            playerRb.AddForce(awayFromPlayer * redBossStrength, ForceMode.Impulse);
        }
        
    }

    void LaunchRockets()
    {
        foreach(var enemy in FindObjectsOfType<Enemy>())
        {
            tmpRocket = Instantiate(rocketPrefab, transform.position+ Vector3.up,Quaternion.identity);
            tmpRocket.GetComponent<RocketBehaviour>().Fire(enemy.transform); 
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Timer()
    {
        timeClock += Time.deltaTime;
        timeSurvived.text = "Time Survived: " + Mathf.Round(timeClock) + " s";
    }
}
