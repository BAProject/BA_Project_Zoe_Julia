using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour
{
    [SerializeField] private int _nutrients;
    [SerializeField] private string type;
    [SerializeField] private float radius;

    private void Start()
    {
        RegisterWithTrees(GetTreesInRadius());
    }

    public int TryGetNutrients(int amount)
    {
        if (_nutrients >= amount)
        {
            _nutrients -= amount;
            return amount;
        }
        else
        {
            int rest = _nutrients;
            _nutrients = 0;
            return rest;
        }
    }

    private List<Plant> GetTreesInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        
        foreach(Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            if (obj.tag == "Tree")
            {
                Tree tree = obj.GetComponent<Tree>();
                if (tree != null)
                    tree.WaterSources.Add(this);
            }
        }

        return new List<Plant>();
    }

    private void RegisterWithTrees(List<Plant> plants)
    {
        // for each plant register this source.
    }

    public void GetNutrients()
    {

    }
}
