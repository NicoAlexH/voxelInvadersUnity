using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerCharacter : MonoBehaviour
{
    public float movementSpeed;
    public MapLimits Limits;

    public GameObject particleEffect;
    public GameObject bullet;
    public Transform pos1;
    public Transform posL;
    public Transform posR;

    public int hp;

    public AudioClip shotSound;
    public float shotPower;
    AudioSource audioS;
    int power;

    public Text scoreText;
    public int score;
    public Text highScoreText;
    int highScore;


    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("highScore"))
        {
            highScore = 0;
            PlayerPrefs.SetInt("highScore", highScore);
        }
        power = 1;
        audioS = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Shooting(); 
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, Limits.minimumX, Limits.maximumX),
            Mathf.Clamp(transform.position.y, Limits.minimumY, Limits.maximumY), 0.0f);
        if (hp <= 0)
        {
            Instantiate(particleEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }

        //Updating score / highscore
        scoreText.text = score.ToString();
        highScoreText.text = PlayerPrefs.GetInt("highScore").ToString();
        if(score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("highScore", highScore);
        }

    }

    void Movement()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(Vector3.right * -movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.up * -movementSpeed * Time.deltaTime);
        }
    }

    void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioS.PlayOneShot(shotSound);
            switch (power)
            {
                case 1:
                    {
                        GameObject newBullet = Instantiate(bullet, pos1.position, transform.rotation);
                        newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                    }
                    break;
                case 2:
                    {
                        GameObject bullet1 = Instantiate(bullet, posL.position, transform.rotation);
                        bullet1.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                        GameObject bullet2 = Instantiate(bullet, posR.position, transform.rotation);
                        bullet2.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                    }
                    break;
                case 3: {
                        GameObject bullet1 = Instantiate(bullet, posL.position, transform.rotation);
                        bullet1.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                        GameObject bullet2 = Instantiate(bullet, posR.position, transform.rotation);
                        bullet2.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                        GameObject bullet3 = Instantiate(bullet, pos1.position, transform.rotation);
                        bullet3.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                    }
                    break;
                default: {
                        GameObject newBullet = Instantiate(bullet, pos1.position, transform.rotation);
                        newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * shotPower;
                    } break;
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "powerUp")
        {
            if(power < 3)
            {
                ++power;
            }
            Destroy(col.gameObject);
        }
        if(col.gameObject.tag == "powerDown")
        {
            if(power > 1)
            {
                --power;
            }
            Destroy(col.gameObject);
        }
        if (col.gameObject.tag == "enemyBullet")
        {
            //Destroying the bullet
            Destroy(col.gameObject);
            Instantiate(particleEffect, transform.position, transform.rotation);
            --hp;
        }
    }
}
