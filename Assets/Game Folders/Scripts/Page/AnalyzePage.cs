using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnalyzePage : Page
{
    [SerializeField] private Button b_back;

    [SerializeField] private GameObject[] allPanels;

    [Header("PANEL MURID")]
    [SerializeField] private Button[] b_videos;
    [SerializeField] private Button b_soal;

    [SerializeField] private Button[] b_youtubes;
    [SerializeField , TextArea(2,2)] private string[] urls;

    [SerializeField] private Sprite[] buttonSprite;

    [SerializeField] private GameObject[] panelVideos;
    [SerializeField] private GameObject LatihanSoal;

    [Header("RESULT MURID")]
    [SerializeField] private TMP_Text label_nama;
    [SerializeField] private TMP_Text label_nim;
    [SerializeField] private TMP_Text label_waktu;
    [SerializeField] private TMP_Text label_nilai;
    [SerializeField] private TMP_Text label_feedback;

    [SerializeField] private GameObject panel_belumLulus;
    [SerializeField] private Button b_ulangTest;

    [SerializeField] private TugasAnalyze lembarJawabTemp;

    private void OnEnable()
    {
        allPanels[0].SetActive(GameManager.Instance.GetPlayerData().role == "Guru");
        allPanels[1].SetActive(GameManager.Instance.GetPlayerData().role == "Murid");
        allPanels[2].SetActive(false);

        if (GameManager.Instance.GetTugasAnalyze() == null || GameManager.Instance.GetPlayerData().role == "Guru")
        {
            return;
        }

        if (!string.IsNullOrEmpty(GameManager.Instance.GetTugasAnalyze().waktuPengerjaan))
        {
            allPanels[0].SetActive(false);
            allPanels[1].SetActive(false);
            allPanels[2].SetActive(true);

            label_nim.text = $"NIM : {GameManager.Instance.GetTugasAnalyze().nim}";
            label_nama.text = $"Nama : {GameManager.Instance.GetTugasAnalyze().nama}";
            label_waktu.text = $"Waktu : {GameManager.Instance.GetTugasAnalyze().waktuPengerjaan}";
            label_nilai.text = $"Nilai : {GameManager.Instance.GetTugasAnalyze().nilai}";
            label_feedback.text = $"Feedback : <br>{GameManager.Instance.GetTugasAnalyze().feedback}";

            if(GameManager.Instance.GetTugasAnalyze().nilai < 70 && !string.IsNullOrEmpty(GameManager.Instance.GetTugasAnalyze().feedback))
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

    private void Start()
    {
        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Menu));

        b_videos[0].onClick.AddListener(() => SetupButton(0));
        b_videos[1].onClick.AddListener(() => SetupButton(1));
        b_videos[2].onClick.AddListener(() => SetupButton(2));

        b_youtubes[0].onClick.AddListener(() => Application.OpenURL(urls[0]));
        b_youtubes[1].onClick.AddListener(() => Application.OpenURL(urls[1]));
        b_youtubes[2].onClick.AddListener(() => Application.OpenURL(urls[2]));

        b_soal.onClick.AddListener(() => 
        {
            LatihanSoal.gameObject.SetActive(true);
        });

        lembarJawabTemp.userId = FirebaseManager.Instance.GetUserId();
        lembarJawabTemp.nama = GameManager.Instance.GetPlayerData().username;
        lembarJawabTemp.nim = GameManager.Instance.GetPlayerData().nim;
        lembarJawabTemp.waktuPengerjaan = DateTime.Now.ToString();
    }

    public void SetTugasAnalisa(LembarJawabAnalyze[] lembarJawab)
    {
        lembarJawabTemp.lembarJawaban = lembarJawab;
        b_soal.interactable = false;

        GameManager.Instance.SetupTugasAnalyze(lembarJawabTemp);
        FirebaseManager.Instance.SaveTugasAnalyze(lembarJawabTemp);
    }
    private void SetupButton(int n)
    {
        if (!panelVideos[n].activeInHierarchy)
        {
            panelVideos[n].gameObject.SetActive(true);
            b_videos[n].GetComponent<Image>().sprite = buttonSprite[1];
        }
        else
        {
            panelVideos[n].gameObject.SetActive(false);
            b_videos[n].GetComponent<Image>().sprite = buttonSprite[0];
        }
    }
}
