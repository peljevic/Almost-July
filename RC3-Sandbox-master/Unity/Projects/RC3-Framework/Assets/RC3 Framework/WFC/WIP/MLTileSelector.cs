
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
    public class MLTileSelector : ITileSelector
    {
        private MLSelectorAgent _agent;
        private ProbabilitySelector _selector;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public MLTileSelector(TileSet tileSet, MLSelectorAgent agent, int seed)
        {
            if (agent == null)
                throw new ArgumentNullException();

            _agent = agent;
            _selector = new ProbabilitySelector(GetDefaultWeights(tileSet.Count), new Random(seed));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        private IEnumerable<double> GetDefaultWeights(int count)
        {
            for (int i = 0; i < count; i++)
                yield return 1.0;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public int Select(TileModel model, int position)
        {
            _selector.SetWeights(_agent.GetWeights(position));

            var d = model.GetDomain(position);
            return d.ElementAt(_selector.Next());
        }
    }
}
