using UnityEngine;
using UniRx;
using System.Collections.Generic;
using System.Collections;

public class Plant : MonoBehaviour
{
    public bool isGroupMaster = false;
    public PlantUI plantUI;
    public Transform connectionsHolder;
    public LineRenderer connectionPrefab;

    [SerializeField] private ReactiveProperty<int> _currentWater;
    [SerializeField] private ReactiveProperty<int> _currentEnergy;

    private List<Source> _nutrientSources;
    private List<Source> _waterSources;

    [SerializeField] private float _radius;

    private PlantGroup plantGroup;

    private const int maxEnergyWater = 1000;

    private void Awake()
    {
        _currentEnergy = new ReactiveProperty<int>(maxEnergyWater);
        _currentWater = new ReactiveProperty<int>(maxEnergyWater);
        _nutrientSources = new List<Source>();
        _waterSources = new List<Source>();

        //_currentEnergy.Subscribe(energy => Debug.Log("Energy changed to " + energy.ToString()));
        //_currentWater.Subscribe(water => Debug.Log("Water changed to " + water.ToString()));

        _currentEnergy.Subscribe(_ => GetEnergyIfNeeded());
        _currentWater.Subscribe(_ => GetWaterIfNeeded());

        GameInitialization.instance.level.RegisterPlant(this);
    }

    private IEnumerator Start()
    {        
        InitializeGroup();
        // Wait one frame for all plant groups to be initialized
        yield return null;
        CreateConnections();
    }

    private void OnDestroy()
    {
        GameInitialization.instance.level.UnregisterPlant(this);
    }

    private void Update()
    {
        UseUpEnergy(1);
        UpdateUI();
        //UseUpEnergy(20); // test
        //Debug.Log("elements in nutrient sources: " + _nutrientSources.Count);

        //if(Input.GetKeyDown(KeyCode.G))
        //{
        //    if(GameInitialization.instance.level.IsPlayerInRange(transform.position, 4f))
        //    {
        //        CreateAndEmitSignal(Signal.SignalType.Fume);
        //    }
        //}
    }

    private void InitializeGroup()
    {
        if (isGroupMaster)
        {
            plantGroup = new PlantGroup();
            plantGroup.master = this;
            plantGroup.plants = GameInitialization.instance.level.GetPlantsInRange(transform.position, _radius);

            foreach (Plant plant in plantGroup.plants)
            {
                plant.plantGroup = plantGroup;
            }
        }      
    }

    private void CreateConnections()
    {
        if(plantGroup != null)
        {
            foreach (Plant plant in plantGroup.plants)
            {
                if (plant != this)
                {
                    LineRenderer connection = Instantiate(connectionPrefab, connectionsHolder);
                    connection.SetPosition(0, transform.position);
                    connection.SetPosition(1, plant.transform.position);
                }
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

    public virtual void CreateAndEmitSignal(Signal.SignalType signalType)
    {
        Signal signal = new Signal(this, signalType);
        EmitSignal(signal);
    }

    protected virtual void EmitSignal(Signal signal)
    {
        List<Plant> plantsInRange = GameInitialization.instance.level.GetPlantsInRange(transform.position, _radius);
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

    public virtual void SetPlantControlled(bool controlled) 
    {
        SetUIActive(controlled);
        connectionsHolder.gameObject.SetActive(controlled);
    }

    public virtual void SetUIActive(bool active) 
    {
        plantUI.gameObject.SetActive(active);
    }

    private void UpdateUI()
    {
        plantUI.SetNutrientFill((float)_currentEnergy.Value / maxEnergyWater);
        plantUI.SetWaterFill((float)_currentWater.Value / maxEnergyWater);
    }

    public bool DoesPlantGroupContain(Plant plant)
    {
        if (plantGroup == null)
            return false;

        return plantGroup.plants.Contains(plant);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 0.5f, 0f, 0.2f);
        Gizmos.DrawSphere(transform.position, _radius);

        if(isGroupMaster && Application.isPlaying)
        {
            Gizmos.color = new Color(0f, 1f, 1f, 1f);
            foreach (Plant plant in plantGroup.plants)
            {
                Gizmos.DrawLine(transform.position, plant.transform.position);
            }
        }
    }
}
