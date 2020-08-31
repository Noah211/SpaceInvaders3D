using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// Author: Noah Logan

public class EnemyManager : MonoBehaviour
{
    public List<GameObject> Row0 { get; private set; }
    public List<GameObject> Row1 { get; private set; }
    public List<GameObject> Row2 { get; private set; }
    public List<GameObject> Row3 { get; private set; }
    public List<GameObject> Row4 { get; private set; }

    private GameObject attackerPrefab;
    private GameObject bomberPrefab;
    private GameObject hornetPrefab;
    private LinkedList<AudioClip> fastInvaderAudioClips;
    private LinkedListNode<AudioClip> currentFastInvaderAudioClip;
    private AudioSource fastInvaderMusic;
    private AudioSource invaderKilled;
    private AudioSource nextWave;
    private Vector3 firstEnemyPosition;
    private Vector3 horizontalGap;
    private Vector3 verticalGap;
    private Vector3 enemySpeed;
    private Vector3 initialEnemySpeed;
    private Vector3 enemySpeedIncreasePerDirectionChange;
    private int wave;
    private int enemyRows;
    private int enemiesPerRow;
    private float secondsSinceDirectionChange;
    private float directionChangeBuffer;
    private float secondsSinceEnemyShot;
    private float secondsPerEnemyShotMax;
    private float secondsPerEnemyShot;
    private float secondsSinceMusicClip;
    private float secondsPerMusicClip;
    private float initialSecondsPerMusicClip;
    private bool enemiesPaused;

    void Start()
    {
        attackerPrefab = Resources.Load("Prefabs/Enemies/Attacker", typeof(GameObject)) as GameObject;
        bomberPrefab = Resources.Load("Prefabs/Enemies/Bomber", typeof(GameObject)) as GameObject;
        hornetPrefab = Resources.Load("Prefabs/Enemies/Hornet", typeof(GameObject)) as GameObject;
        fastInvaderAudioClips = new LinkedList<AudioClip>();
        fastInvaderAudioClips.AddLast(Resources.Load("Audio/fastInvader1", typeof(AudioClip)) as AudioClip);
        fastInvaderAudioClips.AddLast(Resources.Load("Audio/fastInvader2", typeof(AudioClip)) as AudioClip);
        fastInvaderAudioClips.AddLast(Resources.Load("Audio/fastInvader3", typeof(AudioClip)) as AudioClip);
        fastInvaderAudioClips.AddLast(Resources.Load("Audio/fastInvader4", typeof(AudioClip)) as AudioClip);
        currentFastInvaderAudioClip = fastInvaderAudioClips.First;
        fastInvaderMusic = gameObject.transform.Find("FastInvaderAudio").GetComponent<AudioSource>();
        fastInvaderMusic.clip = currentFastInvaderAudioClip.Value;
        fastInvaderMusic.Play();
        invaderKilled = gameObject.transform.Find("InvaderKilledAudio").GetComponent<AudioSource>();
        nextWave = gameObject.transform.Find("NextWaveAudio").GetComponent<AudioSource>();
        enemySpeed = new Vector3(2.0f, 0, 0.10f);
        initialEnemySpeed = enemySpeed;
        enemySpeedIncreasePerDirectionChange = enemySpeed * 1.01f;
        wave = 1;
        enemyRows = 5;
        enemiesPerRow = 11;
        secondsSinceDirectionChange = 0;
        directionChangeBuffer = 2.0f;
        secondsSinceEnemyShot = 0;
        secondsPerEnemyShotMax = 2f;
        secondsPerEnemyShot = Random.Range(0.5f, secondsPerEnemyShotMax);
        secondsSinceMusicClip = 0;
        secondsPerMusicClip = 0.80f;
        initialSecondsPerMusicClip = secondsPerMusicClip;
        firstEnemyPosition = new Vector3(75, 0, -190);
        horizontalGap = new Vector3(-15, 0, 0);
        verticalGap = new Vector3(0, 0, -15);
        Row0 = new List<GameObject>();
        Row1 = new List<GameObject>();
        Row2 = new List<GameObject>();
        Row3 = new List<GameObject>();
        Row4 = new List<GameObject>();
        SpawnRows();
    }

    void Update()
    {
        if (!enemiesPaused)
        {
            secondsSinceDirectionChange += Time.deltaTime;
            secondsSinceEnemyShot += Time.deltaTime;
            GameObject[] enemyProjectiles = GameObject.FindGameObjectsWithTag("EnemyProjectile");
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            // Make an enemy shoot if time < timer, there are currently less than 3 enemyProjectiles, and the player is alive.
            if ((secondsSinceEnemyShot >= secondsPerEnemyShot) && (enemyProjectiles.Length < 3) && (player.GetComponent<Renderer>().enabled == true))
            {
                EnemyShoot();
                secondsSinceEnemyShot = 0;
                secondsPerEnemyShot = Random.Range(0.5f, secondsPerEnemyShotMax);
            }

            // If a wave of enemies has been killed.
            if ((Row0.Count == 0) && (Row1.Count == 0) && (Row2.Count == 0) && (Row3.Count == 0) && (Row4.Count == 0))
            {
                secondsPerEnemyShotMax *= (9 + wave) / 10;

                if (secondsPerEnemyShotMax < 0.5f)
                {
                    secondsPerEnemyShotMax = 0.5f;
                }

                enemySpeed = initialEnemySpeed;
                SpawnRows();
                nextWave.Play();
                wave++;
                GameObject.Find("GameUIUpdater").GetComponent<GameUIUpdater>().UpdateWave();
                secondsPerMusicClip = initialSecondsPerMusicClip;
            }

            PlayMusic();
        }
    }

    /*
     * Change the direction all enemies are currently moving. Also increase their speed and the tempo of the music.
     */
    public void ChangeEnemyDirection(string direction)
    {
        if (secondsSinceDirectionChange >= directionChangeBuffer)
        {
            secondsPerMusicClip *= 0.9f;
            enemySpeed += enemySpeedIncreasePerDirectionChange * ((9 + wave) / 10);
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

            foreach (GameObject enemy in enemies)
            {
                if (direction == "PositiveX")
                {
                    enemy.SendMessage("ChangeDirection", direction);
                }
                else if (direction == "NegativeX")
                {
                    enemy.SendMessage("ChangeDirection", direction);
                }

                enemy.SendMessage("ChangeSpeed", enemySpeed);
            }

            secondsSinceDirectionChange = 0;
        }
    }

    /*
     * If enemiesPaused = true, make enemies stop moving and delete any enemyProjectiles. If enemiesPaused = false, enemies start moving again.
     */
    public void PauseEnemies(bool enemiesPaused)
    {
        this.enemiesPaused = enemiesPaused;
        Vector3 speed = enemySpeed;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (enemiesPaused)
        {
            speed = Vector3.zero;
            GameObject[] enemyProjectiles = GameObject.FindGameObjectsWithTag("EnemyProjectile");

            foreach (GameObject enemyProjectile in enemyProjectiles)
            {
                Destroy(enemyProjectile);
            }
        }

        foreach (GameObject enemy in enemies)
        {
            enemy.SendMessage("ChangeSpeed", speed);
        }
    }

    public void PlayinvaderKilled()
    {
        invaderKilled.Play();
    }

    private void PlayMusic()
    {
        secondsSinceMusicClip += Time.deltaTime;

        if (secondsSinceMusicClip >= (fastInvaderMusic.clip.length + secondsPerMusicClip))
        {
            if (currentFastInvaderAudioClip.Next != null)
            {
                currentFastInvaderAudioClip = currentFastInvaderAudioClip.Next;
            }
            else
            {
                currentFastInvaderAudioClip = fastInvaderAudioClips.First;
            }

            fastInvaderMusic.Stop();
            fastInvaderMusic.clip = currentFastInvaderAudioClip.Value;
            fastInvaderMusic.Play();
            secondsSinceMusicClip = 0;
        }
    }

    private void SpawnRows()
    {
        for (int row = 0; row < enemyRows; row++)
        {
            for (int enemiesInRow = 0; enemiesInRow < enemiesPerRow; enemiesInRow++)
            {
                GameObject enemyPrefab = attackerPrefab;
                if ((row == 2) || (row == 3))
                {
                    enemyPrefab = hornetPrefab;
                }
                else if (row == 4)
                {
                    enemyPrefab = bomberPrefab;
                }
                Vector3 position = firstEnemyPosition + (enemiesInRow * horizontalGap) + (row * verticalGap);
                GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.Euler(0, 0, 0));

                switch (row)
                {
                    case 0:
                        Row0.Add(enemy);
                        break;
                    case 1:
                        Row1.Add(enemy);
                        break;
                    case 2:
                        Row2.Add(enemy);
                        break;
                    case 3:
                        Row3.Add(enemy);
                        break;
                    case 4:
                        Row4.Add(enemy);
                        break;
                    default:
                        break;
                }
                enemy.SendMessage("ChangeSpeed", initialEnemySpeed);
            }
        }
    }

    private void EnemyShoot()
    {
        if (Row0.Count > 0)
        {
            int i = Random.Range(0, Row0.Count - 1);
            GameObject enemy = Row0.ElementAt(i);
            enemy.SendMessage("Shoot");
        }
        else if (Row1.Count > 0)
        {
            int i = Random.Range(0, Row1.Count - 1);
            GameObject enemy = Row1.ElementAt(i);
            enemy.SendMessage("Shoot");
        }
        else if (Row2.Count > 0)
        {
            int i = Random.Range(0, Row2.Count - 1);
            GameObject enemy = Row2.ElementAt(i);
            enemy.SendMessage("Shoot");
        }
        else if (Row3.Count > 0)
        {
            int i = Random.Range(0, Row3.Count - 1);
            GameObject enemy = Row3.ElementAt(i);
            enemy.SendMessage("Shoot");
        }
        else if (Row4.Count > 0)
        {
            int i = Random.Range(0, Row4.Count - 1);
            GameObject enemy = Row4.ElementAt(i);
            enemy.SendMessage("Shoot");
        }
    }
}
