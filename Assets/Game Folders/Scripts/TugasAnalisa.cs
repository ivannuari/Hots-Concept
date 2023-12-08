using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TugasAnalisa : MonoBehaviour
{
    [SerializeField] private Button b_submit;
    [SerializeField] private Button b_back;

    [SerializeField] private LembarTugasAnalyze[] lembarTugas;
    [SerializeField] private LembarJawabAnalyze[] lembarJawab;

    private AnalyzePage page;

    private void Start()
    {
        page = GetComponentInParent<AnalyzePage>();

        b_submit.onClick.AddListener(() =>
        {
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
            if (string.IsNullOrEmpty(lembarTugas[6].inputJawaban.text) && string.IsNullOrEmpty(lembarTugas[7].inputJawaban.text) && string.IsNullOrEmpty(lembarTugas[8].inputJawaban.text) && string.IsNullOrEmpty(lembarTugas[9].inputJawaban.text))
            {
                GameManager.Instance.CreateNotification("jawaban harus diisi!");
                return;
            }

            for (int i = 0; i < lembarTugas.Length; i++)
            {
                lembarJawab[i].jawaban = lembarTugas[i].inputJawaban.text;
            }

            page.SetTugasAnalisa(lembarJawab);

            GameManager.Instance.CreateNotification("Tugas Selesai!");
            gameObject.SetActive(false);
        });

        b_back.onClick.AddListener(() => this.gameObject.SetActive(false));
    }
}

[System.Serializable]
public class LembarTugasAnalyze
{
    public int nomor;
    public TMP_InputField inputJawaban;
}
