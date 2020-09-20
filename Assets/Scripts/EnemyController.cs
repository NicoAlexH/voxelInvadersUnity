using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public float changeTimer;
        
    public GameObject particleEffect;
    public MapLimits Limits;

    public int hp;
    public bool directionSwitch;

    public bool canShoot;
    public Transform shootingPosition;
    public GameObject bullet;
    float shootTimer;
    public float shotPower;

    public GameObject powerUp;
    public GameObject powerDown;

    Rigidbody rig;

    public int scoreReward;

    // Start is called before the first frame update
    void Start()
    {
        shootTimer = 1;
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switchTimer();
        Movement();
        if (transform.position.x == Limits.maximumX || transform.position.x == Limits.minimumX)
            directionSwitch = !directionSwitch;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, Limits.minimumX, Limits.maximumX),
            Mathf.Clamp(transform.position.y, Limits.minimumY, Limits.maximumY), -0.5f);

        //Decrease to 0 over time
        shootTimer -= Time.deltaTime;
        if(canShoot && shootTimer <= 0)
        {
            shoot();            
        }
    }

    void Movement()
    {
        if (directionSwitch)
        {
            rig.velocity = new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0);
        } else
        {
            rig.velocity = new Vector3(-speed * Time.deltaTime, -speed * Time.deltaTime, 0);
        }
    }

    void switchTimer()
    {
        changeTimer -= Time.deltaTime;
        if (changeTimer < 0)
        {
            directionSwitch = !directionSwitch;
            changeTimer = Random.Range(1,3);
        }
    }

    private void OnTriggerEnter(Collider col)
    {        
        if (col.gameObject.tag == "Enemy")
        {
            //Decreasing enemy hp
            --col.gameObject.GetComponent<EnemyController>().hp;
            Instantiate(particleEffect, transform.position, transform.rotation);
            --hp;
        }
        else if (col.gameObject.tag == "enemyBullet" || col.gameObject.tag == "friendlyBullet")
        {
            //Destroying the bullet
            Destroy(col.gameObject);
            Instantiate(particleEffect, transform.position, transform.rotation);
            --hp;
        }
        //collision with the player
        else if(col.gameObject.tag == "Player")
        {
            --col.gameObject.GetComponent<PlayerCharacter>().hp;
            --hp;
        }


        if (hp <= 0)
        {
            int i = Random.Range(1, 3);
            if(i < 2)
            {
                Instantiate(powerUp, transform.position, powerUp.transform.rotation);
            } else
            {
                Instantiate(powerDown, transform.position, powerDown.transform.rotation);
            }

            Instantiate(particleEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>().score += scoreReward;
        }
    }

    private void shoot()
    {
        GameObject newBullet = Instantiate(bullet, shootingPosition.position, shootingPosition.rotation);
        newBullet.GetComponent<Rigidbody>().velocity = Vector3.up * -shotPower;
        shootTimer = Random.Range(0, 3);
    }
    

}
