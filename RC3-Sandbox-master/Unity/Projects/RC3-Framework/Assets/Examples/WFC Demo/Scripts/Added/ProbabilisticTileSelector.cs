
/*
 * Notes
 */

using System;
using System.Linq;
using System.Collections.Generic;

using SpatialSlur.Core;
using RC3.WFC;

using UnityEngine;

namespace RC3.Unity.WFCDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class ProbabilisticTileSelector : MonoBehaviour, ITileSelector
    {
        [SerializeField] private TileSet _tileSet;
        [SerializeField] private int _seed;

        private ProbabilitySelector _selector;
        private double[] _weights;


        /// <summary>
        /// 
        /// </summary>
        private void Awake()
        {
            _weights = GetTileWeights().ToArray();
            _selector = new ProbabilitySelector(_weights, new System.Random(_seed));
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<double> GetTileWeights()
        {
            for (int i = 0; i < _tileSet.Count; i++)
                yield return _tileSet[i].Weight; // TODO assign actual weights
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int Select(TileModel model, int position)
        {
            var d = model.GetDomain(position);
            _selector.SetWeights(GetModifiedWeights(d)); // update the weights in the selector

            return _selector.Next();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        private IEnumerable<double> GetModifiedWeights(ReadOnlySet<int> domain)
        {
            for(int i = 0; i <_weights.Length; i++)
                yield return domain.Contains(i) ? _weights[i] : 0.0;
        }
    }
}
