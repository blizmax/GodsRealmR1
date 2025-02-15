﻿using System;
using UI;
using UI.CustomControls;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using UnityEngine.UIElements;

public class NewUIManager : MonoBehaviour
{
    //Localization
    [SerializeField]
    private LocalizedStringTable UILocalTable;
    [SerializeField]
    private LocalizedStringTable ItemsLocalTable;
    [SerializeField]
    private LocalizedStringTable CharacterLocalTable;

    [HideInInspector]
    public StringTable UITable;
    [HideInInspector]
    public StringTable ItemsTable;
    [HideInInspector]
    public StringTable CharacterTable;

    public static NewUIManager Instance;
    public GameObject PauseMenu;
    public GameObject SettingsMenu;
    public GameObject LoadingScreen;
    public GameObject Inventory;
    public GameObject InGameUI;
    public UIDocument ModalWindow;

    private GameObject PreviousMenu;
    public GameObject CurrentMenu;

    private bool CanGoBack = true;
    public bool IsModalWindow = false;
    public GameObject Blur;

    public event Action OnMenuOpen = delegate { };

    public event Action OnMenuClose = delegate { };

    private void Awake()
    {
        Instance = this;

        var playerInput = GetComponent<PlayerInput>();

        var ExitButton = playerInput.actions["Exit"];
        var InventoryButton = playerInput.actions["Inventory"];

        ExitButton.performed += ctx =>
        {
            if (CurrentMenu == null)
            {
                OpenMenu(PauseMenu);
            }
            else
            {
                GoBack();
            }
        };

        InventoryButton.performed += ctx => OpenMenu(Inventory);

        OnMenuOpen += () =>
        {
            Blur.SetActive(true);
            InGameUI.SetActive(false);
            Time.timeScale = 0;
        };

        OnMenuClose += () =>
        {
            Blur.SetActive(false);
            InGameUI.SetActive(true);
            Time.timeScale = 1;
        };

        //Loaclization
        UITable = UILocalTable.GetTable();
        CharacterTable = CharacterLocalTable.GetTable();
        ItemsTable = ItemsLocalTable.GetTable();
    }

    public void ChangeMenu(GameObject NextMenu)
    {

    }

    public void ChangeScene(string SceneName, GameObject PreviousMenu)
    {

    }

    public void GoBack()
    {
        if (CanGoBack)
        {
            CurrentMenu.SetActive(false);
            if (PreviousMenu != null & !IsModalWindow)
            {
                PreviousMenu.SetActive(true);
                CurrentMenu = PreviousMenu;
                PreviousMenu = null;
            }
            else if (IsModalWindow)
            {
                ModalWindow.rootVisualElement.Q<ModalWindow>().style.display = DisplayStyle.None;
                CurrentMenu.SetActive(true);
            }
            else
            {
                CurrentMenu = null;
                OnMenuClose();
            }
        }
    }

    public void OpenMenu(GameObject menu)
    {
        if (menu == null) GoBack();

        if (!menu.active & CurrentMenu != menu & PreviousMenu != menu)
        {
            menu.SetActive(true);
            CurrentMenu = menu;
            OnMenuOpen();
        }
        else
        {
            GoBack();
        }
    }

    /// <summary>
    /// Установка нового текста и его локализация
    /// </summary>
    /// <param _name="textElement">Текстовый элемент</param>
    /// <param _name="key">Ключ в таблице локализации</param>
    /// <param _name="table">Таблица локализации</param>
    public void ChangeLabelsText(TextElement textElement, string key, StringTable table)
    {
        if (!string.IsNullOrEmpty(key))
        {
            StringTableEntry entry = table[key];
            if (entry != null)
                textElement.text = entry.LocalizedValue;
            else
                textElement.text = key;
        }
    }

    public void ShowModalWindow(ModalWindowType type, string Caption, string Title, Action Success)
    {
        ModalWindow.gameObject.SetActive(false);
        ModalWindow.gameObject.SetActive(true);

        IsModalWindow = true;
        ModalWindow.rootVisualElement.Q<ModalWindow>().Show(type, Caption, Title, Success, CurrentMenu);
    }
}