using UnityEngine;

namespace RC3.Unity.WFCDemo
{
    public static class ExtensionMethods
    {
        public static void ResetTransform(this Transform transform)
        {
            transform.position = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
}