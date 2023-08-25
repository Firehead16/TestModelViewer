using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonListener : MonoBehaviour
{
    private Model model;

    private Button button;

    void Click()
    {
        ViewerManager.Instance.SetModel(model);
    }

    public void SetListener(Model modelSet)
    {
        model = modelSet;
        button = GetComponent<Button>();
        button.onClick.AddListener(Click);
    }
}
