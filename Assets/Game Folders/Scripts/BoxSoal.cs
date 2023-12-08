using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxSoal : MonoBehaviour
{
    [SerializeField] private TMP_Text label_soal;
    [SerializeField] private TMP_Text[] label_opsis;

    [SerializeField] private Toggle[] allToogles;

    public int jawaban;

    private void Start()
    {
        allToogles = GetComponentsInChildren<Toggle>();

        foreach (var t in allToogles)
        {
            t.isOn = false;
        }
    }

    public void PilihJawaban(int n)
    {
        jawaban = n;
    }

    internal void Setup(Soal soal)
    {
        label_soal.text = soal.pertanyaan;
        for (int i = 0; i < soal.opsis.Length; i++)
        {
            label_opsis[i].text = soal.opsis[i];
        }
    }
}
