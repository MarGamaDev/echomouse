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

        [SerializeField] protected CatFSM state;
        private NavMeshAgent agent;
        private Rigidbody rb;
        private float distanceToDistraction;

        [SerializeField] Transform[] targets; //array of targets for like sounds or the player
        private Transform currentTarget;
        [SerializeField]private GameObject player;

        private List<IBehavior> wanderBehaviour;
        private List<IBehavior> chaseBehaviour;
        private List<IBehavior> investigateBehaviour;
        private List<IBehavior> attackBehaviour;
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
            state = CatFSM.Wander;

            // referentie naar het Steering component die wordt bewaard in een variabele
            steering = GetComponent<Steering>();

            wanderBehaviour = new List<IBehavior>();
            chaseBehaviour = new List<IBehavior>();
            investigateBehaviour = new List<IBehavior>();
            attackBehaviour = new List<IBehavior>();

            wanderBehaviour.Add(new Wander(gameObject.transform));
            wanderBehaviour.Add(new AvoidWall());
            chaseBehaviour.Add(new Pursue(player));
            chaseBehaviour.Add(new AvoidWall());
            investigateBehaviour.Add(new AvoidWall());
        }

        private void FixedUpdate()
        {
            UpdateCatState();
        }

        private void UpdateCatState()
        {
            float distanceToPlayer = Mathf.Infinity;

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < distanceToPlayer)
            {
                distanceToPlayer = distance;
                currentTarget = player.transform;
            }

            switch (state)
            {
                case CatFSM.Wander:
                    if (distanceToPlayer <= catStats.CatSenseRange)
                        state = CatFSM.Chase;
                        WhatToDoInState();
                    break;
                case CatFSM.Chase:
                    if (distanceToPlayer <= catStats.CatAttackRange)
                        state = CatFSM.Attack;
                    else if (distanceToPlayer > catStats.CatSenseRange)
                        state = CatFSM.Wander;
                        WhatToDoInState();
                    break;
                case CatFSM.Investigate:
                    if (distanceToPlayer <= catStats.CatSenseRange)
                        state = CatFSM.Chase;
                    else if (distanceToDistraction <= 0)
                        state = CatFSM.Wander;
                        WhatToDoInState();
                    break;
                case CatFSM.Attack:
                    if (distanceToPlayer <= catStats.CatSenseRange)
                        state = CatFSM.Chase;
                    WhatToDoInState();
                    break;
            }
        }

        public void GetDestracted(GameObject distraction)
        {
            distanceToDistraction = Mathf.Infinity;

            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance < distanceToDistraction)
            {
                distanceToDistraction = distance;
                currentTarget = distraction.transform;
            }
            investigateBehaviour.Add(new Pursue(distraction));
            state = CatFSM.Investigate;
        }

        private void WhatToDoInState()
        {
            switch (state)
            {
                case CatFSM.Wander:
                        steering.SetBehaviors(wanderBehaviour);
                    break;
                case CatFSM.Chase:
                    steering.SetBehaviors(chaseBehaviour);
                    break;
                case CatFSM.Investigate:
                    steering.SetBehaviors(investigateBehaviour);
                    break;
                case CatFSM.Attack:
                    steering.SetBehaviors(attackBehaviour);
                    break;
            }
        }
    }
}
