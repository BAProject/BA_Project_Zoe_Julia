using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System.Collections.Generic;
using System.Collections;

public class Plant : MonoBehaviour
{
    public bool isGroupMaster = false;
    public PlantUI plantUI;
    public Text plantUIText;
    public Transform connectionsHolder;
    public LineRenderer connectionPrefab;

    public float _startingNutirents;
    public float _startingWater;

    public ReactiveProperty<float> _currentWater;
    public ReactiveProperty<float> _currentNutrients;

    public float _nutrientConsumption;
    public float _waterConsumption;

    private List<Source> _nutrientSources;
    private List<Source> _waterSources;

    [SerializeField] private float _radius;

    private PlantGroup plantGroup;

    [SerializeField] bool _isHealthy;
    [SerializeField] float _sicknessThreshold;

    public Material _materialHealthy;
    public Material _materialSick;
    public MeshRenderer _treeCrownRenderer;

    [SerializeField] bool _isWatered;
    [SerializeField] float _dryThreshold;

    private void Awake()
    {
        _currentNutrients = new ReactiveProperty<float>(_startingNutirents);
        _currentWater = new ReactiveProperty<float>(_startingWater);
        _nutrientSources = new List<Source>();
        _waterSources = new List<Source>();

        //_currentEnergy.Subscribe(energy => Debug.Log("Energy changed to " + energy.ToString()));
        //_currentWater.Subscribe(water => Debug.Log("Water changed to " + water.ToString()));

        _currentNutrients.Subscribe(_ => GetEnergyIfNeeded());
        _currentWater.Subscribe(_ => GetWaterIfNeeded());

        _currentNutrients.Subscribe(_ => UpdateHealth());
        _currentWater.Subscribe(_ => UpdateHealth());

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
        UseUpEnergy(_nutrientConsumption);
        UseUpWater(_waterConsumption);
        UpdateUI();

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

    private void UseUpEnergy(float amount)
    {
        _currentNutrients.Value -= amount;

        if (_currentNutrients.Value <= 0f)
            _currentNutrients.Value = 0f;
    }

    private void UseUpWater(float amount)
    {
        _currentWater.Value -= amount;
    }

    private void GetEnergyIfNeeded()
    {
        if (_currentNutrients.Value >= 100)
            return;

        var totalMissingEnergy = 100 - _currentNutrients.Value;
        var missingEnergy = totalMissingEnergy;
        var nutrientsReturned = 0f;

        foreach (Source source in _nutrientSources)
        {
            nutrientsReturned += source.TryGetNutrients(missingEnergy);
            if (nutrientsReturned == missingEnergy)
                break;
            missingEnergy = totalMissingEnergy - nutrientsReturned;
        }

        _currentNutrients.Value += nutrientsReturned;
    }

    private void GetWaterIfNeeded()
    {
        if (_currentWater.Value >= 100)
            return;

        var totalMissingWater = 100 - _currentWater.Value;
        var missingWater = totalMissingWater;
        var waterReturned = 0f;

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
        if(!active)
            SetSendNutrientsTextActive(false);

        plantUI.gameObject.SetActive(active);
    }

    private void UpdateUI()
    {
        plantUI.SetNutrientFill((float)_currentNutrients.Value / _startingNutirents);
        plantUI.SetWaterFill((float)_currentWater.Value / _startingWater);
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

    private void UpdateHealth()
    {
        if (HasEnoughNutrients() && HasEnoughWater())            
            MakeHealthy();
        else
            MakeSick();
    }

    private void MakeSick()
    {
        _isHealthy = false;
        _treeCrownRenderer.material = _materialSick;
    }

    private void MakeHealthy()
    {
        _isHealthy = true;
        _treeCrownRenderer.material = _materialHealthy;
    }

    public bool CanSendNutrientsTo(Plant otherTree)
    {
        return HasEnoughNutrients() && !otherTree.HasEnoughNutrients();
    }

    public void SendNutrientsTo(Plant otherTree)
    {
        _currentNutrients.Value -= GameInitialization.instance.config.sentNutrientsPerClick;
        otherTree._currentNutrients.Value += GameInitialization.instance.config.sentNutrientsPerClick;
    }

    public void SetSendNutrientsTextActive(bool active)
    {
        plantUIText.gameObject.SetActive(active);
    }

    public bool HasEnoughNutrients()
    {
        return _currentNutrients.Value >= _sicknessThreshold;
    }

    public bool HasEnoughWater()
    {
        return _currentWater.Value >= _dryThreshold;
    }
}
