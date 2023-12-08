using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreatePanelGuru : MonoBehaviour
{
    private AllCreateTugas baseData;
    [SerializeField] private TugasCreate[] allTugas;

    [SerializeField] private ItemLembarJawabanCreate lembarJawab;
    [SerializeField] private Transform content;

    private int selectedSiswa;

    [Header("PANEL PENILAIAN")]
    [SerializeField] private GameObject panel_penilaian;
    [SerializeField] private TMP_Text label_link;
    [SerializeField] private Button b_openLink;

    [SerializeField] private TMP_Text label_text;

    [SerializeField] private TMP_InputField input_nilai;
    [SerializeField] private TMP_InputField input_feedback;
    [SerializeField] private Button b_submit;

    private void OnEnable()
    {
        FirebaseManager.Instance.GetAllCreateTugas(OnGetTugas);
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

        FirebaseManager.Instance.NilaiTugasCreate(baseData);
        panel_penilaian.SetActive(false);

        allTugas = null;
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
        FirebaseManager.Instance.GetAllCreateTugas(OnGetTugas);
    }

    private void OnDisable()
    {
        allTugas = null;
        foreach (Transform t in content)
        {
            Destroy(t.gameObject);
        }
    }

    private void OnGetTugas(AllCreateTugas tugas)
    {
        baseData = tugas;
        allTugas = tugas.tugas.ToArray();

        for (int i = 0; i < allTugas.Length; i++)
        {
            ItemLembarJawabanCreate j = Instantiate(lembarJawab, content);
            j.Initiate(allTugas[i] , i);
        }
    }

    public void OpenPanelPenilaian(int nomor)
    {
        selectedSiswa = nomor;
        panel_penilaian.SetActive(true);
        label_link.text = allTugas[nomor].linkTugas;
        label_text.text = allTugas[nomor].textTugas;
        b_openLink.onClick.AddListener(() => Application.OpenURL(allTugas[nomor].linkTugas));
    }
}
