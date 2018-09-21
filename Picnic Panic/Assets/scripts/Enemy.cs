using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public GameObject m_target = null;
    public int m_maxHealth;
    public float m_speed;
    [HideInInspector] public GameObject m_spawner = null;

    private int m_health;
    private NavMeshPath m_path = null;
    private Rigidbody m_rigidBody = null;
    private Vector3 m_movement;

    // Use this for initialization
    void Start()
    {
        m_health = m_maxHealth;
        m_rigidBody = GetComponent<Rigidbody>();
        m_path = new NavMeshPath();
        m_movement = new Vector3();
    }

    private void FixedUpdate()
    {
        m_rigidBody.MovePosition(m_rigidBody.position + (m_movement * Time.deltaTime * m_speed));

        if ((m_target.transform.position - m_rigidBody.position).magnitude < 2)
        {
            m_rigidBody.position = m_target.transform.position + (m_rigidBody.position - m_target.transform.position).normalized * 2;
        }
    }
    // Update is called once per frame
    void Update()
    {
        m_movement = (m_target.transform.position - transform.position).normalized;
        
        //m_movement = (path.corners[1] - transform.position).normalized;
        m_movement.y = 0;
    }

    public void TakeDamage(int damage)    
    {
        m_health -= damage;
        if (m_health <= 0)
        {
            m_spawner.GetComponent<Spawner>().EnemyDeath(gameObject);
            Destroy(gameObject);
        }
    }
}
