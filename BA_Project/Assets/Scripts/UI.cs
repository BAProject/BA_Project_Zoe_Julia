using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private RectTransform _cursorImage;
    [SerializeField] private GameObject _controllableUi;
    [SerializeField] private GameObject _treeUi;

    [Header("Trees")]
    public PlantUI controlledTreePlantUI;
    public PlantUI mouseOverTreePlantUI;
    public Text sendNutrientsText;
    public GameObject rainUI;

    private bool mouseOverTreeActive = false;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        _cursorImage.position = Input.mousePosition;

        if(mouseOverTreeActive)
        {
            mouseOverTreePlantUI.transform.position = Input.mousePosition;
        }
    }

    public void ShowControllableUi()
    {
        _controllableUi.SetActive(true);
    }

    public void HideControllableUi()
    {
        _controllableUi.SetActive(false);
    }

    public void ShowTreeUi()
    {
        _treeUi.SetActive(true);
    }

    public void HideTreeUi()
    {
        _treeUi.SetActive(false);
    }

    public void SetControlledTreeActive(bool active)
    {
        controlledTreePlantUI.gameObject.SetActive(active);
    }

    public void SetMouseOverTreeActive(bool active)
    {
        mouseOverTreePlantUI.gameObject.SetActive(active);
        mouseOverTreeActive = active;
    }

    public void SetControlledTreeValues(Plant tree)
    {
        controlledTreePlantUI.SetNutrientFill(tree.GetNutrientFill());
        controlledTreePlantUI.SetWaterFill(tree.GetWaterFill());
    }

    public void SetMouseOverTreeValues(Plant tree)
    {
        mouseOverTreePlantUI.SetNutrientFill(tree.GetNutrientFill());
        mouseOverTreePlantUI.SetWaterFill(tree.GetWaterFill());
    }

    public void SetSendNutrientsTextActive(bool active)
    {
        sendNutrientsText.gameObject.SetActive(active);
    }

    public void SetRainUIActive(bool active)
    {
        rainUI.SetActive(active);
    }
}
