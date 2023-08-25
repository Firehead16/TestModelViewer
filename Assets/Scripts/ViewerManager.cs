using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewerManager : MonoBehaviour
{
    public static ViewerManager Instance;

    [SerializeField]
    private List<Model> models = new List<Model>();

    [SerializeField]
    private Model model;

    [SerializeField]
    private GameObject currentModel;

    private GameObject currentObject;

    private List<Transform> movableByDecomposition = new List<Transform>();

    [SerializeField]
    private TMP_Text NameText;

    [SerializeField]
    private TMP_Text DescriptionText;

    [SerializeField]
    private Transform ButtonsPlace;

    [SerializeField]
    private GameObject ButtonPrefab;

    private bool isPlaying;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button hideButton;

    [SerializeField]
    private Button showButton;

    [SerializeField]
    private Button decomposeButton;

    public GameObject CurrentObject 
    { 
        get => currentObject; 
        set => SetCurrentObject(value); 
    }
    public GameObject CurrentModel { get => currentModel; set => SetCurrentModel(value); }

    void Start()
    {
        Instance = this;

        SetModel(models[0]);

        SetButtons();

        playButton.onClick.AddListener(Play);
        hideButton.onClick.AddListener(Hide);
        showButton.onClick.AddListener(Show);
        decomposeButton.onClick.AddListener(Decompose);
    }

    private void SetButtons()
    {
        foreach (var model in models)
        {
            GameObject tempButton = Instantiate(ButtonPrefab, ButtonsPlace);
            tempButton.GetComponentInChildren<TMP_Text>().text = model.ModelName;
            tempButton.GetComponent<ButtonListener>().SetListener(model);
        }
    }

    private void SetCurrentObject(GameObject obj)
    {
        currentObject = obj;

        NameText.text = obj.name;
        
        if(currentObject.GetComponent<Description>() != null) 
        {
            DescriptionText.text = currentObject.GetComponent<Description>().DescriptionString;
        }
    }

    private void SetCurrentModel(GameObject obj)
    {
        isPlaying = false;

        currentModel = Instantiate(obj);

        movableByDecomposition.Clear();

        foreach (Transform item in currentModel.transform)
        {
            if(item.GetComponent<MovableByDecomposition>() != null)
            {
                movableByDecomposition.Add(item);
            }
        }
    }

    public void SetModel(Model modelNew)
    {
        Destroy(currentModel);

        model = modelNew;
        SetCurrentModel(modelNew.ModelPrefab);
        model.SetStartPositions();


        #region Сбросить кнопки

        decomposeButton.onClick.RemoveAllListeners();
        decomposeButton.GetComponentInChildren<TMP_Text>().text = "Разобрать";
        decomposeButton.onClick.AddListener(Decompose);

        playButton.onClick.RemoveAllListeners();
        playButton.GetComponentInChildren<TMP_Text>().text = "Воспроизвести";
        playButton.onClick.AddListener(Play);
        #endregion
    }

    public void Decompose()
    {
        switch (model.decomposeAxis)
        {
            case Model.DecomposeAxis.X:
                foreach (Transform item in movableByDecomposition)
                {
                    item.DOLocalMoveX(item.position.x * model.DecomposeStrength, 1.0f);
                }
                break;
            case Model.DecomposeAxis.Y:
                foreach (Transform item in movableByDecomposition)
                {
                    item.DOLocalMoveY(item.position.y * model.DecomposeStrength, 1.0f);
                }
                break; 
            case Model.DecomposeAxis.Z:
                foreach (Transform item in movableByDecomposition)
                {
                    item.DOLocalMoveZ(item.position.z * model.DecomposeStrength, 1.0f);
                }
                break;
        }

        decomposeButton.onClick.RemoveAllListeners();
        decomposeButton.onClick.AddListener(Compose);
        decomposeButton.GetComponentInChildren<TMP_Text>().text = "Собрать";
    }

    public void Compose()
    {
        for (int i = 0; i < movableByDecomposition.Count; i++)
        {
            movableByDecomposition[i].position = model.movableStartPos[i];
        }

        decomposeButton.onClick.RemoveAllListeners();
        decomposeButton.onClick.AddListener(Decompose);
        decomposeButton.GetComponentInChildren<TMP_Text>().text = "Разобрать";
    }

    public void Hide()
    {
        if(currentObject != null && currentObject.GetComponent<Hideable>() != null)
        {
            currentObject.gameObject.SetActive(false);
        }
    }

    public void Show()
    {
        foreach (Transform item in currentModel.transform)
        {
            item.gameObject.SetActive(true);
        }
    }

    public void Play()
    {
        foreach (Transform item in currentModel.transform)
        {
            if (item.GetComponent<IPlayable>() != null)
            {
                item.GetComponent<IPlayable>().Play();
            }
        }

        playButton.onClick.RemoveAllListeners();
        playButton.GetComponentInChildren<TMP_Text>().text = "Остановить";
        playButton.onClick.AddListener(Stop);
    }

    public void Stop()
    {
        foreach (Transform item in currentModel.transform)
        {
            if (item.GetComponent<IPlayable>() != null)
            {
                item.GetComponent<IPlayable>().Stop();
            }
        }

        playButton.onClick.RemoveAllListeners();
        playButton.GetComponentInChildren<TMP_Text>().text = "Воспроизвести";
        playButton.onClick.AddListener(Play);
    }
}
