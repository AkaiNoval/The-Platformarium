using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP.Resource
{
    public class RTree : Resource, IHarvestable
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

        public void Harvest()
        {
            Health--;
            if(Health > 0)
            {
                animator.SetTrigger("Chopped");
            }
            else
            {
                animator.SetTrigger("Death");
                gameObject.SetActive(false);
                Debug.Log("You got some wood ");
                var allStorages = WorldContext.Instance.AllStorageOnTheMap;
                for (int i = 0; i < allStorages.Count; i++)
                {
                    if (allStorages[i].CurrentCapacity >= allStorages[i].MaxCapacity) continue;
                    allStorages[i].CurrentCapacity++; break;
                }
            }

        }
    }


}

