using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private RectTransform _cursorImage;
    [SerializeField] private GameObject _controllableUi;
    [SerializeField] private GameObject _treeUi;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        _cursorImage.position = Input.mousePosition;
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
}
