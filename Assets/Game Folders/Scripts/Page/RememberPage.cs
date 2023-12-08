using System;
using UnityEngine;
using UnityEngine.UI;

public class RememberPage : Page
{
    [SerializeField] private Button b_back;
    [SerializeField] private Button b_submit;
    [SerializeField] private Button b_mulai;

    [SerializeField] private GameObject[] allPanels;

    [SerializeField] private DaftarSoal currentSoal;
    [SerializeField] private BoxSoal[] boxes;

    [SerializeField] private TugasRemember lembarJawabTemp;

    private void OnEnable()
    {
        if(GameManager.Instance.GetPlayerData().role == "Guru")
        {
            allPanels[0].SetActive(false);
            allPanels[1].SetActive(false);
            allPanels[2].SetActive(true);
        }
    }

    private void Start()
    {
        b_mulai.onClick.AddListener(() =>
        {
            allPanels[0].SetActive(false);
            allPanels[1].SetActive(true);

            lembarJawabTemp.userId = FirebaseManager.Instance.GetUserId();
            lembarJawabTemp.nama = GameManager.Instance.GetPlayerData().username;
            lembarJawabTemp.nim = GameManager.Instance.GetPlayerData().nim;
            lembarJawabTemp.waktuPengerjaan = DateTime.Now.ToString();

            b_back.gameObject.SetActive(false);
        });

        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Menu));
        b_submit.onClick.AddListener(() => 
        {
            //cari nilai
            LembarJawaban[] jawabanBenar = Array.FindAll(lembarJawabTemp.lembarJawaban, j => j.benar == true);
            lembarJawabTemp.nilai = jawabanBenar.Length * 5;

            GameManager.Instance.SetupTugasRemember(lembarJawabTemp);
            FirebaseManager.Instance.SaveTugasRemember(lembarJawabTemp);

            GameManager.Instance.CreateNotification("tugas terkirim");
            GameManager.Instance.ChangeState(GameState.RememberResult);
        });

        InitSoal();
        InitJawaban();
    }

    private void InitSoal()
    {
        for (int i = 0; i < currentSoal.semuaSoal.Length; i++)
        {
            boxes[i].Setup(currentSoal.semuaSoal[i]);
        }
    }

    public void InitJawaban()
    {
        for (int i = 0; i < boxes.Length; i++)
        {
            lembarJawabTemp.lembarJawaban[i].Jawaban = boxes[i].jawaban.ToString();
            lembarJawabTemp.lembarJawaban[i].benar = boxes[i].jawaban == currentSoal.semuaSoal[i].jawaban;
        }

        if(GameManager.Instance.GetPlayerData().role == "Guru")
        {
            for (int i = 0; i < boxes.Length; i++)
            {

            }
        }
    }
}



[System.Serializable]
public class Soal
{
    [TextArea(5,5)] public string pertanyaan;
    [TextArea(2,2)] public string[] opsis;

    public int jawaban;
}

[System.Serializable]
public class DaftarSoal
{
    public Soal[] semuaSoal;
}
