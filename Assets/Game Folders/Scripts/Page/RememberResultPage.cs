using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RememberResultPage : Page
{
    [SerializeField] private Button b_back;

    [SerializeField] private TMP_Text label_nim;
    [SerializeField] private TMP_Text label_nama;
    [SerializeField] private TMP_Text label_waktu;
    [SerializeField] private TMP_Text label_nilai;

    [SerializeField] private TMP_Text label_result;
    [SerializeField] private Button b_ulang;

    private void Start()
    {
        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Menu));
        b_ulang.onClick.AddListener(() =>
        {
            TugasRemember newTugas = new TugasRemember(null, null, null, null, 0, null);
            GameManager.Instance.SetupTugasRemember(newTugas);

            GameManager.Instance.ChangeState(GameState.Remember);
        });
    }

    private void OnEnable()
    {
        label_nim.text = $"NIM : {GameManager.Instance.GetTugasRemember().nim}";
        label_nama.text = $"Nama : {GameManager.Instance.GetTugasRemember().nama}";
        label_waktu.text = $"Waktu : {GameManager.Instance.GetTugasRemember().waktuPengerjaan}";
        label_nilai.text = $"Nilai : {GameManager.Instance.GetTugasRemember().nilai}";

        if(GameManager.Instance.GetTugasRemember().nilai >= 70)
        {
            label_result.text = $"Selamat, kamu bisa membuka menu selanjutnya dan kerjakan tugas lain nya.";
            b_ulang.gameObject.SetActive(false);
        }
        else
        {
            label_result.text = $"Kamu belum lulus, Belajar lagi dari Menu Understand, dan mulai ulang test nya.";
            b_ulang.gameObject.SetActive(true);
        }
    }
}
