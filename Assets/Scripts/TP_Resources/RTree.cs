using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP.Resource
{
    public class RTree : Resource
    {

        private void OnEnable()
        {
            if (WorldContext.Instance != null)
            {
                WorldContext.Instance.AllTreesOnTheMap.Add(this);
            }
        }
        private void OnDisable()
        {
            if (WorldContext.Instance != null)
            {
                WorldContext.Instance.AllTreesOnTheMap.Remove(this);
            }
        }
    }
}

