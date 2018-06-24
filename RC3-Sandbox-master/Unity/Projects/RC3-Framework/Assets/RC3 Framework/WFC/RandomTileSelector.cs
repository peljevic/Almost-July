
/*
 * Notes
 */

using System;
using System.Linq;

namespace RC3.WFC
{
    /// <summary>
    /// 
    /// </summary>
    public class RandomTileSelector : TileSelector
    {
        private Random _random;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public RandomTileSelector(int seed)
        {
            _random = new Random(seed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public override int Select(int position)
        {
            var d = Model.GetDomain(position);
            return d.ElementAt(_random.Next(d.Count));
        }

    }
}
