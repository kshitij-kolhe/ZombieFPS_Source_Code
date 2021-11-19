using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ZombieAI : MonoBehaviour
{
    private NavMeshAgent navMeshAgent = null;

    private Transform target = null;

    private bool flag = false;
    private bool once = true;

    private int currentWayPointIndex = 0;
    private float wayPointTolerance = 0.5f;
    private bool isActive = true;
    private float hitPoint = 2;

    public Transform Target { get => target; set => target = value; }
    public bool IsActive { get => isActive; set => isActive = value; }
    public float HitPoint { get => hitPoint; set => hitPoint = value; }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

   
    void Update()
    {
        if(IsActive)
        {
            Move();
            Attack();
        }
        else if(!isActive && once)
        {
            DeathSequence();
        }
    }

    private void Attack()
    {
        if(Vector3.Distance(transform.position,target.position) < 2.3f && !GetComponent<Animator>().GetBool("attacking"))
        {
            
            navMeshAgent.velocity = Vector3.zero;
            navMeshAgent.isStopped = true;
            transform.LookAt(target);

            if (!GetComponent<Animator>().GetBool("attacking"))
            {
                AnimationEvents("walking",false);
                AnimationEvents("attacking",true);
            }
        }
    }

    private void Move()
    {
        if (!GetComponent<Animator>().GetBool("walking") && !flag && navMeshAgent != null)
        {
            navMeshAgent.destination = target.position;
            AnimationEvents("walking", true);
        }
    }

    //Animation Event of zombie attacking player
    private void Hit()
    {
        //player has zombie health system, it acts as universla health system, has not renamed yet
        target.GetComponent<ZombieHealthSystem>().Hit(HitPoint);
    }

    //Player on bullet hit
    public void OnHit(float time)
    {
        if(IsActive && GetComponent<Animator>().GetBool("walking"))
        {
            flag = true;
            StartCoroutine(Stop(time));
        }
    }

    private IEnumerator Stop(float time)
    {
        if(navMeshAgent != null)
            navMeshAgent.isStopped = true;

        AnimationEvents("walking", false);
        yield return new WaitForSeconds(time);
        AnimationEvents("walking", true);

        if (navMeshAgent != null)
            navMeshAgent.isStopped = false;
        flag = false;
    }

    public void AnimationEvents(String parameterName,bool value)
    {
        GetComponent<Animator>().SetBool(parameterName,value);
    }

    private void DeathSequence()
    {
        GetComponentInChildren<Canvas>().enabled = false;
        AnimationEvents("walking", false);
        AnimationEvents("attacking", false);

        WorldSpawnner worldSpawnner = FindObjectOfType<WorldSpawnner>();
        worldSpawnner.AliveZombieCount--;
        once = false;

        foreach (BoxCollider boxCollider in GetComponentsInChildren<BoxCollider>())
        {
            boxCollider.enabled = false;
        }

        AnimationEvents("dyingH", true);
    }

    private void Death()
    {
        Destroy(transform.gameObject);
    }
}
