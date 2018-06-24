 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RC3;
using RC3.WFC;
using RC3.Graphs;


namespace RC3.Unity.WFCDemo
{
    public class SunAnalysis : MonoBehaviour
    {
      
        // Variables
        private int _initalParticlesCount = 0;
        private int _particlesCountAfterSimulation;
        private float _directLightPercentage = 100.0f;
        private float _evaluationTime;
        private bool _endSimulation;
        private List<ParticleCollisionEvent> _collisionEvents;
        [SerializeField] private GameObject _sunHitPoint;

        void Start()
        {
            _initalParticlesCount = GetComponent<ParticleSystem>().main.maxParticles;
            _collisionEvents = new List<ParticleCollisionEvent>();
        }

        void Update()
        {
            if (Time.time > _evaluationTime && _endSimulation == false)
            {
                _particlesCountAfterSimulation = GetComponent<ParticleSystem>().particleCount;
                _directLightPercentage = (_particlesCountAfterSimulation * 100) / _initalParticlesCount;

                Debug.Log("Percentage of direct sun light exposure is: " + (100 - _directLightPercentage).ToString());
                _endSimulation = true;
            }
        }

        public void StartSimulation()
        {
            transform.gameObject.SetActive(true);
            GetComponent<ParticleSystem>().Play();
            _evaluationTime = (Time.time + GetComponent<ParticleSystem>().main.startLifetime.constant) - 1;
            _endSimulation = false;
        }

        void OnParticleCollision(GameObject other)
        {
            int numCollisionEvents = GetComponent<ParticleSystem>().GetCollisionEvents(other, _collisionEvents);

            Rigidbody rb = other.GetComponent<Rigidbody>();
            VertexObject vertexobject = other.GetComponent<VertexObject>();
            int i = 0;

            while (i < numCollisionEvents)
            {
                Vector3 pos = _collisionEvents[i].intersection;
                Instantiate(_sunHitPoint, pos, Quaternion.identity, this.transform);
                vertexobject.SunCollisions++;
                i++;
            }
        }
         
        public float Percentage
        {
            get { return 100 - _directLightPercentage; }
        }

        public float GetSimulationResult()
        {
            return 100 - _directLightPercentage;
        }
    }
}
