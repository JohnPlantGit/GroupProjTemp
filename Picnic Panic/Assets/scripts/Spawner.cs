using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject m_player = null;
    public GameObject m_enemyPrefab = null;
    public int m_health;
    public float m_spawnDelay;
    public float m_spawnJitter;
    public int m_maxSpawns;

    private float m_timer;
    private int m_currentSpawns;

    private List<GameObject> m_enemies = new List<GameObject>();
	// Use this for initialization
	void Start ()
    {
        RandomizeDelay();
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_timer -= Time.deltaTime;

        if (m_timer <= 0.0f && m_currentSpawns < m_maxSpawns)
        {
            RandomizeDelay();

            Vector3 spawnPosition = new Vector3();

            Rect playArea = new Rect(new Vector2(-25, -25), new Vector2(50, 50));

            Vector3 direction = -m_player.transform.position;
            direction.y = 0;
            direction.Normalize();
            direction *= 35;

            Vector3 jitter = new Vector3(Random.value, 0, Random.value);
            jitter.Normalize();
            jitter *= 5;

            Vector3 position = direction + jitter;

            spawnPosition.x = Mathf.Min(Mathf.Max(position.x, playArea.xMin), playArea.xMax);
            spawnPosition.z = Mathf.Min(Mathf.Max(position.z, playArea.yMin), playArea.yMax);
            spawnPosition.y = 0.5f;

            GameObject newEnemy = Instantiate(m_enemyPrefab, spawnPosition, new Quaternion());
            newEnemy.GetComponent<Enemy>().m_target = m_player;
            newEnemy.GetComponent<Enemy>().m_spawner = gameObject;
            newEnemy.GetComponent<Enemy>().m_maxHealth = m_health;

            m_enemies.Add(newEnemy);
            m_currentSpawns++;
        }
	}

    private void RandomizeDelay()
    {
        m_timer = m_spawnDelay + (m_spawnJitter * Random.Range(-1, 1));
    }

    public void EnemyDeath(GameObject enemy)
    {
        m_currentSpawns--;
        m_enemies.Remove(enemy);
    }
}
