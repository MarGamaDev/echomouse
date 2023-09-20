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
        private float distanceToDistraction;
        [SerializeField]private GameObject player;
        [SerializeField] private GameObject attackHitBox;
        private GameObject currentTarget;
        private Coroutine currentCoroutine;

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

        private void Start()
        {

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
        }

        private void FixedUpdate()
        {
            UpdateCatState();
        }

        private void UpdateCatState()
        {
            float distanceToPlayer = Mathf.Infinity;

            float distancePlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distancePlayer < distanceToPlayer)
            {
                distanceToPlayer = distancePlayer;
            }

            if (currentTarget != null)
            {
                distanceToDistraction = Mathf.Infinity;

                float distanceDistraction = Vector3.Distance(transform.position, currentTarget.transform.position);

                if (distanceDistraction < distanceToDistraction)
                {
                    distanceToDistraction = distanceDistraction;
                }
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
                    else if (distanceToDistraction <= 5)
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
            investigateBehaviour.Clear();

            currentTarget = distraction;
            investigateBehaviour.Add(new Pursue(currentTarget));
            investigateBehaviour.Add(new AvoidWall());
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
                    distanceToDistraction = Mathf.Infinity;

                    float distance = Vector3.Distance(transform.position, currentTarget.transform.position);

                    if (distance < distanceToDistraction)
                    {
                        distanceToDistraction = distance;
                    }
                    steering.SetBehaviors(investigateBehaviour);
                    break;
                case CatFSM.Attack:
                    if (currentCoroutine == null)
                    {
                        currentCoroutine = StartCoroutine(Attack());
                    }
                    steering.SetBehaviors(attackBehaviour);
                    break;
            }
        }

        private IEnumerator Attack()
        {
            attackHitBox.SetActive(true);
            yield return new WaitForSeconds(1f);
            attackHitBox.SetActive(false);

            currentCoroutine = null;
        }
    }
}
