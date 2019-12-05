using UnityEngine;

public class Plant : MonoBehaviour
{
    enum WaterSupply
    {
        Low,
        Normal,
        high
    }

    enum NutriantSupply
    {
        Low,
        Normal,
        high
    }
    enum Danger
    {
        LowWater,
        LowNutrients,
        Animals,
        Insects
    }

    [SerializeField] WaterSupply _waterSupply;
    [SerializeField] NutriantSupply _nutrientSupply;
    [SerializeField] bool _isHealthy;
    [SerializeField] bool _hasEnergy;
    [SerializeField] Danger[] _dangers;

    public void CommunicateViaRoots()
    {

    }

    public void CommunicateViaFume()
    {

    }
}
