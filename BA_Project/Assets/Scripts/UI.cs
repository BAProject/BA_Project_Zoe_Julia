using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    private GameObject _treeUi;

    public void ShowTreeUi()
    {
        _treeUi.SetActive(true);
    }

    public void HideTreeUi()
    {
        _treeUi.SetActive(false);
    }
}
