using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EmitFumes : MonoBehaviour
{
    public Plant plant;

	public ParticleSystem[] FragrancesAllGood;
	public ParticleSystem[] FragrancesRain;
	public ParticleSystem[] FragrancesInsects;
	public ParticleSystem[] FragrancesWater;
	public ParticleSystem[] FragrancesNutrient;

	// Start is called before the first frame update
	void Start()
    {
        SetParticlesSystemsEmissionEnabled(FragrancesAllGood, true);

        SetParticlesSystemsEmissionEnabled(FragrancesRain, false);
        SetParticlesSystemsEmissionEnabled(FragrancesInsects, false);
        SetParticlesSystemsEmissionEnabled(FragrancesWater, false);
        SetParticlesSystemsEmissionEnabled(FragrancesNutrient, false);
    }

    // Update is called once per frame
    void Update()
    {
        SetParticlesSystemsEmissionEnabled(FragrancesAllGood, plant.HasEnoughNutrients() && plant.HasEnoughWater());

        SetParticlesSystemsEmissionEnabled(FragrancesNutrient, !plant.HasEnoughNutrients());
        SetParticlesSystemsEmissionEnabled(FragrancesWater, !plant.HasEnoughWater());
    }

    private void SetParticlesSystemsEmissionEnabled(ParticleSystem[] particles, bool enabled)
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.enableEmission = enabled;
        }
    }
}
