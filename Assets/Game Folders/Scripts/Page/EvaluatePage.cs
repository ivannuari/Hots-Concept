using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class EvaluatePage : Page
{
    [SerializeField] private Button b_back;
    [SerializeField] private Button[] b_video;

    [SerializeField] private GameObject[] allPanels;

    [Header("Tugas Video Panel")]
    [SerializeField] private TugasEvaluasi tugas;
    [SerializeField] private VideoPlayer vp;
    [SerializeField] private VideoClip[] allKlips;

    [Header("RESULT MURID")]
    [SerializeField] private TMP_Text label_nama;
    [SerializeField] private TMP_Text label_nim;
    [SerializeField] private TMP_Text label_waktu;
    [SerializeField] private TMP_Text label_nilai;
    [SerializeField] private TMP_Text label_feedback;

    [SerializeField] private GameObject panel_belumLulus;
    [SerializeField] private Button b_ulangTest;

    [SerializeField] private TugasEvaluate lembarJawabTemp;

    private void OnEnable()
    {
        allPanels[0].SetActive(GameManager.Instance.GetPlayerData().role == "Guru");
        allPanels[1].SetActive(GameManager.Instance.GetPlayerData().role == "Murid");
        allPanels[2].SetActive(false);

        if(GameManager.Instance.GetTugasEvaluate() == null || GameManager.Instance.GetPlayerData().role == "Guru")
        {
            return;
        }

        if (GameManager.Instance.GetTugasEvaluate().tugasVideos == null || GameManager.Instance.GetTugasEvaluate().tugasVideos.Length < 1)
        {
            return;
        }

        if (GameManager.Instance.GetTugasEvaluate().tugasVideos != null)
        {
            for (int i = 0; i < b_video.Length; i++)
            {
                b_video[i].interactable = !GameManager.Instance.GetTugasEvaluate().tugasVideos[i].selesai;
            }
        }

        if (GameManager.Instance.GetTugasEvaluate().tugasVideos[0].selesai && GameManager.Instance.GetTugasEvaluate().tugasVideos[1].selesai && GameManager.Instance.GetTugasEvaluate().tugasVideos[2].selesai)
        {
            allPanels[0].SetActive(false);
            allPanels[1].SetActive(false);
            allPanels[2].SetActive(true);

            label_nim.text = $"NIM : {GameManager.Instance.GetTugasEvaluate().nim}";
            label_nama.text = $"Nama : {GameManager.Instance.GetTugasEvaluate().nama}";
            label_waktu.text = $"Waktu : {GameManager.Instance.GetTugasEvaluate().waktuPengerjaan}";
            label_nilai.text = $"Nilai : {GameManager.Instance.GetTugasEvaluate().nilai}";
            label_feedback.text = $"Feedback : <br>{GameManager.Instance.GetTugasEvaluate().feedback}";

            if (GameManager.Instance.GetTugasEvaluate().nilai < 70 && !string.IsNullOrEmpty(GameManager.Instance.GetTugasEvaluate().feedback))
            {
                panel_belumLulus.SetActive(true);

                b_ulangTest.onClick.AddListener(() =>
                {
                    allPanels[0].SetActive(false);
                    allPanels[1].SetActive(true);
                    allPanels[2].SetActive(false);

                    foreach (var b in b_video)
                    {
                        b.interactable = true;
                    }
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

        b_video[0].onClick.AddListener(() =>
        {
            SetVideoPage(0);
        });
        b_video[1].onClick.AddListener(() =>
        {
            SetVideoPage(1);
        });
        b_video[2].onClick.AddListener(() =>
        {
            SetVideoPage(2);
        });

        lembarJawabTemp.userId = FirebaseManager.Instance.GetUserId();
        lembarJawabTemp.nama = GameManager.Instance.GetPlayerData().username;
        lembarJawabTemp.nim = GameManager.Instance.GetPlayerData().nim;
        lembarJawabTemp.waktuPengerjaan = DateTime.Now.ToString();

        if (GameManager.Instance.GetTugasEvaluate() == null || GameManager.Instance.GetPlayerData().role == "Guru")
        {
            return;
        }

        if (GameManager.Instance.GetTugasEvaluate().tugasVideos == null || GameManager.Instance.GetTugasEvaluate().tugasVideos.Length < 1)
        {
            return;
        }

        
    }

    private void SetVideoPage(int n)
    {
        tugas.gameObject.SetActive(true);
        tugas.SetCurrentSoal(n);
        vp.clip = allKlips[n];
        vp.Play();
    }

    public TugasEvaluate GetTugasEvaluate()
    {
        return lembarJawabTemp;
    }

    public void SetTugasEvaluate(LembarJawabEvaluate lembarJawab , int nomor)
    {
        if(IsArrayEmpty())
        {
            GameManager.Instance.GetTugasEvaluate().tugasVideos = new LembarJawabEvaluate[3];
        }
        else
        {
            for (int i = 0; i < lembarJawabTemp.tugasVideos.Length; i++)
            {
                lembarJawabTemp.tugasVideos[i] = GameManager.Instance.GetTugasEvaluate().tugasVideos[i];
            }
        }
        
        lembarJawabTemp.tugasVideos[nomor] = lembarJawab;
        b_video[nomor].interactable = false;

        GameManager.Instance.SetupTugasEvaluate(lembarJawabTemp);
        FirebaseManager.Instance.SaveTugasEvaluate(lembarJawabTemp);
    }

    private bool IsArrayEmpty()
    {
        if (GameManager.Instance.GetTugasEvaluate().tugasVideos == null || GameManager.Instance.GetTugasEvaluate().tugasVideos.Length == 0)
        { 
            return true; 
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.GetTugasEvaluate().tugasVideos.Length; i++)
            {
                if (GameManager.Instance.GetTugasEvaluate().tugasVideos[i] != null)
                {
                    return false;
                }
            }
            return false;
        }
    }
}