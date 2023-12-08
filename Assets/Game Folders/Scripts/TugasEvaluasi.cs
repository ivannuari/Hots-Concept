using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TugasEvaluasi : MonoBehaviour
{
    [SerializeField] private Button b_submit;
    [SerializeField] private int currentVideo;
    [SerializeField] private LembarTugasEvaluate[] lembarTugas;
    [SerializeField] private LembarJawabEvaluate lembarJawab;

    private EvaluatePage page;

    private void Start()
    {
        page = GetComponentInParent<EvaluatePage>();

        b_submit.onClick.AddListener(SubmitJawaban);

        foreach (var item in lembarTugas)
        {
            item.Init();
        }
    }

    private void SubmitJawaban()
    {
        for (int i = 0; i < lembarTugas.Length; i++)
        {
            lembarJawab.allSoals[i].nilai = Mathf.FloorToInt(lembarTugas[i].sliderNilai.value);
            lembarJawab.allSoals[i].komentar = lembarTugas[i].inputJawaban.text;
        }

        if (string.IsNullOrEmpty(lembarTugas[0].inputJawaban.text) && string.IsNullOrEmpty(lembarTugas[1].inputJawaban.text) && string.IsNullOrEmpty(lembarTugas[2].inputJawaban.text))
        {
            GameManager.Instance.CreateNotification("jawaban harus diisi!");
            return;
        }
        if (string.IsNullOrEmpty(lembarTugas[3].inputJawaban.text) && string.IsNullOrEmpty(lembarTugas[4].inputJawaban.text) && string.IsNullOrEmpty(lembarTugas[5].inputJawaban.text))
        {
            GameManager.Instance.CreateNotification("jawaban harus diisi!");
            return;
        }
        if (string.IsNullOrEmpty(lembarTugas[6].inputJawaban.text) && string.IsNullOrEmpty(lembarTugas[7].inputJawaban.text))
        {
            GameManager.Instance.CreateNotification("jawaban harus diisi!");
            return;
        }

        lembarJawab.selesai = true;
        page.SetTugasEvaluate(lembarJawab, currentVideo);

        GameManager.Instance.CreateNotification("Tugas Selesai!");
        gameObject.SetActive(false);
    }

    public void SetCurrentSoal(int n)
    {
        currentVideo = n;
    }
}


[System.Serializable]
public class LembarTugasEvaluate
{
    public Slider sliderNilai;
    public TMP_Text labelNilai;
    public TMP_InputField inputJawaban;

    public void Init()
    {
        sliderNilai.onValueChanged.AddListener((n) => labelNilai.text = Mathf.FloorToInt(n).ToString());
    }
}