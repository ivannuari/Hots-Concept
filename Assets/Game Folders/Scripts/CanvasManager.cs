using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] protected Page[] allPages;

    private void Awake()
    {
        GameManager.Instance.OnStateChanged += Instance_OnStateChanged;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnStateChanged -= Instance_OnStateChanged;
    }

    private void Instance_OnStateChanged(GameState newState)
    {
        switch (newState)
        {
            case GameState.Login:
                ShowPage(PageName.Login);
                break;
            case GameState.Register:
                ShowPage(PageName.Register);
                break;
            case GameState.Menu:
                ShowPage(PageName.Menu);
                break;
            case GameState.Understand:
                ShowPage(PageName.Understand);
                break;
            case GameState.Plan:
                ShowPage(PageName.Plan);
                break;
            case GameState.Prepare:
                ShowPage(PageName.Prepare);
                break;
            case GameState.Profile:
                ShowPage(PageName.Profile);
                break;
            case GameState.Remember:
                ShowPage(PageName.Remember);
                break;
            case GameState.Apply:
                ShowPage(PageName.Apply);
                break;
            case GameState.Create:
                ShowPage(PageName.Create);
                break;
            case GameState.Analyse:
                ShowPage(PageName.Analyse);
                break;
            case GameState.Evaluate:
                ShowPage(PageName.Evaluate);
                break;
            case GameState.Practice:
                ShowPage(PageName.Practice);
                break;
            case GameState.Loading:
                ShowPage(PageName.Loading);
                break;
            case GameState.RememberResult:
                ShowPage(PageName.RememberResult);
                break;
            case GameState.MoreMenu:
                ShowPage(PageName.MoreMenu);
                break;
            case GameState.CommonEnglish:
                ShowPage(PageName.CommongEnglish);
                break;
            case GameState.References:
                ShowPage(PageName.References);
                break;
        }
    }

    public void ShowPage(PageName findNama)
    {
        foreach (var p in allPages)
        {
            p.gameObject.SetActive(false);
        }

        Page findPage = Array.Find(allPages, p => p.nama == findNama);
        findPage?.gameObject.SetActive(true);
    }
}
