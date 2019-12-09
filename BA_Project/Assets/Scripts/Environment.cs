using UnityEngine;

public class Environment : MonoBehaviour
{
    public static Environment singleton;



    private void Awake()
    {
        if(singleton == null)
        {
            singleton = this;
            Initialize();
        }
    }

    private void Initialize()
    {
        
    }

    //Vorhandene Nährstoffe (komplett random pro Pflanze)
    //Vorhandenes Wasser(Regen) mit Zufallselement - einstellbar im Inspector
    //Insekten (komplett random pro Pflanze)

}
