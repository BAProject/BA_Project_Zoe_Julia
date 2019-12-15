using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    public static Level instance;

    [SerializeField] private List<Plant> _allPlants = new List<Plant>();

    private Transform playerTransform;

    private void Awake()
    {
        instance = this;
    }

    public void RegisterPlant(Plant plant)
    {
        if(_allPlants.Contains(plant))
        {
            Debug.LogWarning("trying to register, but plants list already contains " + plant.name);
        }
        else
        {
            _allPlants.Add(plant);
        }
    }

    public void UnregisterPlant(Plant plant)
    {
        if (!_allPlants.Contains(plant))
        {
            Debug.LogWarning("trying to unregister, but plants list doesn't contain " + plant.name);
        }
        else
        {
            _allPlants.Remove(plant);
        }
    }

    public List<Plant> GetPlantsInRange(Vector3 origin, float radius)
    {
        List<Plant> plantsInRange = new List<Plant>();

        foreach(Plant plant in _allPlants)
        {
            if(Vector3.Distance(origin, plant.transform.position) <= radius)
            {
                plantsInRange.Add(plant);
            }
        }

        return plantsInRange;
    }

    public bool IsPlayerInRange(Vector3 position, float range)
    {
        CheckPlayerRef();
        return Vector3.Distance(position, playerTransform.position) <= range;
    }

    private void CheckPlayerRef()
    {
        if(playerTransform == null)
        {
            playerTransform = GameObject.FindGameObjectWithTag(Tags.PlayerTag).transform;
        }
    }
}
