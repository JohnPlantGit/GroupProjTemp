using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float m_speed;
    public float m_dashDistance;
    public float m_dashCooldown;
    public float m_attackSpeed;
    public Renderer m_facing;

    private Rigidbody m_rigidBody = null;
    private ParticleSystem m_dashParticle = null;
    private List<Collider> m_enemies = new List<Collider>();
    private Vector3 m_movement;
    private float m_dashTimer;
    private float m_attackTimer;
    private bool m_canAttack;

	// Use this for initialization
	void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_dashParticle = GetComponentInChildren<ParticleSystem>();
        m_movement = new Vector3();
	}

    // Update is called once per frame
    void Update ()
    {
        m_dashTimer -= Time.deltaTime;
        m_attackTimer -= Time.deltaTime;

        m_movement.z = Input.GetAxisRaw("Vertical");
        m_movement.x = Input.GetAxisRaw("Horizontal");

        
        if (Input.GetKeyDown(KeyCode.Space) && m_dashTimer < 0)
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward, m_dashDistance * new Vector3((Mathf.Log(1f / (Time.deltaTime * m_rigidBody.drag + 1)) / -Time.deltaTime), 0, (Mathf.Log(1f / (Time.deltaTime * m_rigidBody.drag + 1)) / -Time.deltaTime)));
            m_rigidBody.AddForce(dashVelocity);
            m_dashTimer = m_dashCooldown;

            m_dashParticle.Play();
        }

        if ((Input.GetButtonDown("Fire1") || Input.GetAxis("Joystick Attack") != 0) && m_attackTimer <= 0)
        {
            for (int i = 0; i < m_enemies.Count; i++)
            {
                if (m_enemies[i] != null)
                {
                    m_enemies[i].GetComponent<Enemy>().TakeDamage(10);
                }
                else
                {
                    m_enemies.RemoveAt(i);
                }
            }
            m_facing.material.color = new Color(1, 0, 0);
            m_canAttack = false;
        }
        if (m_attackTimer < 0 && !m_canAttack)
        {
            m_facing.material.color = new Color(1, 1, 1);
            m_canAttack = true;
        }
    }

    private void FixedUpdate()
    {
        m_rigidBody.MovePosition(m_rigidBody.position + (m_movement * Time.deltaTime * m_speed));

        Vector3 functional = new Vector3(Input.GetAxis("Functional Direction X"), 0, Input.GetAxis("Functional Direction Y"));

        if (functional.magnitude > 0)
        {
            m_rigidBody.rotation = Quaternion.LookRotation(functional.normalized);
        }
        else if (m_movement.magnitude > 0)
        {
            m_rigidBody.rotation = Quaternion.LookRotation(m_movement.normalized);
        }

        if (m_dashParticle.isPlaying && m_rigidBody.velocity.magnitude < 1)
        {
            if (m_rigidBody.velocity.magnitude != 0)
            {
                m_dashParticle.Stop();
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            m_enemies.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
        {
            m_enemies.Remove(other);
        }
    }
}
