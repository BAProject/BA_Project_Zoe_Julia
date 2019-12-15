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

    [SerializeField] private float _radius;

    private void Awake()
    {
        _currentEnergy = new ReactiveProperty<int>(100);
        _currentWater = new ReactiveProperty<int>(100);
        _nutrientSources = new List<Source>();
        _waterSources = new List<Source>();

        //_currentEnergy.Subscribe(energy => Debug.Log("Energy changed to " + energy.ToString()));
        //_currentWater.Subscribe(water => Debug.Log("Water changed to " + water.ToString()));

        _currentEnergy.Subscribe(_ => GetEnergyIfNeeded());
        _currentWater.Subscribe(_ => GetWaterIfNeeded());

        Level.instance.RegisterPlant(this);
    }

    private void OnDestroy()
    {
        Level.instance.UnregisterPlant(this);
    }

    private void Update()
    {
        UseUpEnergy(20); // test
        //Debug.Log("elements in nutrient sources: " + _nutrientSources.Count);

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(Level.instance.IsPlayerInRange(transform.position, 4f))
            {
                CreateAndEmitSignal(Signal.SignalType.Fume);
            }
        }
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

    public virtual void CreateAndEmitSignal(Signal.SignalType signalType)
    {
        Signal signal = new Signal(this, signalType);
        EmitSignal(signal);
    }

    protected virtual void EmitSignal(Signal signal)
    {
        List<Plant> plantsInRange = Level.instance.GetPlantsInRange(transform.position, _radius);
        Debug.Log("emit signal of type " + signal.signalType + " from " + gameObject.name + " with radius " + _radius);

        foreach(Plant plant in plantsInRange)
        {
            if(signal.CanBeReceived(plant) && plant.CanHandleSignal(signal))
            {
                plant.ReceiveSignal(signal);
            }
        }
    }

    protected virtual void ReceiveSignal(Signal signal)
    {
        Debug.Log("receive signal of type " + signal.signalType + " at " + gameObject.name);
        signal.OnReceived(this);

        if(signal.isRepeated)
        {
            EmitSignal(signal);
        }
    }

    protected virtual bool CanHandleSignal(Signal signal)
    {
        return true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0.5f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
