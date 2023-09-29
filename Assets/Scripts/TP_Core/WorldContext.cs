using System.Collections;
using System.Collections.Generic;
using TP.Resource;
using UnityEngine;

public class WorldContext : Singleton<WorldContext>
{
    /* Resources */
    [field: SerializeField] public List<RTree> AllTreesOnTheMap { get; private set; }
    [field: SerializeField] public List<RBoulder> AllBouldersOnTheMap { get; private set; }

    /* Buildings */
    [field: SerializeField] public List<Storage> AllStorageOnTheMap { get; private set; }
    [field: SerializeField] public List<ResidentialHouse> AllHousesOnTheMap { get; private set; }
    [field: SerializeField] public List<RBerryBush> AllBushesOnTheMap { get; private set; }
}
