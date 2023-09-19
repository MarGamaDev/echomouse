using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CatBehaviour : MonoBehaviour
{
    [SerializeField] CatStats catStats;

    [SerializeField]protected CatFSM m_state;
    private NavMeshAgent agent;
    private Rigidbody rb;

    [SerializeField]Transform[] targets; //array of targets for like sounds or the player
    private Transform currentTarget;

    protected enum CatFSM
    {
        Wander,
        Investigate,
        Chase,
        Attack
    }

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        m_state = CatFSM.Wander;
    }

    private void Update()
    {
        UpdateCatState();
    }

    private void UpdateCatState()
    {
        float distanceToTarget = Mathf.Infinity;

        foreach (Transform target in targets)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance < distanceToTarget)
            {
                distanceToTarget = distance;
                currentTarget = target;
            }
        }

        switch (m_state)
        {
            case CatFSM.Wander:
                if (distanceToTarget <= catStats.catSenseRange)
                    m_state = CatFSM.Chase;
                break;
        }

        //switch (m_state)
        //{
        //    case ChildFSM.Chase:
        //        if (isStunned == true)
        //            TrasitionToStunnedState();
        //        else if (distanceToTarget <= enemyStats.attackRange)
        //            m_state = ChildFSM.Attack;
        //        break;

        //    case ChildFSM.Attack:
        //        if (isStunned == true)
        //            TrasitionToStunnedState();
        //        else if (distanceToTarget > enemyStats.attackRange)
        //            m_state = ChildFSM.Chase;
        //        break;

        //    case ChildFSM.Stunned:
        //        if (isStunned == true)
        //            return;
        //        else if (distanceToTarget <= enemyStats.attackRange)
        //            m_state = ChildFSM.Attack;
        //        else if (distanceToTarget > enemyStats.attackRange)
        //            m_state = ChildFSM.Chase;
        //        break;
        //}
    }
}
