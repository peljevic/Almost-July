
/*
 * Notes
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace RC3.Unity.WFCDemo
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(menuName = "RC3/WFC Demo/Tile")]
    public class Tile : ScriptableObject
    {
        [SerializeField] private Mesh _mesh;
        [SerializeField] private Material _material;
        [SerializeField, HideInInspector] private string[] _labels;

        //added
        [SerializeField] private int _index;
        [SerializeField] private float _drag;
        [SerializeField] private double _weight = 1.0;
        [SerializeField] private int _area = 0;

        private int _thisTileCount = 0;

        public float Drag
        {
            get { return _drag; }
        }

        public int Area
        {
            set { _area = value; }
            get { return _area; }
        }

        public double Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public int Index
        {
            get { return _index; }
        }

        public int CountThisType
        {
            set { _thisTileCount = value; }
            get { return _thisTileCount; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Mesh Mesh
        {
            get { return _mesh; }
        }


        /// <summary>
        /// 
        /// </summary>
        public Material Material
        {
            get { return _material; }
        }


        /// <summary>
        /// 
        /// </summary>
        public string[] Labels
        {
            get { return _labels; }
        }
    }
}
