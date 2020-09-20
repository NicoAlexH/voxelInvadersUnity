using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public MapLimits Limits;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;

    public GameObject background;

    public float spawnTimer = 2;

    // Start is called before the first frame update
    void Start()
    {
        spawnEnemy();
        addBackground();
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0)
        {
            spawnEnemy();
        }


    }

    void spawnEnemy()
    {
        int i = UnityEngine.Random.Range(1, 10);        
        if(i < 5)
        {
            Instantiate(enemy1, new Vector3(UnityEngine.Random.Range(Limits.minimumX, Limits.maximumX), UnityEngine.Random.Range(Limits.minimumY, Limits.maximumY), -0.5f), enemy1.transform.rotation);
            spawnTimer = 2;
        } else if (i > 5 && i < 9)
        {
            Instantiate(enemy2, new Vector3(UnityEngine.Random.Range(Limits.minimumX, Limits.maximumX), UnityEngine.Random.Range(Limits.minimumY, Limits.maximumY), -0.5f), enemy2.transform.rotation);
            spawnTimer = 4;
        } else
        {
            Instantiate(enemy3, new Vector3(UnityEngine.Random.Range(Limits.minimumX, Limits.maximumX), UnityEngine.Random.Range(Limits.minimumY, Limits.maximumY), -0.5f), enemy3.transform.rotation);
            spawnTimer = 10;
        }

    }

    private void addBackground()
    {
        Instantiate(background, new Vector3(0, 130, 10), background.transform.rotation);
        var rig = background.GetComponent<Rigidbody>();
        rig.velocity.Set(0, 5, 0);
    }
}
