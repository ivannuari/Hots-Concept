using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemLembarJawabanEvaluate : MonoBehaviour
{
    [SerializeField] private TMP_Text label_nama;
    [SerializeField] private TMP_Text label_nim;
    [SerializeField] private TMP_Text label_waktuPengerjaan;
    [SerializeField] private TMP_Text label_nilai;

    [SerializeField] private Button b_nilai;

    private int nomor;

    public void Initiate(TugasEvaluate data , int n)
    {
        label_nama.text = data.nama;
        label_nim.text = data.nim;
        label_waktuPengerjaan.text = $"Waktu Pengerjaan : {data.waktuPengerjaan}";
        label_nilai.text = $"Nilai : {data.nilai}";

        nomor = n;

        b_nilai.onClick.AddListener(() =>
        {
            GetComponentInParent<EvaluatePanelGuru>().OpenPanelPenilaian(nomor);
        });

        b_nilai.interactable = data.nilai == 0;
    }
}
