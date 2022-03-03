using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObject : MonoBehaviour
{
    public GameObject solidObject;
    public GameObject transparentObject;
    private void Awake()
    {
        ShowSolid();
    }

    public void ShowTransparent()
    {
        solidObject.SetActive(false);
        transparentObject.SetActive(true);
    }

    public void ShowSolid()
    {
        solidObject.SetActive(true);
        transparentObject.SetActive(false);
    }


}
