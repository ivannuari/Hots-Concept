using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuPage : Page
{
    [SerializeField] private Button b_akun;

    [SerializeField] private Button b_understand;
    [SerializeField] private Button b_remember;
    [SerializeField] private Button b_apply;
    [SerializeField] private Button b_analyse;
    [SerializeField] private Button b_evaluate;
    [SerializeField] private Button b_create;

    [SerializeField] private TMP_Text label_nama;
    [SerializeField] private TMP_Text label_role;

    private void OnEnable()
    {
        Setup();
    }

    private void Start()
    {
        b_akun.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.MoreMenu));
        
        b_understand.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Understand));
        b_remember.onClick.AddListener(() =>
        {
            if(GameManager.Instance.GetTugasRemember() == null)
            {
                GameManager.Instance.ChangeState(GameState.Remember);
                return;
            }
            if (string.IsNullOrEmpty(GameManager.Instance.GetTugasRemember().waktuPengerjaan))
            {
                GameManager.Instance.ChangeState(GameState.Remember);
            }
            else
            {
                GameManager.Instance.ChangeState(GameState.RememberResult);
            }
        });

        b_apply.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Apply));
        b_analyse.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Analyse));
        b_evaluate.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Evaluate));
        b_create.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Create));
    }

    private void Setup()
    {
        if(GameManager.Instance.GetPlayerData() == null)
        {
            StartCoroutine("CheckSetup");
            return;
        }
        PlayerData data = GameManager.Instance.GetPlayerData();
        label_nama.text = $"Halo, {data.username}";
        label_role.text = $"Saya mendaftar sebagai {data.role}.";

        if(data.role == "Guru")
        {
            return;
        }

        b_apply.interactable = GameManager.Instance.GetTugasRemember().nilai >= 70;
        b_analyse.interactable = GameManager.Instance.GetTugasRemember().nilai >= 70;
        b_evaluate.interactable = GameManager.Instance.GetTugasAnalyze().nilai >= 70;
        b_create.interactable = GameManager.Instance.GetTugasEvaluate().nilai >= 70;
    }

    IEnumerator CheckSetup()
    {
        FirebaseManager.Instance.InitializeFirebase();
        yield return new WaitForSeconds(1.5f);
        Setup();
    }
}
