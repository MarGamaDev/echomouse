using System.Collections.Generic;
using UnityEngine;

namespace GLU.SteeringBehaviours.Refactored
{
    public abstract class FlockManager : IFlockManager
    {
        protected Dictionary<GameObject, Flock> m_flockingObjects = new Dictionary<GameObject, Flock>();

        private List<Flock> m_objects        = new List<Flock>();
        private float       m_dataUpdateTime = -1.0f;
        private float       m_dataBuildTime  = -1.0f;

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public FlockManager()
        {
        }
        
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public virtual void RegisterFlockObject(GameObject flockingObject, Flock flock)
        {
            m_flockingObjects.Add(flockingObject, flock);
            m_dataUpdateTime = Time.time;
        }

        public virtual void UnregisterFlockObject(GameObject flockingObject)
        {
            m_flockingObjects.Remove(flockingObject);
            m_dataUpdateTime = Time.time;
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        protected virtual void PreStep() {}

        public void Step(float dt)
        {
            // prestep flocking objects
            PreStep();

            // step flocking objects
            StepFlockingObjects();
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        private void StepFlockingObjects()
        {
            // update list with flocking objects
            if (m_dataUpdateTime > m_dataBuildTime)
            {
                m_objects = new List<Flock>();
                foreach (Flock flock in m_flockingObjects.Values)
                    m_objects.Add(flock);

                m_dataBuildTime = m_dataUpdateTime;
            }

            // step all flocking objects
            foreach (Flock flock in m_objects)
                flock.CalculateDesiredVelocity();
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public abstract void ProcessNeighbors(Vector3 position, float searchRadius, IFlockManager.ProcessNeighbor processNeighbor);
    }
}
