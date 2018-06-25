using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using SpatialSlur.Core;
using RC3.WFC;

namespace RC3.Unity.WFCDemo
{
    public class NewProbabilisticHolder : MonoBehaviour
    {
        public ProbabilisticTileSelector _tileSelector;
        [SerializeField] private SharedTileSelector _selector;
        private TileModelManager _manager;

        private void Awake()
        {
            _manager = GetComponent<TileModelManager>();
            _tileSelector = new ProbabilisticTileSelector(_manager.TileSet);

        }

        public ProbabilisticTileSelector Selector
        {
            set { _tileSelector = value; }
            get { return _tileSelector; }
        }
    }
}
