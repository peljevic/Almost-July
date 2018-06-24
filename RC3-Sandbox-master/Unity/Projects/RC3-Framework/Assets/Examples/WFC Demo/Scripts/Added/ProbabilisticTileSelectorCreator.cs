
/*
 * Notes
 */

using System.Collections.Generic;
using UnityEngine;
using RC3.WFC;

namespace RC3.Unity.WFCDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class ProbabilisticTileSelectorCreator : InitializableBehavior
    {
        [SerializeField] private SharedTileSelector _tileSelector;
        [SerializeField] private TileSet _tileSet;
        private int _seed = 1;


        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            _tileSelector.Value = new ProbabilisticTileSelector(_tileSet, _seed);
        }
    }
}
