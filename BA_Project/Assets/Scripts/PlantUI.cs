using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantUI : MonoBehaviour
{
    public Image waterFill;
    public Image nutrientFill;

    public void SetWaterFill(float value)
    {
        waterFill.fillAmount = value;
    }

    public void SetNutrientFill(float value)
    {
        nutrientFill.fillAmount = value;
    }
}
