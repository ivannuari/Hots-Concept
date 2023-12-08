using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegisterPage : Page
{
    [SerializeField] private TMP_InputField input_username;
    [SerializeField] private TMP_InputField input_email;
    [SerializeField] private TMP_InputField input_password;
    [SerializeField] private TMP_InputField input_confirm;

    [SerializeField] private GameObject FormIsianData;

    [SerializeField] private TMP_Text label_nama;
    [SerializeField] private TMP_Text label_email;
    [SerializeField] private TMP_Text label_role;

    [SerializeField] private TMP_InputField input_nim;
    [SerializeField] private TMP_InputField input_prodi;
    [SerializeField] private TMP_InputField input_kampus;

    [SerializeField] private GameObject panelPilihPengajar;
    [SerializeField] private TMP_Dropdown selectPengajar;

    [SerializeField] private Button b_login, b_register, b_inputData;
    [SerializeField] private Toggle[] roles;


    private string tempNama, tempEmail, tempRole;

    private void Start()
    {
        b_login.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Login));
        b_inputData.onClick.AddListener(InputData);
        b_register.onClick.AddListener(Register);
    }

    private void InputData()
    {
        if (string.IsNullOrEmpty(input_username.text) || string.IsNullOrEmpty(input_email.text) || string.IsNullOrEmpty(input_password.text) || string.IsNullOrEmpty(input_confirm.text))
        {
            GameManager.Instance.CreateNotification("isi kotak isian!");
            return;
        }

        if (input_confirm.text != input_password.text)
        {
            GameManager.Instance.CreateNotification("password tidak sama!");
            return;
        }

        bool isGuru = roles[0].isOn;
        string role;

        if(isGuru)
        {
            role = "Guru";
        }
        else
        {
            role = "Murid";
        }

        GameManager.Instance.SetupPlayerData(input_username.text, role, null, input_email.text, null , null , null , null);
        
        FormIsianData.SetActive(true);
        PlayerData data = GameManager.Instance.GetPlayerData();
        label_nama.text = $"Nama : {data.username}";
        label_email.text = $"Email : {data.email}";
        label_role.text = $"Saya seorang {data.role}";

        FirebaseManager.Instance.GetListPengajar((List<string> temp) =>
        {
            panelPilihPengajar.SetActive(data.role == "Murid");
            selectPengajar.AddOptions(temp);
        });
    }

    private void Register()
    {
        if(string.IsNullOrEmpty(input_nim.text) || string.IsNullOrEmpty(input_prodi.text) || string.IsNullOrEmpty(input_kampus.text) || string.IsNullOrEmpty(selectPengajar.options[selectPengajar.value].text))
        {
            return;
        }

        PlayerData data = GameManager.Instance.GetPlayerData();
        GameManager.Instance.SetupPlayerData(data.username, data.role, null, data.email, input_nim.text, input_prodi.text, input_kampus.text , selectPengajar.options[selectPengajar.value].text);

        FirebaseManager.Instance.Register(input_email.text, input_password.text, onSucessRegister); 
    }

    private void onSucessRegister(bool isSuccess)
    {
        if (isSuccess)
        {
            Scene s = SceneManager.GetActiveScene();
            SceneManager.LoadScene(s.name);
        }
    }
}
