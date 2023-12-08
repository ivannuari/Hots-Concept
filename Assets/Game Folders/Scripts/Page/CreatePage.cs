using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatePage : Page
{
    [SerializeField] private Button b_back;
    [SerializeField] private GameObject[] allPanels;

    [Header("MURID")]
    [SerializeField] private TMP_InputField[] input_text;
    [SerializeField] private TMP_InputField input_link;
    [SerializeField] private Button b_kirim;

    [Header("RESULT MURID")]
    [SerializeField] private TMP_Text label_nama;
    [SerializeField] private TMP_Text label_nim;
    [SerializeField] private TMP_Text label_waktu;
    [SerializeField] private TMP_Text label_nilai;
    [SerializeField] private TMP_Text label_feedback;

    [SerializeField] private GameObject panel_belumLulus;
    [SerializeField] private Button b_ulangTest;

    [SerializeField] private TugasCreate lembarJawabTemp;

    private void Start()
    {
        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Menu));
        b_kirim.onClick.AddListener(HandleKirimTugas);
    }

    private void OnEnable()
    {
        allPanels[0].SetActive(GameManager.Instance.GetPlayerData().role == "Guru");
        allPanels[1].SetActive(GameManager.Instance.GetPlayerData().role == "Murid");
        allPanels[2].SetActive(false);

        if(GameManager.Instance.GetTugasCreate() == null)
        {
            return;
        }

        if (!string.IsNullOrEmpty(GameManager.Instance.GetTugasCreate().waktuPengerjaan) && GameManager.Instance.GetPlayerData().role == "Murid")
        {
            allPanels[0].SetActive(false);
            allPanels[1].SetActive(false);
            allPanels[2].SetActive(true);

            label_nim.text = $"NIM : {GameManager.Instance.GetTugasCreate().nim}";
            label_nama.text = $"Nama : {GameManager.Instance.GetTugasCreate().nama}";
            label_waktu.text = $"Waktu : {GameManager.Instance.GetTugasCreate().waktuPengerjaan}";
            label_nilai.text = $"Nilai : {GameManager.Instance.GetTugasCreate().nilai}";
            label_feedback.text = $"Feedback : <br>{GameManager.Instance.GetTugasCreate().feedback}";

            if (GameManager.Instance.GetTugasCreate().nilai < 60 && !string.IsNullOrEmpty(GameManager.Instance.GetTugasCreate().feedback))
            {
                panel_belumLulus.SetActive(true);
                b_ulangTest.onClick.AddListener(() =>
                {
                    allPanels[0].SetActive(false);
                    allPanels[1].SetActive(true);
                    allPanels[2].SetActive(false);
                });
            }
            else
            {
                panel_belumLulus.SetActive(false);
            }
        }
    }

    private void HandleKirimTugas()
    {
        if(string.IsNullOrEmpty(input_link.text))
        {
            GameManager.Instance.CreateNotification("isi link tugas");
            return;
        }
        for (int i = 0; i < input_text.Length; i++)
        {
            if(string.IsNullOrEmpty(input_text[i].text))
            {
                GameManager.Instance.CreateNotification("isi paragraph tugas");
                return;
            }
        }

        //kirim Tugas
        b_kirim.interactable = false;
        lembarJawabTemp.userId = FirebaseManager.Instance.GetUserId();
        lembarJawabTemp.nama = GameManager.Instance.GetPlayerData().username;
        lembarJawabTemp.nim = GameManager.Instance.GetPlayerData().nim;
        lembarJawabTemp.waktuPengerjaan = DateTime.Now.ToString();
        lembarJawabTemp.linkTugas = input_link.text;
        string temp = "";
        for (int i = 0; i < input_text.Length; i++)
        {
            temp += input_text[i].text;
        }
        lembarJawabTemp.textTugas = temp;

        GameManager.Instance.SetupTugasCreate(lembarJawabTemp);
        FirebaseManager.Instance.SaveTugasCreate(lembarJawabTemp);

        GameManager.Instance.CreateNotification("tugas terkirim");
        GameManager.Instance.ChangeState(GameState.Menu);


    }
}
