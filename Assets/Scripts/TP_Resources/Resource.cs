using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TP.Resource
{
    public abstract class Resource : MonoBehaviour
    {
        public string ResourceName;
        public MaterialDataSO MaterialType;

        /* The cell's position of this resource*/
        private Cell cellPosition;

        public Cell CellPosition 
        { 
            get => cellPosition; 
            set => cellPosition = value; 
        }
    }
}

