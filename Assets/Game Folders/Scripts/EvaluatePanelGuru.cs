using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EvaluatePanelGuru : MonoBehaviour
{
    private AllEvaluateTugas baseData;
    [SerializeField] private TugasEvaluate[] allTugas;

    [SerializeField] private ItemLembarJawabanEvaluate lembarJawab;
    [SerializeField] private Transform content;

    private int selectedSiswa;

    [Header("PANEL PENILAIAN")]
    [SerializeField] private GameObject panel_penilaian;

    [SerializeField] private LembarInputGuru panel_input;

    [SerializeField] private TMP_InputField input_nilai;
    [SerializeField] private TMP_InputField input_feedback;
    [SerializeField] private Button b_submit;

    private void OnEnable()
    {
        FirebaseManager.Instance.GetAllEvaluateTugas(OnGetTugas);
    }
    private void Start()
    {
        b_submit.onClick.AddListener(HandleSubmit);
    }

    private void HandleSubmit()
    {
        if (string.IsNullOrEmpty(input_nilai.text) || string.IsNullOrEmpty(input_feedback.text))
        {
            GameManager.Instance.CreateNotification("isi bagian kosong!");
            return;
        }

        baseData.tugas[selectedSiswa].nilai = int.Parse(input_nilai.text);
        baseData.tugas[selectedSiswa].feedback = input_feedback.text;

        FirebaseManager.Instance.NilaiTugasEvaluate(baseData);
        panel_penilaian.SetActive(false);

        allTugas = null;
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
        FirebaseManager.Instance.GetAllEvaluateTugas(OnGetTugas);
    }

    private void OnDisable()
    {
        allTugas = null;
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
    }

    private void OnGetTugas(AllEvaluateTugas tugas)
    {
        baseData = tugas;
        allTugas = tugas.tugas.ToArray();

        for (int i = 0; i < allTugas.Length; i++)
        {
            ItemLembarJawabanEvaluate j = Instantiate(lembarJawab, content);
            j.Initiate(allTugas[i] , i);
        }
    }

    public void OpenPanelPenilaian(int nomor)
    {
        selectedSiswa = nomor;
        panel_penilaian.SetActive(true);

        for (int i = 0; i < allTugas[nomor].tugasVideos[0].allSoals.Length; i++)
        {
            panel_input.video1[i].label_nilai.text = $"Nilai : {allTugas[nomor].tugasVideos[0].allSoals[i].nilai}";
            panel_input.video1[i].label_komentar.text = $"Komentar : {allTugas[nomor].tugasVideos[0].allSoals[i].komentar}";
        }
        for (int i = 0; i < allTugas[nomor].tugasVideos[1].allSoals.Length; i++)
        {
            panel_input.video2[i].label_nilai.text = $"Nilai : {allTugas[nomor].tugasVideos[1].allSoals[i].nilai}";
            panel_input.video2[i].label_komentar.text = $"Komentar : {allTugas[nomor].tugasVideos[1].allSoals[i].komentar}";
        }
        for (int i = 0; i < allTugas[nomor].tugasVideos[2].allSoals.Length; i++)
        {
            panel_input.video3[i].label_nilai.text = $"Nilai : {allTugas[nomor].tugasVideos[2].allSoals[i].nilai}";
            panel_input.video3[i].label_komentar.text = $"Komentar : {allTugas[nomor].tugasVideos[2].allSoals[i].komentar}";
        }
    }
}

[System.Serializable]
public class LembarInputGuru
{
    public LembarInput[] video1;
    public LembarInput[] video2;
    public LembarInput[] video3;
}

[System.Serializable]
public class LembarInput
{
    public TMP_Text label_nilai;
    public TMP_Text label_komentar;
}
