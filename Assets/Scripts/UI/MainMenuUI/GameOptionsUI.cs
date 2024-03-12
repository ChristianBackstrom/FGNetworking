using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameOptionsUI : MonoBehaviour
{
    private UIDocument _uIDocument;
    private VisualElement _rootElement;


    private Toggle _unlimitedLivesToggle;

    private void Awake()
    {
        _uIDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        if(_uIDocument == null) return;
        _rootElement = _uIDocument.rootVisualElement;

        _unlimitedLivesToggle = _rootElement.Q<Toggle>("UnlimitedLives");
        _unlimitedLivesToggle.value = Convert.ToBoolean(PlayerPrefs.GetInt("UnlimitedLives"));
        _unlimitedLivesToggle.RegisterValueChangedCallback<bool>(evt =>
        {
            PlayerPrefs.SetInt("UnlimitedLives", Convert.ToInt32(evt.newValue));
        });
    }
}
