using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private float moveHorizontal;
    private Rigidbody2D rigidPlayer;
    private float speed = 5.5f;
    private SpriteRenderer render;
    private float jumpForce = 6f;
    private bool isOnGround = true;
    private int points = 0;
    private int lifes = 2;
    public TextMeshProUGUI PointsText, LifeText, MaxPointsText;
    private AudioSource audioSourcePlayer;
    public AudioClip jumpAudioClip, diamondAudioClip, attackAudioClip, explosionAudioClip;
    public Camera cameraPlayer;
    public GameObject explosionPrefab;
    private Animator animator;
    public SpawnManager[] spawnManagerControllers;
    public GameObject panelGameOver;
    private int maxScore;


    // Start is called before the first frame update
    void Start()
    {
        rigidPlayer = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        audioSourcePlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        maxScore = PlayerPrefs.GetInt("MaxScore", 0);
        MaxPointsText.text = "Max: " + maxScore.ToString();

    }

    // Update is called once per frame
    void Update()
    {
       Movimiento();
       Saltar();
       Atacar();
    }

    private void Movimiento()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
       if(moveHorizontal < 0)
       {
        render.flipX = true;
       }
       if(moveHorizontal > 0)
       {
        render.flipX = false;
       }
       transform.position += new Vector3(moveHorizontal, 0) * Time.deltaTime * speed; 
       
    }

    private void Atacar () 
    {
        if(Input.GetKeyDown(KeyCode.Space) && isOnGround){
            audioSourcePlayer.PlayOneShot(attackAudioClip);
            animator.SetTrigger("attack");
        }
    }
    private void Saltar()
    {
        if(Input.GetKeyDown(KeyCode.UpArrow) && isOnGround){
            audioSourcePlayer.PlayOneShot(jumpAudioClip);
            rigidPlayer.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) 
{
    if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "MobilePlatform" || collision.gameObject.tag == "Chest") {
        isOnGround = true;
    }
    
    if (collision.gameObject.tag == "ZombieM") 
    {
        TakeDamage(1); 
    } 
    else if (collision.gameObject.tag == "ZombieF") 
    {
        TakeDamage(2); 
    }
}

private void TakeDamage(int damageAmount) {
    lifes -= damageAmount;
    lifes = Mathf.Max(lifes, 0); 
    Debug.Log("Lifes = " + lifes);
    LifeText.text = "Lifes: " + lifes.ToString();

    if (lifes <= 0) {
        Die();
    }
}

private void Die() {
    cameraPlayer.transform.parent = null;
    Instantiate(explosionPrefab, transform.position + new Vector3(0, 0.3f, 0), Quaternion.identity);
    for (int i = 0; i < spawnManagerControllers.Length; i++) {
        spawnManagerControllers[i].CancelarEnemigos();
    }
    panelGameOver.SetActive(true);
    Destroy(gameObject);
}

   private void OnCollisionExit2D(Collision2D collision) {
        if(collision.gameObject.tag == "Ground" || collision.gameObject.tag == "MobilePlatform" || collision.gameObject.tag == "Chest" ) {
        isOnGround = false;
        }
        if (collision.gameObject.tag == "ZombieM" || collision.gameObject.tag == "ZombieF") {
        isOnGround = true;
        } 
   }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.tag == "Diamond")
        {
            audioSourcePlayer.PlayOneShot(diamondAudioClip);
            points += 4;
            Debug.Log("Points = " + points);
            PointsText.text = "Points: " + points.ToString();

            if(points > maxScore)
            {
                maxScore = points;
                MaxPointsText.text = "Max: " + maxScore.ToString();
                PlayerPrefs.SetInt("MaxScore", maxScore);
            }

            Destroy(collision.gameObject);
        }
        if(collision.tag == "Mushroom")
        {            
            audioSourcePlayer.PlayOneShot(diamondAudioClip);
            points ++;
            Debug.Log("Points = " + points);
            PointsText.text = "Points: " + points.ToString();

            if(points > maxScore)
            {
                maxScore = points;
                MaxPointsText.text = "Max: " + maxScore.ToString();
                PlayerPrefs.SetInt("MaxScore", maxScore);
            }

            Destroy(collision.gameObject);
        }
         if(collision.tag == "Pumpkin")
        {
            audioSourcePlayer.PlayOneShot(diamondAudioClip);
            lifes ++;
            Debug.Log("Lifes = " + lifes);
            LifeText.text = "Lifes: " + lifes.ToString();
            Destroy(collision.gameObject);
        }
        if(collision.tag == "Key")
        {
            audioSourcePlayer.PlayOneShot(diamondAudioClip);
        }
         
    }
}

