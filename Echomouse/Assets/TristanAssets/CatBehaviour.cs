using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace GLU.SteeringBehaviours
{
    [RequireComponent(typeof(GLU.SteeringBehaviours.Steering))]
    public class CatBehaviour : MonoBehaviour
    {
        [SerializeField] CatStats catStats;

        [SerializeField] protected CatFSM m_state;
        private NavMeshAgent agent;
        private Rigidbody rb;

        [SerializeField] Transform[] targets; //array of targets for like sounds or the player
        private Transform currentTarget;

        private List<IBehavior> wanderBehaviour;
        private Steering steering;
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

            // referentie naar het Steering component die wordt bewaard in een variabele
            steering = GetComponent<Steering>();

            wanderBehaviour = new List<IBehavior>();

            wanderBehaviour.Add(new Wander(gameObject.transform));
        }

        private void FixedUpdate()
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
                        WhatToDoInState();
                    break;
                case CatFSM.Chase:
                    if (distanceToTarget <= catStats.catAttackRange)
                        m_state = CatFSM.Attack;
                    else if (distanceToTarget > catStats.catSenseRange)
                        m_state = CatFSM.Wander;
                        WhatToDoInState();
                    break;
                case CatFSM.Investigate:
                    if (distanceToTarget <= catStats.catSenseRange)
                        m_state = CatFSM.Chase;
                    else
                        m_state = CatFSM.Wander;
                        WhatToDoInState();
                    break;
            }
        }

        private void WhatToDoInState()
        {
            switch (m_state)
            {
                case CatFSM.Wander:
                        steering.SetBehaviors(wanderBehaviour);
                    break;
            }
        }
    }
}
