using UnityEngine;
using UniRx;
using System.Collections.Generic;

public class Plant : MonoBehaviour
{
    [SerializeField] private ReactiveProperty<int> _currentWater;
    [SerializeField] private ReactiveProperty<int> _currentEnergy;
    [SerializeField] private bool _isHealthy;
    [SerializeField] private Danger[] _dangersCurrentlyAwareOf;
    [SerializeField] private Plant[] _plantsInGroup;
    [SerializeField] private Plant[] _plantsInFumeReach;

    [SerializeField] private List<Source> _nutrientSources;
    [SerializeField] private List<Source> _waterSources;

    private void Start()
    {
        _currentEnergy = new ReactiveProperty<int>(100);
        _currentWater = new ReactiveProperty<int>(100);

        _currentEnergy.Subscribe(energy => Debug.Log("Energy changed to " + energy.ToString()));
        _currentWater.Subscribe(water => Debug.Log("Water changed to " + water.ToString()));

        _currentEnergy.Subscribe(_ => GetEnergyIfNeeded());
        _currentWater.Subscribe(_ => GetWaterIfNeeded());

        UseUpEnergy(20); // test
    }

    public List<Source> NutrientSources
    {
        get { return _nutrientSources; }
        set { _nutrientSources = value; }
    }

    public List<Source> WaterSources
    {
        get { return _waterSources; }
        set { _waterSources = value; }
    }

    private void UseUpEnergy(int amount)
    {
        _currentEnergy.Value -= amount;
    }

    private void GetEnergyIfNeeded()
    {
        if (_currentEnergy.Value >= 100)
            return;

        int totalMissingEnergy = 100 - _currentEnergy.Value;
        int missingEnergy = totalMissingEnergy;
        int nutrientsReturned = 0;

        foreach (Source source in _nutrientSources)
        {
            nutrientsReturned += source.TryGetNutrients(missingEnergy);
            if (nutrientsReturned == missingEnergy)
                break;
            missingEnergy = totalMissingEnergy - nutrientsReturned;
        }

        _currentEnergy.Value += nutrientsReturned;
    }

    private void GetWaterIfNeeded()
    {
        if (_currentEnergy.Value >= 100)
            return;

        int totalMissingWater = 100 - _currentEnergy.Value;
        int missingWater = totalMissingWater;
        int waterReturned = 0;

        foreach (Source source in _waterSources)
        {
            waterReturned += source.TryGetNutrients(missingWater);
            if (waterReturned == missingWater)
                break;
            missingWater = totalMissingWater - waterReturned;
        }

        _currentWater.Value += waterReturned;
    }

    private void ReceiveWarning()
    {

    }

    private void CommunicateViaRoots(Danger danger)
    {

    }

    private void CommunicateViaFume(Danger danger)
    {

    }
}
