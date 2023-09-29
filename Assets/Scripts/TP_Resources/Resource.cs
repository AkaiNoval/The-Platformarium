using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHarvestable
{
    void Harvest();
}
namespace TP.Resource
{
    public abstract class Resource : MonoBehaviour
    {
        public string ResourceName;
        public MaterialDataSO MaterialType;
        
        public Animator animator;
        /* The cell's position of this resource*/
        private Cell cellPosition;

        public Cell CellPosition 
        { 
            get => cellPosition; 
            set => cellPosition = value; 
        }
        [SerializeField] private int health = 10;
        public int Health 
        { 
            get => health;
            set 
            { 
                health = Mathf.Clamp(value, 0, 10); 
                //Play animation here
            }
        }
    }
}

