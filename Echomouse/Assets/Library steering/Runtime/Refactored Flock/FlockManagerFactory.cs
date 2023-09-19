using System;
using System.Collections;
using UnityEngine;

namespace GLU.SteeringBehaviours.Refactored
{
    public class FlockManagerFactory : Generic.Singleton<FlockManagerFactory>
    {
        public enum NeighborLookupType { Simple, OverlapSphere }

        private SteeringStepSettings m_stepSettings;
        private IFlockManager        m_flockManager;

        // constructor to disable DontDestroyOnLoad() in generic Singleton
        FlockManagerFactory() : base(false)
        {
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public IFlockManager GetFlockManager(SteeringSettings settings) 
        {
            // create a new manager if necessary
            if (m_flockManager == null)
            {
                // copy the step settings
                m_stepSettings = SteeringSetup.GetInstance().m_stepSettings;

                // construct a new flock manager
                switch (SteeringSetup.GetInstance().m_flockManagerType)
                {
                    case NeighborLookupType.Simple        : m_flockManager = new FlockManagerSimple       ();         break;
                    case NeighborLookupType.OverlapSphere : m_flockManager = new FlockManagerOverlapSphere(settings); break;
                }
            }
            return m_flockManager;
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        private void Start()
        {
            // copy the step settings
            m_stepSettings = SteeringSetup.GetInstance().m_stepSettings;

            // copy the step settings and start coroutine if required
            m_stepSettings = SteeringSetup.GetInstance().m_stepSettings;
            if (m_stepSettings.m_location == SteeringStepSettings.UpdateMethod.Coroutine)
                StartCoroutine(StepCoroutine());
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        private IEnumerator StepCoroutine()
        {
            while (true)
            {
                float dt = m_stepSettings.DeltaTime();
                Step(dt);
                yield return new WaitForSeconds(dt);
            }
        }

        private void Update()
        {
            if (m_stepSettings.m_location == SteeringStepSettings.UpdateMethod.Update)
                Step(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (m_stepSettings.m_location == SteeringStepSettings.UpdateMethod.FixedUpdate)
                Step(Time.fixedDeltaTime);
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        private void Step(float dt)
        {
            // step the flock manager
            m_flockManager.Step(Time.fixedDeltaTime);
        }
    }
}

