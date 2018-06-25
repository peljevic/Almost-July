using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RC3.Unity.WFCDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class VertexObject : RC3.Unity.VertexObject
    {
        [SerializeField] private Tile _tile;

        private GameObject _child;
        private MeshFilter _filter;
        private MeshRenderer _renderer;
        private Vector3 _scale;

        private TileSet _tileSet;
        private TileTypeCounter _tileTypeCounter;

        private Rigidbody _rigidbody;
        private MeshCollider _collider;

        private float _height;
        private int _areaValue = 0;
        private float _sunExposureValue;
        private int _suncollisions = 0;

        /// <summary>
        /// 
        /// </summary>
        public Tile Tile
        {
            get { return _tile; }
            set
            {
                _tile = value;
                OnSetTile();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            _child = transform.GetChild(0).gameObject;
            _filter = GetComponent<MeshFilter>();
            _renderer = GetComponent<MeshRenderer>();
            _scale = transform.localScale;

            //added
            _rigidbody = GetComponent<Rigidbody>();
            _height = GetComponent<Transform>().position.y;
            _collider = GetComponent<MeshCollider>();
            _collider.enabled = false;

            _tileTypeCounter = GetComponent<TileTypeCounter>();

            OnSetTile();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        private void OnSetTile()
        {
            transform.localScale = _scale;
            
            if (_tile == null)
            {
                _filter.sharedMesh = null;
                _renderer.sharedMaterial = null;
                _child.SetActive(true);
                return;
            }

            _filter.sharedMesh = _tile.Mesh;
            _renderer.sharedMaterial = _tile.Material;
            _rigidbody.drag = _tile.Drag;
            _areaValue = _tile.Area;
            _tile.CountThisType++;

            var colMesh = _tile.Mesh;
            if (_filter.sharedMesh != null)
            {
                _collider.sharedMesh = _tile.Mesh;
                _collider.enabled = false;
            }

            _child.SetActive(false);
        }

        public int SunCollisions
        {
            get { return _suncollisions; }
            set { _suncollisions = value; }
        }

        public float Height
        {
            set { _height = value; }
            get { return _height; }
        }

        public int Area
        {
            set { _areaValue = value; }
            get { return _areaValue; }
        }

        public Rigidbody Body
        {
            get { return _rigidbody; }
        }

        public float Velocity
        {
            get { return _rigidbody.velocity.magnitude; }
        }

        public MeshCollider Collider
        {
            get { return _collider; }
        }

        public MeshRenderer Renderer
        {
            get { return _renderer; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size"></param>
        public void Reduce(float factor)
        {
            transform.localScale = _scale * factor;
        }

        
        /// <summary>
        /// 
        /// </summary>
        public void Collapse()
        {
            // TODO reset scale and swap symbol
        }
    }
}
