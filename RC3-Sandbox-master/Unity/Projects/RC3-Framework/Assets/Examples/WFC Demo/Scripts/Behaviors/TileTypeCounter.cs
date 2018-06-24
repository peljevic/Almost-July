using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RC3.Graphs;
using System;
using RC3.WFC;


namespace RC3.Unity.WFCDemo
{
    public class TileTypeCounter : MonoBehaviour
    {
        [SerializeField] private SharedDigraph _grid;
        private List<VertexObject> _vertices;
        private TileSet _tileSet;
        private int[] _counts;

        private void Awake()
        {
            _vertices = _grid.VertexObjects;

            _tileSet = GetComponent<TileModelManager>().TileSet;
            _counts = new int[_tileSet.Count];

            for (int i = 0; i < _tileSet.Count; i++)
            {
                _counts[i] = _tileSet[i].CountThisType;
            }
        }

        
    }
}
