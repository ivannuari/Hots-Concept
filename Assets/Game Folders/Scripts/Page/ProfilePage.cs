using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePage : Page
{
    [SerializeField] private Button b_back;

    [SerializeField] private TMP_Text label_username;
    [SerializeField] private TMP_Text label_nim;
    [SerializeField] private TMP_Text label_prodi;
    [SerializeField] private TMP_Text label_fakultas;
    [SerializeField] private TMP_Text label_role;

    private void Start()
    {
        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Menu));
    }

    private void OnEnable()
    {
        Setup();
    }

    private void Setup()
    {
        PlayerData data = GameManager.Instance.GetPlayerData();

        label_username.text = $"Nama    : <color=blue>{data.username}</color>";
        if(GameManager.Instance.GetPlayerData().role == "Murid")
        {
            label_nim.text = $"NIM  : <color=blue>{data.nim}</color>";
        }
        else
        {
            label_nim.text = $"NIDN/NIDK  : <color=blue>{data.nim}</color>";
        }
        label_prodi.text = $"Prodi/Jurusan  : <color=blue>{data.prodi}</color>";
        label_fakultas.text = $"Universitas/Sekolah    : <color=blue>{data.kampus}</color>";
        label_role.text = $"Saya mendaftar sebagai {data.role}";
    }
}
