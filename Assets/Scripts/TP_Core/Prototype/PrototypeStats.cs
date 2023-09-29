using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeStats : MonoBehaviour
{
    [field: SerializeField] public SOPrototype SOPrototypeStats { get; private set; }

    [SerializeField] private float health;
    [SerializeField] private float hunger;
    [SerializeField] private float energy;
    [SerializeField] private float hungerDecayRate;
    [SerializeField] private float energyDecayRate;

    public float Health
    {
        get { return health; }
        private set { health = Mathf.Clamp(value, 0f, SOPrototypeStats.MaxHealth); } 
    }

    public float Hunger
    {
        get { return hunger; }
        set { hunger = Mathf.Clamp(value, 0f, SOPrototypeStats.MaxHunger); }
    }

    public float Energy
    {
        get { return energy; }
        set { energy = Mathf.Clamp(value, 0f, SOPrototypeStats.MaxEnergy); }
    }
    private void Start()
    {
        InitStats(SOPrototypeStats);
        StartCoroutine(DecreaseStatsOverTime(hungerDecayRate, energyDecayRate));
    }

    private void InitStats(SOPrototype stats)
    {
        Health = stats.MaxHealth;
        Energy = stats.MaxEnergy;
        Hunger = stats.MaxHunger;
    }

    private void OnEnable()
    {
        StartCoroutine(DecreaseStatsOverTime(hungerDecayRate, energyDecayRate));
    }

    private IEnumerator DecreaseStatsOverTime(float decayRateHunger, float decayRateEnergy)
    {
        while (Hunger > 0f || Energy > 0f)
        {
            yield return new WaitForSeconds(1f);
            // Decrease hunger and energy
            Hunger -= decayRateHunger;
            Energy -= decayRateEnergy;
            Debug.Log("Decrease hunger and energy");
        }
    }

}

