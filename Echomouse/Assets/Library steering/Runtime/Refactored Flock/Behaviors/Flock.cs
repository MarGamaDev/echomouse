using UnityEngine;

namespace GLU.SteeringBehaviours.Refactored
{
    public class Flock : Behavior 
    {
        public delegate void ProcessNeighbor(GameObject neighbor);

        public Vector3 m_localDesiredVelocity;

        // private stuff
        private readonly GameObject m_ownGameObject;

        private IFlockManager    m_flockManager;
        private SteeringSettings m_settings;
        private float            m_searchRadius;

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public Flock(GameObject gameObject)
        {
            m_ownGameObject = gameObject;
        }

        ~Flock()
        {
            // unregister this object from the flock manager
            if (m_flockManager != null)
                m_flockManager.UnregisterFlockObject(m_ownGameObject);
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        override public void Start(BehaviorContext context)
        {
            base.Start(context);

            // get flock manager
            m_flockManager = FlockManagerFactory.GetInstance().GetFlockManager(context.m_settings);

            // register this object with the flock manager
            m_flockManager.RegisterFlockObject(m_ownGameObject, this);

            // remember settings
            m_settings = context.m_settings;

            // determine largest radius and keep it as the search radius when looking for neighbors
            m_searchRadius = 0.0f;
            if (context.m_settings.m_flockAlignmentWeight  > 0.0f)
                m_searchRadius = Mathf.Max(m_searchRadius, context.m_settings.m_flockAlignmentRadius);
            if (context.m_settings.m_flockCohesionWeight   > 0.0f)
                m_searchRadius = Mathf.Max(m_searchRadius, context.m_settings.m_flockCohesionRadius);
            if (context.m_settings.m_flockSeparationWeight > 0.0f)
                m_searchRadius = Mathf.Max(m_searchRadius, context.m_settings.m_flockSeparationRadius);
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public void CalculateDesiredVelocity() => CalculateDesiredVelocity(m_settings);

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public void CalculateDesiredVelocity(SteeringSettings settings)
        {
            // create a new flock force accumulator that will collect all alignment, cohesion and separation forces
            FlockForceAccumulator accumulator = new FlockForceAccumulator(m_ownGameObject, settings.m_flockVisibilityAngle,
                                                                                           settings.m_flockAlignmentRadius,
                                                                                           settings.m_flockCohesionRadius,
                                                                                           settings.m_flockSeparationRadius);

            // ask manager to process all neighbors
            m_flockManager.ProcessNeighbors(m_ownGameObject.transform.position, m_searchRadius, accumulator.ProcessNeighbor);

            // calculate desired velocity
            Vector3 desiredVelocity = accumulator.CalculateDesiredVelocity(m_ownGameObject.transform.position, settings);
            m_localDesiredVelocity = desiredVelocity.normalized * settings.m_maxDesiredVelocity;
        }
        
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        public override Vector3 CalculateSteeringForce(float dt, BehaviorContext context)
        {
            // update target position plus desired velocity, and return steering force 
            m_velocityDesired = m_localDesiredVelocity;
            m_positionTarget  = context.m_position + m_velocityDesired * dt;
            return m_velocityDesired - context.m_velocity;
        }

        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        override public void OnDrawGizmos(BehaviorContext context)
        {
            base.OnDrawGizmos(context);
#if false
            DrawGizmos.DrawWireDisc(context.m_position, m_searchRadius, Color.black);
#endif
#if false
            // calculate the world rotation (the angle the velocity makes in with the vector to the right)
            float worldRotation = Vector3.SignedAngle(Vector3.right, context.m_velocity, -Vector3.up) * Mathf.Deg2Rad;

            // draw view lines
            float a = context.m_settings.m_flockVisibilityAngle * Mathf.Deg2Rad;
            Vector3 rangeMin = new Vector3(m_searchRadius * Mathf.Cos(worldRotation - a),
                                           0.0f,
                                           m_searchRadius * Mathf.Sin(worldRotation - a));

            Vector3 rangeMax = new Vector3(m_searchRadius * Mathf.Cos(worldRotation + a),
                                           0.0f,
                                           m_searchRadius * Mathf.Sin(worldRotation + a));

            Debug.DrawLine(context.m_position, context.m_position + rangeMin, Color.black);
            Debug.DrawLine(context.m_position, context.m_position + rangeMax, Color.black);
#endif
        }
    }
}
