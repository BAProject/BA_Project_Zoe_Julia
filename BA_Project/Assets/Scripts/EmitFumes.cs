using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class EmitFumes : MonoBehaviour
{

	public Transform FragranceAllGood;
	public Transform FragranceRain;
	public Transform FragranceInsects;
	public Transform FragranceWater;
	public Transform FragranceNutrient;

	// Start is called before the first frame update
	void Start()
    {
		FragranceAllGood.GetComponent<ParticleSystem> ().enableEmission = true;

		FragranceRain.GetComponent<ParticleSystem> ().enableEmission = false;
		FragranceInsects.GetComponent<ParticleSystem> ().enableEmission = false;
		FragranceWater.GetComponent<ParticleSystem> ().enableEmission = false;
		FragranceNutrient.GetComponent<ParticleSystem> ().enableEmission = false;
		FragranceInsects.GetComponent<ParticleSystem> ().enableEmission = false;


	}

    // Update is called once per frame
    void Update()
    {
			Plant plantScript = GetComponent<Plant>();
		if (plantScript._currentEnergy.Value < 50)
		{
			FragranceNutrient.GetComponent<ParticleSystem>().enableEmission = true;

			FragranceRain.GetComponent<ParticleSystem>().enableEmission = false;
			FragranceInsects.GetComponent<ParticleSystem>().enableEmission = false;
			FragranceWater.GetComponent<ParticleSystem>().enableEmission = false;
			FragranceAllGood.GetComponent<ParticleSystem>().enableEmission = false;
			FragranceInsects.GetComponent<ParticleSystem>().enableEmission = false;
		}

		if(plantScript._currentWater.Value < 50)
		{
			FragranceRain.GetComponent<ParticleSystem>().enableEmission = true;

			FragranceNutrient.GetComponent<ParticleSystem>().enableEmission = false;
			FragranceInsects.GetComponent<ParticleSystem>().enableEmission = false;
			FragranceWater.GetComponent<ParticleSystem>().enableEmission = false;
			FragranceAllGood.GetComponent<ParticleSystem>().enableEmission = false;
			FragranceInsects.GetComponent<ParticleSystem>().enableEmission = false;
		}

    }
}
