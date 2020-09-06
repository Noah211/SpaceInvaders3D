using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class Player : MonoBehaviour, IPlayer
{
    public GameObject ExplosionEffect;
    public bool Dead { get; private set; }

    private GameObject playerProjectilePrefab;
    private GameObject switchScenesPrefab;
    private AudioSource playerDeath;
    private AudioSource shoot;
    private float respawnTime;
    private float respawnTimer;
    private int lives;

    public void Start()
    {
        playerProjectilePrefab = Resources.Load("Prefabs/Projectiles/PlayerProjectile", typeof(GameObject)) as GameObject;
        switchScenesPrefab = Resources.Load("Prefabs/SwitchScenes", typeof(GameObject)) as GameObject;
        playerDeath = gameObject.transform.Find("PlayerDeathAudio").GetComponent<AudioSource>();
        shoot = gameObject.transform.Find("ShootAudio").GetComponent<AudioSource>();
        respawnTime = 0;
        respawnTimer = 3;
        lives = 3;
        Dead = false;
        transform.position = Vector3.zero;
    }

    public void Update()
    {
        respawnTime += Time.deltaTime;

        if (!Dead)
        {
            float horizontalMovement = Input.GetAxis("Horizontal") * 30.0f;
            transform.Translate(horizontalMovement * Time.deltaTime, 0, 0);
            float xPosition = Mathf.Clamp(transform.position.x, -90, 90);
            transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);
            respawnTime = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Shoot();
            }
        }
        else if ((respawnTime > respawnTimer) && (lives > 0))
        {
            transform.position = Vector3.zero;
            GetComponent<Renderer>().enabled = true;
            Dead = false;
            GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().PauseEnemies(false);
            respawnTime = 0;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }

    public void Die()
    {
        lives--;
        GameObject.Find("GameUIUpdater").GetComponent<GameUIUpdater>().UpdateLives(lives);
        Dead = true;
        playerDeath.Play();
        GetComponent<Renderer>().enabled = false;
        GameObject explosion = Instantiate(ExplosionEffect, transform.position, transform.rotation);
        explosion.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        Destroy(explosion, 2);

        if (lives > 0)
        {
            GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().PauseEnemies(true);
        }
        else
        {
            Instantiate(switchScenesPrefab);
            /*
            GameObject.Find("SwitchCameras").GetComponent<SwitchCameras>().UpdateFirstCamera();
            Destroy(gameObject);
            */
        }
    }

    public void Shoot()
    {
        GameObject[] playerProjectiles = GameObject.FindGameObjectsWithTag("PlayerProjectile");

        if (playerProjectiles.Length == 0)
        {
            shoot.Play();
            Instantiate(playerProjectilePrefab, transform.position, Quaternion.Euler(0, 0, 0));
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!Dead && (lives > 0))
        {
            if (other.gameObject.CompareTag("EnemyProjectileCollider"))
            {
                Die();
            }
            else if (other.gameObject.CompareTag("EnemyCollider"))
            {
                lives = 1;
                Die();
            }
        }
    }
}
