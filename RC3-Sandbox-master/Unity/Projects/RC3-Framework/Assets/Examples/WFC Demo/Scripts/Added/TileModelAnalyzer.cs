using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RC3.Graphs;
using RC3.WFC;

using SpatialSlur.Core;

namespace RC3.Unity.WFCDemo
{
    public class TileModelAnalyzer : MonoBehaviour
    {
        public bool _analysisOn = false;

        [SerializeField] private SharedDigraph _grid;
        [SerializeField] private TileSet _tileSet;
        [SerializeField] private float _neededArea = 30;
        [SerializeField] private float _areaTolerance;
        [SerializeField] public float _allowedDisplacement;

        List<VertexObject> _verts;
        private Digraph _graph;
        private CollapseStatus _status;
        private TileModelManager _manager;

        [Range(0.0f, 10000.0f)]
        [SerializeField]
        private float MaxForce = 1000.0f;

        [Range(0.0f, 10000.0f)]
        [SerializeField]
        private float MaxTorque = 1000.0f;

        private float BreakForce = Mathf.Infinity;
        private float BreakTorque = Mathf.Infinity;

        private List<VertexObject> _meshedTiles;
        private List<VertexObject> _weakTiles;
        private List<VertexObject> _stableTiles;

        private float _totalArea =0;
        private float[] _densities;
        

        private void Awake()
        {
            _graph = _grid.Graph;
            _verts = _grid.VertexObjects;

            _manager = GetComponent<TileModelManager>();
            _tileSet = _manager.TileSet;

            _stableTiles = new List<VertexObject>();
            _weakTiles = new List<VertexObject>();
            _meshedTiles = new List<VertexObject>();
            _densities = new float[_verts.Count];

        }

        private void Update()
        {
            if(_analysisOn)    // if (_status == CollapseStatus.Complete) AnalyzeModel(); 
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    AnalyzeModel();
                    MarkWeakTiles();
                }
            }
            
        }

        private void MarkWeakTiles()
        {
            foreach (var weak in _weakTiles)
                weak.Renderer.sharedMaterial.color = Color.red;
        }

        /// <summary>
        /// Evaluations for the agent to collect
        /// </summary>
        public IEnumerable<VertexObject> WeakTiles
        {
            get { return _weakTiles; }

        }

        /// <summary>
        /// Evaluations for the agent to collect
        /// </summary>
        public IEnumerable<VertexObject> StableTiles
        {
            get { return _stableTiles; }

        }

        private void SeparateTiles()
        {
            _meshedTiles.Clear();

            for (int i = 0; i < _graph.VertexCount; i++)
            {
                var v = _verts[i];

                if (v.Tile != _tileSet[0])
                {
                    _meshedTiles.Add(v);
                }
            }
        }

        public void AnalyzeModel()
        {
            AreaAnalyzer();
            CountAllDensities();
            SunExposureAnalysis();
            StructureAnalyzer();
        }

        #region Average Sun Exposure

        private void SunExposureAnalysis()
        {

        }

        private void CompareSunExposure()
        {
            var collisions = 0;

            foreach(var v in _verts)
            {
                collisions += v.SunCollisions;
            }
        }

        #endregion

        #region Area Analyzer 

        private bool AreaAnalyzer()
        {
            return SlurMath.ApproxEquals(AreaDeviation(), _neededArea, _areaTolerance);              
        }

        private float TotalArea()
        {
            _totalArea = 0;

            foreach (var v in _verts)
            {
                _totalArea += v.Area;
            }

            return _totalArea;
        }

        private float AreaDeviation()
        {
            return TotalArea() - _neededArea;
        }

        #endregion Area Analyzer 

        #region Density Analyzer 

        private void CountAllDensities()
        {
            for (int i = 0; i < _verts.Count; i++)
            {
                _densities[i] = CountNeighbourhoodDensity(i);

                if (_densities[i] > 0.5f)
                    Debug.Log("Density on vertex " + i + "is too high");
            }
        }

        private float CountNeighbourhoodDensity(int vertex)
        {
            int neighCounter = 0;
            var neigh = _graph.GetVertexNeighborsOut(vertex);

            foreach (var v in neigh)
            {
                if (_meshedTiles.Contains(_verts[v]))
                {
                    neighCounter++;
                }
            }
            return neighCounter / 14;
        }
        
        #endregion Density Analyzer 

        #region Structure Analyzer

        private void StructureAnalyzer()
        {
            AddKinematicToLowest();
            AddJointsToConnected();
            AddGravity();
            CheckVelocity();
            RemoveGravity();
            RemoveJoints();
            ResetPositions();
        }

        private float MinDistance()
        {
            if (_meshedTiles == null)
                SeparateTiles();

            var minDistance = _meshedTiles.Min(v => v.transform.position.y);

            Debug.Log("Minimun distance from the ground is" + minDistance);
            return minDistance;
        }

        private void AddKinematicToLowest()
        {
            var minDistance = MinDistance();
            var tolerance = 1.0f;

            foreach (var v in _meshedTiles)
                v.Body.isKinematic = false;

            foreach (var v in _meshedTiles.Where(v => SlurMath.ApproxEquals(v.transform.position.y, minDistance, tolerance)))
                { v.Body.isKinematic = true; }
        }
        
        private void AddJointsToConnected()
        {
            for (int i = 0; i < _verts.Count; i++)
            {
                var v = _verts[i];

                var allNeigh = v.Tile.Labels;

                for (int j = 0; j < allNeigh.Length; j++)
                {
                    var neighbour = _graph.GetVertexNeighborOut(i, j);
                    var vn = _verts[neighbour];

                    if (allNeigh[j] != "0" && v != vn)
                    {
                        var vJoint = v.gameObject.AddComponent<FixedJoint>();
                        vJoint.connectedBody = vn.GetComponent<Rigidbody>();

                        vJoint.breakForce = BreakForce;
                        vJoint.breakTorque = BreakTorque;
                    }
                }
            }
        }

        private void CheckVelocity()
        {            
            foreach (var v in _verts)
            {
                var _displacement = MaxVelocity(v);

                if (_displacement > _allowedDisplacement)
                {  
                    v.GetComponent<Renderer>().material.color = Color.red;
                    _weakTiles.Add(v);
                    //_displacementValues[v.Tile.Index] = _displacement;
                }

                else if (_displacement < _allowedDisplacement)
                {
                    _stableTiles.Add(v);
                }
            }
        }

        private float MaxVelocity(VertexObject vertex)
        {
            var _XmeshDisplacement = vertex.GetComponent<Rigidbody>().velocity.x;
            var _YmeshDisplacement = vertex.GetComponent<Rigidbody>().velocity.y;
            var _ZmeshDisplacement = vertex.GetComponent<Rigidbody>().velocity.z;

            var maxValue = Mathf.Max(_XmeshDisplacement, _YmeshDisplacement, _ZmeshDisplacement) * 1000;
            //Debug.Log(maxValue);

            return maxValue;
        }

        private void AddGravity()
        {
            if (_meshedTiles == null)
            {
                SeparateTiles();

                if (_meshedTiles != null)
                {
                    foreach (var r in _meshedTiles)
                    {
                        var body = r.Body;
                        body.useGravity = true;
                    }
                }
            }
        }
        
        private void RemoveGravity()
        {
            if (_meshedTiles == null)
            {
                SeparateTiles();

                if (_meshedTiles != null)
                {
                    foreach (var r in _meshedTiles)
                    {
                        var body = r.Body;
                        body.useGravity = false;
                    }
                }
            }
        }

        private void RemoveJoints()
        {
            foreach (var v in _verts)
            {
               while(v.gameObject.GetComponent<FixedJoint>()!=null)
                    Destroy(v.gameObject.GetComponent<FixedJoint>());
            }
        }

        private void ResetPositions()
        {
            foreach (var v in _verts)
                v.transform.ResetTransform();
        }

        #endregion Structure Analyzer
    }
}