
using System.Collections.Generic;
using UnityEngine;
using RC3.Graphs;
using RC3.WFC;

namespace RC3.Unity.WFCDemo
{
    /// <summary>
    /// Creates a new graph based on tile labels
    /// </summary>
    public class TileGraphExtractor : MonoBehaviour
    {
        [SerializeField] private SharedDigraph _tileGraph;
        [SerializeField] private string[] _validLabels;
        [SerializeField] private GameObject _vertexObject;
        [SerializeField] private GameObject _lineRenderer;

        private TileModel _model;
        private TileMap<string> _map;
        private HashSet<string> _labelSet;
        private List<VertexObject> _vertices;
        private bool _swap = false;
        private int _edgeCount=0;


        /// <summary>
        /// 
        /// </summary>
        private void Initialize()
        {
            _vertices = _tileGraph.VertexObjects;

            var manager = GetComponent<TileModelManager>();

            _model = manager.TileModel;
            _map = manager.TileMap;
            _labelSet = new HashSet<string>(_validLabels);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W)) SwapVisible();

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Extract();
                Debug.Log("Function Called");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public Digraph Extract()
        {
            // https://en.wikipedia.org/wiki/Single_responsibility_principle

            if (_model == null)
                Initialize();

            var g0 = _tileGraph.Graph;
            var g1 = new Digraph(g0.VertexCount);
            var n = _map.TileDegree;

            for (int v0 = 0; v0 < g0.VertexCount; v0++)
            {
                g1.AddVertex();
                var tile = _model.GetAssigned(v0);

                for (int i = 0; i < n; i++)
                {
                    var label = _map.GetLabel(i, tile);

                    if (_labelSet.Contains(label))
                    {
                        var v1 = _tileGraph.Graph.GetVertexNeighborOut(v0, i);
                        if (v0 != v1) g1.AddEdge(v0, v1);
                    }
                }
            }

            return g1;
        }


        /// <summary>
        /// 
        /// </summary>
        public Digraph Extract2()
        {
            // https://en.wikipedia.org/wiki/Single_responsibility_principle

            if (_model == null)
                Initialize();

            var g0 = _tileGraph.Graph;
            var g1 = new Digraph(g0.VertexCount);

            for (int i = 0; i < g0.VertexCount; i++)
            {
                g1.AddVertex();

                if (_vertices[i].Tile.Mesh != null)
                {
                    _swap = true;
                    _vertices[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
                    Transform pos = _vertices[i].transform;
                    Instantiate(_vertexObject, pos);
                }

                Debug.Log("Works!");

                var tile = _model.GetAssigned(i);
                var n = _map.TileDegree;

                for (int j = 0; j < n; j++)
                {
                    var label = _map.GetLabel(j, tile);
                    var neigh = _tileGraph.Graph.GetVertexNeighborOut(i, j);

                    if (_labelSet.Contains(label))
                    {
                        if (i != j)
                        {
                            g1.AddEdge(i, j);

                            var lR = Instantiate(_lineRenderer, Vector3.zero, Quaternion.identity, _vertices[i].transform);

                            lR.GetComponent<LineRenderer>().SetPosition(0, _vertices[i].gameObject.transform.position);
                            lR.GetComponent<LineRenderer>().SetPosition(1, _vertices[neigh].gameObject.transform.position);

                            _edgeCount++;
                            Debug.Log("Edge count : " + _edgeCount.ToString());
                        }
                    }
                }
            }
            return g1;
        }



        private void SwapVisible()
        {
            foreach (var v in _vertices)
            {
                v.gameObject.GetComponent<MeshRenderer>().enabled = !_swap;
            }
        }
    }
}
