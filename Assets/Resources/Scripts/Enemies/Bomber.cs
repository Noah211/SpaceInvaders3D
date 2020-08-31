using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Author: Noah Logan

public class Bomber : MonoBehaviour, IEnemy
{
    public GameObject ExplosionEffect;

    private GameObject enemyProjectilePrefab;
    private GameObject switchScenesPrefab;
    private Vector3 speed;
    private int pointValue;
    private string currentDirection;
    private string prevDirection;
    private bool dead;

    public void Start()
    {
        enemyProjectilePrefab = Resources.Load("Prefabs/Projectiles/EnemyProjectile", typeof(GameObject)) as GameObject;
        switchScenesPrefab = Resources.Load("Prefabs/SwitchScenes", typeof(GameObject)) as GameObject;
        pointValue = 30;
        currentDirection = "NegativeX";
        prevDirection = currentDirection;
        dead = false;
    }

    public void Update()
    {
        // Don't allow enemies to go off-screen
        if ((transform.position.x >= 90) && (currentDirection == "PositiveX"))
        {
            GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().ChangeEnemyDirection("NegativeX");
        }
        else if ((transform.position.x <= -90) && (currentDirection == "NegativeX"))
        {
            GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>().ChangeEnemyDirection("PositiveX");
        }

        // Move
        if ((currentDirection == "PositiveX") && (prevDirection == "PositiveX"))
        {
            transform.Translate(speed.x * Time.deltaTime, speed.y * Time.deltaTime, speed.z * Time.deltaTime);
        }
        else if ((currentDirection == "NegativeX") && (prevDirection == "NegativeX"))
        {
            transform.Translate(-speed.x * Time.deltaTime, speed.y * Time.deltaTime, speed.z * Time.deltaTime);
        }

        prevDirection = currentDirection;

        // If enemy has passed player position
        if (transform.position.z > 10)
        {
            Instantiate(switchScenesPrefab);
            GameObject.Find("GameUIUpdater").GetComponent<GameUIUpdater>().DisplayGameOver();
        }
    }

    public void ChangeDirection(string direction)
    {
        currentDirection = direction;
    }

    public void ChangeSpeed(Vector3 speed)
    {
        this.speed = speed;
    }

    public void Die()
    {
        EnemyManager enemyManager = GameObject.FindGameObjectWithTag("EnemyManager").GetComponent<EnemyManager>();
        enemyManager.PlayinvaderKilled();

        if (enemyManager.Row4.Contains(gameObject))
        {
            enemyManager.Row4.Remove(gameObject);
        }
        GameObject.Find("GameUIUpdater").GetComponent<GameUIUpdater>().UpdateScore(pointValue);
        GameObject explosion = Instantiate(ExplosionEffect, transform.position, transform.rotation);
        explosion.transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
        Destroy(explosion, 2);
        Destroy(gameObject);
        dead = true;
    }

    public void Shoot()
    {
        Instantiate(enemyProjectilePrefab, transform.position, Quaternion.Euler(0, 0, 0));
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!dead)
        {
            if (other.gameObject.CompareTag("Player") && (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().Dead == true))
            {
                // Don't die
            }
            else if (!other.gameObject.CompareTag("EnemyProjectileCollider") && !other.gameObject.CompareTag("EnemyCollider"))
            {
                Die();
            }
        }
    }
}
