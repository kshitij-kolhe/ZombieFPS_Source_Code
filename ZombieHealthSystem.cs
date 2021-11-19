using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieHealthSystem : MonoBehaviour
{

    private float maxHealth = 0;

    [SerializeField]
    private GameObject healthBar = null;

    private float currentHelath;

    void Start()
    {
        currentHelath = maxHealth;
    }
    

    public void Hit(float hitPoint)
    {
        healthBar.GetComponent<Slider>().value =(currentHelath - hitPoint) / maxHealth;
        currentHelath -= hitPoint;

        if (currentHelath <= 0.0f)
        {
            Destroy(transform.GetComponent<NavMeshAgent>());
            if (transform.GetComponent<ZombieAI>() != null)
            {
                transform.GetComponent<ZombieAI>().IsActive = false;
            }
            else
            {
                transform.GetComponent<PlayerAI>().IsActive = false;
            }
        }

    }

    public void SetMaxHealth(float maxHealth)
    {
        this.maxHealth = maxHealth;
        currentHelath = this.maxHealth;
    }
    
}
