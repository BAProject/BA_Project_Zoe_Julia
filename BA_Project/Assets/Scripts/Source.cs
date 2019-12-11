using UnityEngine;

public class Source : MonoBehaviour
{
    public enum SourceType
    {
        Water,
        Nutrient
    }

    [SerializeField] private int _nutrients;
    [SerializeField] private SourceType _type;
    [SerializeField] private float _radius;

    private void Start()
    {
        NotifyTreesInRadius();
    }

    private void NotifyTreesInRadius()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            if (obj.tag == "Tree")
            {
                Debug.Log("Found Tag Tree");
                Tree tree = obj.GetComponent<Tree>();
                if (tree != null)
                {
                    Debug.Log("Found Script Tree");
                    if (_type == SourceType.Water)
                        tree.WaterSources.Add(this);
                    else if (_type == SourceType.Nutrient)
                        tree.NutrientSources.Add(this);
                }
            }
        }
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
}
