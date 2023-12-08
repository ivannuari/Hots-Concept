using TMPro;
using UnityEngine;

public class ItemLembarJawabanRemember : MonoBehaviour
{
    [SerializeField] private TMP_Text label_nama;
    [SerializeField] private TMP_Text label_nim;
    [SerializeField] private TMP_Text label_waktuPengerjaan;
    [SerializeField] private TMP_Text label_nilai;


    public void Initiate(TugasRemember data)
    {
        label_nama.text = data.nama;
        label_nim.text = data.nim;
        label_waktuPengerjaan.text = $"Waktu Pengerjaan : {data.waktuPengerjaan}";
        label_nilai.text = $"Nilai : {data.nilai}";
    }
}
