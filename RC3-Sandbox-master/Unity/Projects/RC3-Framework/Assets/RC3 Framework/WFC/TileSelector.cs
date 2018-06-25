
/*
 * Notes
 */

using System;

namespace RC3.WFC
{


    /// <summary>
    /// 
    /// </summary>
    public abstract class TileSelector
    {
        protected TileModel _model;
        
        /// <summary>
        /// 
        /// </summary>
        public TileModel Model
        {
            get { return _model; }
            set { _model = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public abstract int Select(int position);

        public void Initialize(TileSelector selector)
        {
           
        }
    }
}
