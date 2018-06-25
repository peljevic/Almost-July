using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RC3.Unity.EventDemo
{


    /// <summary>
    /// 
    /// </summary>
    public class BehaviorA : MonoBehaviour
    {
        [SerializeField] private Event _onComplete;


        /// <summary>
        /// 
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Event raised!");
                _onComplete.Raise();
            }
        }


        /// <summary>
        /// returns true when finished
        /// </summary>
        /// <returns></returns>
        private bool Run()
        {
            return false;
        }
    }
}