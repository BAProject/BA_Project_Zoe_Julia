using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmitFumes : MonoBehaviour
{
    public Plant plant;

	public ParticleSystem FragranceAllGood;
	public ParticleSystem FragranceRain;
	public ParticleSystem FragranceInsects;
	public ParticleSystem FragranceWater;
	public ParticleSystem FragranceNutrient;

	// Start is called before the first frame update
	void Start()
    {
		FragranceAllGood.enableEmission = true;

		FragranceRain.enableEmission = false;
		FragranceInsects.enableEmission = false;
		FragranceWater.enableEmission = false;
		FragranceNutrient.enableEmission = false;
		FragranceInsects.enableEmission = false;
	}

    // Update is called once per frame
    void Update()
    {
        FragranceAllGood.enableEmission = plant.HasEnoughNutrients() && plant.HasEnoughWater();
        FragranceNutrient.enableEmission = !plant.HasEnoughNutrients();
        FragranceWater.enableEmission = !plant.HasEnoughWater();
    }
}
