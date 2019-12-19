using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _treeUi;

    public void ShowTreeUi(Controllable tree)
    {
        _treeUi.SetActive(true);
    }

    public void HideTreeUi()
    {
        _treeUi.SetActive(false);
    }
}
