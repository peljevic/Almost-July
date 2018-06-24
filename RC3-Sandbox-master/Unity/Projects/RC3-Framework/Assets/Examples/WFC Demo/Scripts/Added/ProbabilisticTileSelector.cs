
/*
 * Notes
 */

using System;
using System.Linq;
using System.Collections.Generic;

using SpatialSlur.Core;
using RC3.WFC;

namespace RC3.Unity.WFCDemo
{
    /// <summary>
    /// 
    /// </summary>
    public class ProbabilisticTileSelector : TileSelector
    {
        private ProbabilitySelector _selector;
        private double[] _weights;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public ProbabilisticTileSelector(TileSet tileSet, int seed = 1)
        {
            _weights = GetTileWeights(tileSet).ToArray();
            _selector = new ProbabilitySelector(_weights, new Random(seed));
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerable<double> GetTileWeights(TileSet tileSet)
        {
            for (int i = 0; i < tileSet.Count; i++)
                yield return tileSet[i].Weight;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int Select(int position)
        {
            var d = Model.GetDomain(position);
            _selector.SetWeights(GetModifiedWeights(d)); // update the weights in the selector
            return d.ElementAt(_selector.Next());
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        private IEnumerable<double> GetModifiedWeights(ReadOnlySet<int> domain)
        {
            for(int i =0; i <_weights.Length; i++)
                yield return domain.Contains(i) ? _weights[i] : 0.0;
        }
    }
}
