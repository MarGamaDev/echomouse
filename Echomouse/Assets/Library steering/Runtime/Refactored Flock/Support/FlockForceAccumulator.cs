using UnityEngine;

namespace GLU.SteeringBehaviours.Refactored
{
    public class FlockForceAccumulator
    {
        private GameObject      m_obj;
        private float           m_visibilityAngle;

        private FlockAlignment  m_alignment; 
        private FlockCohesion   m_cohesion;  
        private FlockSeparation m_separation;

        public FlockForceAccumulator(GameObject obj,
                                     float      visibilityAngle,
                                     float      alignmentRadius,
                                     float      cohesionRadius,
                                     float      separationRadius)
        {
            // remember our own game object (which we have to ignore) and other parameters
            m_obj             = obj;
            m_visibilityAngle = visibilityAngle;

            // prepare to calculate the three flock calculation forces
            m_alignment       = new FlockAlignment (alignmentRadius );
            m_cohesion        = new FlockCohesion  (cohesionRadius  );
            m_separation      = new FlockSeparation(separationRadius);
        }

        public void ProcessNeighbor(GameObject neighbor)
        {
            // do not process our own object
            if (neighbor == m_obj)
                return;

            // get steering component from neighbor (if any)
            Steering neighborSteering = neighbor.GetComponent<Steering>();
            if (neighborSteering == null)
            {
                Debug.LogError($"ERROR: Flock Behavior found neighbor without Steering script!");
                return;
            }

            // calcute direction and squared distance to neighbor
            Vector3 neighborDirection = neighborSteering.m_position - m_obj.transform.position;
            float   sqrDistance       = neighborDirection.sqrMagnitude;

            // skip neigbor if not in sight
            if (Vector3.Angle(m_obj.transform.forward, neighborDirection) > m_visibilityAngle)
                return;

            // update alignment, cohesion and separation
            m_alignment .AddVelocity (sqrDistance, neighborSteering.m_velocity);
            m_cohesion  .AddPosition (sqrDistance, neighborSteering.m_position);
            m_separation.AddDirection(sqrDistance, neighborDirection);
        }

        public Vector3 CalculateDesiredVelocity(Vector3 position, SteeringSettings settings)
        {
            // calculate desired velocity
            return m_alignment .DesiredVelocity()         * settings.m_flockAlignmentWeight +
                   m_cohesion  .DesiredVelocity(position) * settings.m_flockCohesionWeight  +
                   m_separation.DesiredVelocity()         * settings.m_flockSeparationWeight;
        }
    }
}
