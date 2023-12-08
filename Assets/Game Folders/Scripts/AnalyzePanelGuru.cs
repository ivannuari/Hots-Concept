using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnalyzePanelGuru : MonoBehaviour
{
    private AllAnalyzeTugas baseData;
    [SerializeField] private TugasAnalyze[] allTugas;

    [SerializeField] private ItemLembarJawabanAnalyze lembarJawab;
    [SerializeField] private Transform content;

    private int selectedSiswa;

    [Header("PANEL PENILAIAN")]
    [SerializeField] private GameObject panel_penilaian;
    [SerializeField] private TMP_Text[] listJawaban;
    [SerializeField] private TMP_InputField input_nilai;
    [SerializeField] private TMP_InputField input_feedback;
    [SerializeField] private Button b_submit;

    private void OnEnable()
    {
        FirebaseManager.Instance.GetAllAnalyzeTugas(OnGetTugas);
    }

    private void Start()
    {
        b_submit.onClick.AddListener(HandleSubmit);
    }

    private void HandleSubmit()
    {
        if(string.IsNullOrEmpty(input_nilai.text) || string.IsNullOrEmpty(input_feedback.text))
        {
            GameManager.Instance.CreateNotification("isi bagian kosong!");
            return;
        }

        baseData.tugas[selectedSiswa].nilai = int.Parse(input_nilai.text);
        baseData.tugas[selectedSiswa].feedback = input_feedback.text;


        FirebaseManager.Instance.NilaiTugasAnalyze(baseData);
        panel_penilaian.SetActive(false);

        allTugas = null;
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
        FirebaseManager.Instance.GetAllAnalyzeTugas(OnGetTugas);
    }

    private void OnDisable()
    {
        panel_penilaian.SetActive(false);
        allTugas = null;
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
    }

    private void OnGetTugas(AllAnalyzeTugas tugas)
    {
        baseData = tugas;
        allTugas = tugas.tugas.ToArray();

        for (int i = 0; i < allTugas.Length; i++)
        {
            ItemLembarJawabanAnalyze j = Instantiate(lembarJawab, content);
            j.Initiate(allTugas[i] , i);
        }
    }

    public void OpenPanelPenilaian(int nomor)
    {
        selectedSiswa = nomor;
        panel_penilaian.SetActive(true);

        for (int i = 0; i < allTugas[nomor].lembarJawaban.Length; i++)
        {
            listJawaban[i].text = $"{i + 1}. {allTugas[nomor].lembarJawaban[i].jawaban}";
        }
    }
}
