using System.Collections;
using System.Collections.Generic;
using TP.Resource;
using UnityEngine;

public class WorldContext : Singleton<WorldContext>
{
    /* Resources */
    [field: SerializeField] public List<RTree> AllTreesOnTheMap { get; private set; }

    /* Buildings */
    [field: SerializeField] public List<Storage> AllStorageInTheMap { get; private set; }
}
