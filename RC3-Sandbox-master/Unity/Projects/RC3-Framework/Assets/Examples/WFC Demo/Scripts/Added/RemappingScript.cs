using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using RC3.Graphs;
using System;
using RC3.WFC;

using SpatialSlur.Core;

namespace RC3.Unity.WFCDemo
{
    /// <summary>
    /// Changes Colors Based on Certain Criterias
    /// </summary>
    public class RemappingScript : MonoBehaviour
    {    

        [SerializeField] private SharedDigraph _grid;

        private TileModelManager _manager;
        private TileSet _tileSet;
        private List<VertexObject> _meshedTiles;
        List<VertexObject> _verts;
        private Digraph _graph;

        private float _velocitySum;
        private float _totalArea =0;


        private void Awake()
        {
            _graph = _grid.Graph;
            _verts = _grid.VertexObjects;

            _manager = GetComponent<TileModelManager>();
            _tileSet = _manager.TileSet;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad0)) RestoreOriginalColor();
            if (Input.GetKeyDown(KeyCode.Keypad1)) ChangeBasedOnVelocity();
            if (Input.GetKeyDown(KeyCode.Keypad2)) ChangeBasedOnArea();
            if (Input.GetKeyDown(KeyCode.Keypad3)) ChangeBasedOnNeighbourhoodDensity();
        }

        private void SeparateTiles()
        {
            for (int i = 0; i < _graph.VertexCount; i++)
            {
                var v = _verts[i];

                if (v.Tile != _tileSet[0])
                {
                    _meshedTiles.Add(v);
                }
            }
        }

        #region GUI
        public void ChangeDensity()
        {
            ChangeBasedOnNeighbourhoodDensity();
        }

        public void ChangeArea()
        {
            ChangeBasedOnArea();
        }

        public void ChangeVelocity()
        {
            ChangeBasedOnVelocity();
        }
        #endregion GUI

        private void ChangeBasedOnNeighbourhoodDensity()
        {
            for (int i = 0; i < _verts.Count; i++)
            {
               var _densities = CountNeighbourhoodDensity(i);
                //Debug.Log("Density is " + _densities[i]);

                if (_densities > 0.5f)
                    Debug.Log("Density on vertex " + i + "is too high");

                _verts[i].Renderer.material.color = DisplayChange(_densities, 14);
            }
        }

        private float CountNeighbourhoodDensity(int vertex)
        {
            int neighCounter = 0;
            var neigh = _graph.GetVertexNeighborsOut(vertex);

            foreach (var v in neigh)
            {
                if (_verts[v].Tile != _tileSet[0])
                {
                    neighCounter++;
                }
            }
            float neighDensity = (float)neighCounter / 14.0f;
            //Debug.Log("Density is " + neighDensity.ToString());

            return neighDensity;
        }


        private float MaxVelocity()
        {
            var maxVelocity = _verts.Max(v => v.transform.position.y);

            return maxVelocity;
        }

        void ChangeBasedOnVelocity()
        {
            for (int i = 0; i < _verts.Count; i++)
            {
                var v = _verts[i];
                var currentVelocity = v.Body.velocity.magnitude;

                if (v.Renderer != null)
                { v.Renderer.material.color = DisplayChange(currentVelocity, (int)MaxVelocity()); }
            }
        }

        private void TotalArea()
        {
            foreach (var v in _verts)
            {
                _totalArea += v.Tile.Area;
            }
        }
        
        private void ChangeBasedOnArea()
        {
            TotalArea();

            Debug.Log(_totalArea);
                   
            foreach (var v in _verts)
            {               
                var area = v.Area;

                if (v.Renderer != null)
                { v.Renderer.material.color = DisplayChange(area, _totalArea); }
            }
        }

        private void RestoreOriginalColor()
        {
            for (int i = 0; i < _verts.Count; i++)
            {
                var v = _verts[i];

                for (int j = 0; j < _tileSet.Count; j++)
                {
                    var tile = _tileSet[j];

                    if (v.Tile = tile)
                        v.Renderer.material = tile.Material;
                }

            }
        }

        public Color DisplayChange(float first, float second)
        {
            float mappedvalue = Remap(first, 0, second, 0.0f, 1.0f);
            //two colors to interpolate between
            Color color1 = new Color(0, 0, 0, 0);
            Color color2 = new Color(1, 0, 1, 1);
            //interpolate color from mapped value
            Color mappedcolor = Color.Lerp(color1, color2, mappedvalue);

            return mappedcolor;
        }

        private float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }
    }
}

