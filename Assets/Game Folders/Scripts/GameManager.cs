using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameState currentState = GameState.Login;

    [SerializeField] private PlayerData data;

    [Header("TUGAS")]
    [SerializeField] private TugasRemember tugasRemember;
    [SerializeField] private TugasCreate tugasCreate;
    [SerializeField] private TugasAnalyze tugasAnalyze;
    [SerializeField] private TugasEvaluate tugasEvaluate;

    public delegate void ChangeStateDelegate(GameState newState);
    public event ChangeStateDelegate OnStateChanged;

    public delegate void CreateNotificationDelegate(string info);
    public event CreateNotificationDelegate OnNotificationUpdate;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 45;
    }

    public void ChangeState(GameState newState)
    {
        currentState = newState;

        OnStateChanged?.Invoke(currentState);
    }

    public void CreateNotification(string info)
    {
        OnNotificationUpdate?.Invoke(info);
    }

    public PlayerData GetPlayerData()
    {
        return data;
    }

    public TugasRemember GetTugasRemember()
    {
        if(tugasRemember == null)
        {
            tugasRemember = new TugasRemember(null, null, null, null, 0, null);
        }
        return tugasRemember;
    }

    public TugasCreate GetTugasCreate()
    {
        if(tugasCreate == null)
        {
            tugasCreate = new TugasCreate(null, null, null, null, 0, null,null , null);
        }
        return tugasCreate;
    }

    public TugasAnalyze GetTugasAnalyze()
    {
        if(tugasAnalyze == null)
        {
            tugasAnalyze = new TugasAnalyze(null, null, null, null, 0, null, null);
        }
        return tugasAnalyze;
    }

    public TugasEvaluate GetTugasEvaluate()
    {
        if(tugasEvaluate == null)
        {
            tugasEvaluate = new TugasEvaluate(null, null, null, null, 0, null, null);
        }
        return tugasEvaluate;
    }

    public void SetupPlayerData(string nama, string newRole, string id, string newEmail, string newNim, string newProdi, string newKampus , string newPembimbing)
    {
        data.username = nama;
        data.role = newRole;
        data.userId = id;
        data.email = newEmail;
        data.nim = newNim;
        data.prodi = newProdi;
        data.kampus = newKampus;
        data.pembimbing = newPembimbing;
    }

    public void SetupPlayerData(PlayerData newData)
    {
        data = newData;
    }

    public void SetupTugasRemember(TugasRemember newTugas)
    {
        tugasRemember = newTugas;
    }
    
    public void SetupTugasCreate(TugasCreate newTugas)
    {
        tugasCreate = newTugas;
    }
    
    public void SetupTugasAnalyze(TugasAnalyze newTugas)
    {
        tugasAnalyze = newTugas;
    }

    public void SetupTugasEvaluate(TugasEvaluate newTugas)
    {
        tugasEvaluate = newTugas;
    }
}


[System.Serializable]
public class PlayerData
{
    public string username;
    public string role;
    public string userId;
    public string email;
    public string nim;
    public string prodi;
    public string kampus;
    public string pembimbing;

    public PlayerData(string nama , string newRole , string id , string newEmail , string newNim , string newProdi , string newKampus , string pembimbing)
    {
        username = nama;
        role = newRole;
        userId = id;
        email = newEmail;
        nim = newNim;
        prodi = newProdi;
        kampus = newKampus;
        this.pembimbing = pembimbing;
    }
}

[System.Serializable]
public class TugasRemember
{
    public string userId;
    public string nama;
    public string nim;
    public string waktuPengerjaan;
    public LembarJawaban[] lembarJawaban;
    public int nilai;

    public TugasRemember(string userId, string nama , string nim , string waktuPengerjaan , int nilai , LembarJawaban[] lembarJawab)
    {
        this.userId = userId;
        this.nama = nama;
        this.nim = nim;
        this.waktuPengerjaan = waktuPengerjaan;
        this.nilai = nilai;
        lembarJawaban = lembarJawab;
    }
}

[System.Serializable]
public class TugasCreate
{
    public string userId;
    public string nama;
    public string nim;
    public string waktuPengerjaan;
    public string linkTugas;
    public string textTugas;
    public int nilai;
    public string feedback;

    public TugasCreate(string userId , string nama, string nim, string waktuPengerjaan, int nilai, string uri, string textTugas , string feedback)
    {
        this.userId = userId;
        this.nama = nama;
        this.nim = nim;
        this.waktuPengerjaan = waktuPengerjaan;
        this.nilai = nilai;
        linkTugas = uri;
        this.textTugas = textTugas;
        this.feedback = feedback;
    }
}

[System.Serializable]
public class TugasAnalyze
{
    public string userId;
    public string nama;
    public string nim;
    public string waktuPengerjaan;
    public int nilai;
    public string feedback;
    public LembarJawabAnalyze[] lembarJawaban;

    public TugasAnalyze(string userId, string nama, string nim, string waktuPengerjaan, int nilai, LembarJawabAnalyze[] lembarJawab, string feedback)
    {
        this.userId = userId;
        this.nama = nama;
        this.nim = nim;
        this.waktuPengerjaan = waktuPengerjaan;
        this.nilai = nilai;
        lembarJawaban = lembarJawab;
        this.feedback = feedback;
    }
}

[System.Serializable]
public class TugasEvaluate
{
    public string userId;
    public string nama;
    public string nim;
    public string waktuPengerjaan;
    public int nilai;
    public string feedback;

    public LembarJawabEvaluate[] tugasVideos;

    public TugasEvaluate(string userId, string nama, string nim, string waktuPengerjaan, int nilai, LembarJawabEvaluate[] lembarJawab, string feedback)
    {
        this.userId = userId;
        this.nama = nama;
        this.nim = nim;
        this.waktuPengerjaan = waktuPengerjaan;
        this.nilai = nilai;
        tugasVideos = lembarJawab;
        this.feedback = feedback;
    }
}


[System.Serializable]
public class LembarJawaban
{
    public int nomor;
    public string Jawaban;
    public bool benar;
}

[System.Serializable]
public class TugasVideo
{
    public int nomor;
    public int nilai;
    public string komentar;
}

[System.Serializable]
public class LembarJawabEvaluate
{
    public TugasVideo[] allSoals;
    public bool selesai;
}

[System.Serializable]
public class LembarJawabAnalyze
{
    public int nomor;
    public string jawaban;
}


public enum GameState
{
    Login,
    Register,
    Menu,
    Understand,
    Plan,
    Prepare,
    Profile,
    Remember,
    Apply,
    Create,
    Analyse,
    Evaluate,
    Practice,
    Loading,
    RememberResult,
    MoreMenu,
    CommonEnglish,
    References
}
