using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rain : MonoBehaviour
{

    public Transform RainParticle;

    // CLick Button to let it rain and refill the watersources/currentWaterValue of the trees
    private void LetItRain()
    {
        Plant plantScript = GetComponent<Plant>();

        if (Input.GetButtonDown("Rain"))
        {
            RainParticle.GetComponent<ParticleSystem>().enableEmission = true;
        }

    }
}

