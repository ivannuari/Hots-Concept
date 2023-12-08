using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FirebaseManager : MonoBehaviour
{
    public static FirebaseManager Instance;

    [SerializeField] private bool isLogin;

    private FirebaseApp app;

    private FirebaseAuth auth;
    private FirebaseDatabase db;

    private FirebaseUser user;
    private DatabaseReference reference;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        InitializeFirebase();
        //Initialize();
    }

    private async void Initialize()
    {
        await FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;

            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase App Available!");
                app = FirebaseApp.DefaultInstance;
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }
        });
    }

    public void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseDatabase.DefaultInstance;

        reference = db.RootReference;

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    void AuthStateChanged(object sender, EventArgs eventArgs)
    {
        if (auth.CurrentUser != null)
        {
            if (auth.CurrentUser != user)
            {
                isLogin = auth.CurrentUser.IsValid();
                user = auth.CurrentUser;

                if (isLogin)
                {
                    Debug.Log("Signed in " + user.UserId);
                    RetrieveDatabase();
                }
            }
        }
        else
        {
            isLogin = false;
            GameManager.Instance.ChangeState(GameState.Login);
        }
    }

    public string GetUserId()
    {
        return user.UserId;
    }

    void OnDestroy()
    {
        if (auth != null)
        {
            auth.StateChanged -= AuthStateChanged;
            auth = null;
        }
    }

    public async void SignIn(string email , string password , Action<bool> isSuccessSignIn)
    {
        await auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                GameManager.Instance.CreateNotification("Sign In Cancelled. Close app and try again!");
                Debug.Log($"<color=red>SignInWithEmailAndPasswordAsync was canceled.</color>");
                return;
            }
            if (task.IsFaulted)
            {
            GameManager.Instance.CreateNotification("Sign In Failed. Close app and try again!");
            Debug.Log($"<color=red> SignInWithEmailAndPasswordAsync encountered an error: {task.Exception}</color>");
            return;
            }

            AuthResult result = task.Result;
            user = result.User;

            Debug.LogFormat("User signed in successfully: {0} ({1})",result.User.DisplayName, result.User.UserId);
            RetrieveDatabase();
        });

        isSuccessSignIn(isLogin);
    }

    public async void Register(string email , string password , Action<bool> isSuccessRegister)
    {
        await auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if (task.IsCanceled)
            {
                GameManager.Instance.CreateNotification("Register Canceled!");
                return;
            }
            if (task.IsFaulted)
            {
                GameManager.Instance.CreateNotification("Register Faulted!");
                return;
            }

            // Firebase user has been created.
            AuthResult result = task.Result;
            user = result.User;

            PlayerData data = GameManager.Instance.GetPlayerData();
            GameManager.Instance.SetupPlayerData(data.username, data.role, user.UserId, data.email, data.nim, data.prodi, data.kampus, data.pembimbing);
            SavePlayerData(data);
        });
        isSuccessRegister(isLogin);
    }

    public void SignedOut()
    {
        auth.SignOut();
    }

    public async void SavePlayerData(PlayerData data)
    {
        string json = JsonUtility.ToJson(data);
        await reference.Child("USERS").Child(user.UserId).SetRawJsonValueAsync(json);
        reference.Push();

        PendaftaranGuru();
    }

    private async void PendaftaranGuru()
    {
        if(GameManager.Instance.GetPlayerData().role == "Murid")
        {
            return;
        }

        //ambil daftar list guru
        await reference.Child("PENGAJAR").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                DaftarGuru daftar = new DaftarGuru();
                if (json != null)
                {
                    daftar = JsonUtility.FromJson<DaftarGuru>(json);
                }

                if(daftar.namaGuru.Exists(n => n == GameManager.Instance.GetPlayerData().username))
                {
                    return;
                }

                daftar.namaGuru.Add(GameManager.Instance.GetPlayerData().username);

                string jsonTugas = JsonUtility.ToJson(daftar);
                reference.Child("PENGAJAR").SetRawJsonValueAsync(jsonTugas);
            }
        });
    }

    public void GetListPengajar(Action<List<string>> OnGetListPengajar)
    {
        reference.Child("PENGAJAR").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
                return;
            }
            else
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                DaftarGuru temp = JsonUtility.FromJson<DaftarGuru>(json);
                OnGetListPengajar(temp.namaGuru);
            }
        });
    }

    [System.Serializable]
    public class DaftarGuru
    {
        public List<string> namaGuru = new List<string>();
    }

    public async void SaveTugasRemember(TugasRemember newTugas)
    {
        await reference.Child("TUGAS_REMEMBER").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllRememberTugas allTugas = new AllRememberTugas();
                if (json != null)
                {
                    allTugas = JsonUtility.FromJson<AllRememberTugas>(json);
                }

                bool isTugasAda = allTugas.tugas.Exists(t => t.userId == user.UserId);
                if (isTugasAda)
                {
                    int nomorTugas = allTugas.tugas.FindIndex(t => t.userId == user.UserId);
                    allTugas.tugas.RemoveAt(nomorTugas);
                }
                allTugas.tugas.Add(newTugas);
                
                string jsonTugas = JsonUtility.ToJson(allTugas);
                reference.Child("TUGAS_REMEMBER").SetRawJsonValueAsync(jsonTugas);

                reference.Push();
            }
        });
    }

    public async void SaveTugasCreate(TugasCreate newTugas)
    {
        await reference.Child("TUGAS_CREATE").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if(task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }

            if(task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllCreateTugas allTugas = new AllCreateTugas();
                if (json != null)
                {
                    allTugas = JsonUtility.FromJson<AllCreateTugas>(json); 
                }

                TugasCreate findTugas = allTugas.tugas.Find(t => t.userId == user.UserId);
                if (findTugas == null)
                {
                    allTugas.tugas.Add(newTugas);
                }
                else
                {
                    int index = allTugas.tugas.FindIndex(t => t.userId == user.UserId);
                    allTugas.tugas[index] = newTugas;
                }

                string jsonTugas = JsonUtility.ToJson(allTugas);
                reference.Child("TUGAS_CREATE").SetRawJsonValueAsync(jsonTugas);

                reference.Push();
            }
        });
    }

    public async void SaveTugasAnalyze(TugasAnalyze newTugas)
    {
        await reference.Child("TUGAS_ANALYZE").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllAnalyzeTugas allTugas = new AllAnalyzeTugas();
                if (json != null)
                {
                    allTugas = JsonUtility.FromJson<AllAnalyzeTugas>(json);
                }

                TugasAnalyze findTugas = allTugas.tugas.Find(t => t.userId == user.UserId);
                if (findTugas == null)
                {
                    allTugas.tugas.Add(newTugas);
                }
                else
                {
                    int index = allTugas.tugas.FindIndex(t => t.userId == user.UserId);
                    allTugas.tugas[index] = newTugas;
                }

                string jsonTugas = JsonUtility.ToJson(allTugas);
                reference.Child("TUGAS_ANALYZE").SetRawJsonValueAsync(jsonTugas);

                reference.Push();
            }
        });
    }

    public async void SaveTugasEvaluate(TugasEvaluate newTugas)
    {
        await reference.Child("TUGAS_EVALUATE").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }

            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllEvaluateTugas allTugas = new AllEvaluateTugas();
                if (json != null)
                {
                    allTugas = JsonUtility.FromJson<AllEvaluateTugas>(json);
                }

                TugasEvaluate findTugas = allTugas.tugas.Find(t => t.userId == user.UserId);
                if (findTugas == null)
                {
                    allTugas.tugas.Add(newTugas);
                }
                else
                {
                    int index = allTugas.tugas.FindIndex(t => t.userId == user.UserId);
                    allTugas.tugas[index] = newTugas;
                }
                
                string jsonTugas = JsonUtility.ToJson(allTugas);
                reference.Child("TUGAS_EVALUATE").SetRawJsonValueAsync(jsonTugas);

                reference.Push();
            }
        });
    }

    public async void NilaiTugasAnalyze(AllAnalyzeTugas newTugas)
    {
        string jsonTugas = JsonUtility.ToJson(newTugas);
        await reference.Child("TUGAS_ANALYZE").SetRawJsonValueAsync(jsonTugas);

        reference.Push();
    }

    public async void NilaiTugasCreate(AllCreateTugas newTugas)
    {
        string jsonTugas = JsonUtility.ToJson(newTugas);
        await reference.Child("TUGAS_CREATE").SetRawJsonValueAsync(jsonTugas);

        reference.Push();
    }

    public async void NilaiTugasEvaluate(AllEvaluateTugas newTugas)
    {
        string jsonTugas = JsonUtility.ToJson(newTugas);
        await reference.Child("TUGAS_EVALUATE").SetRawJsonValueAsync(jsonTugas);

        reference.Push();
    }

    public async void RetrieveDatabase()
    {
        //user
        await db.GetReference("USERS").Child(user.UserId).GetValueAsync().ContinueWithOnMainThread(task => 
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                PlayerData newData = JsonUtility.FromJson<PlayerData>(json);
                GameManager.Instance.SetupPlayerData(newData);
            }
        });
        //tugas remember
        await db.GetReference("TUGAS_REMEMBER").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllRememberTugas newData = JsonUtility.FromJson<AllRememberTugas>(json);
                if (newData == null)
                {
                    return;
                }

                TugasRemember myTugas = newData.tugas.Find(t => t.userId == user.UserId);
                GameManager.Instance.SetupTugasRemember(myTugas);
            }
        });
        //tugas create
        await db.GetReference("TUGAS_CREATE").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllCreateTugas newData = JsonUtility.FromJson<AllCreateTugas>(json);
                if(newData == null)
                {
                    return;
                }

                TugasCreate myTugas = newData.tugas.Find(t => t.userId == user.UserId);
                GameManager.Instance.SetupTugasCreate(myTugas);
            }
        });
        //tugas analyze
        await db.GetReference("TUGAS_ANALYZE").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllAnalyzeTugas newData = JsonUtility.FromJson<AllAnalyzeTugas>(json);
                if (newData == null)
                {
                    return;
                }

                TugasAnalyze myTugas = newData.tugas.Find(t => t.userId == user.UserId);
                GameManager.Instance.SetupTugasAnalyze(myTugas);
            }
        });
        //tugas evaluate
        await db.GetReference("TUGAS_EVALUATE").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllEvaluateTugas newData = JsonUtility.FromJson<AllEvaluateTugas>(json);
                if (newData == null)
                {
                    return;
                }

                TugasEvaluate myTugas = newData.tugas.Find(t => t.userId == user.UserId);
                GameManager.Instance.SetupTugasEvaluate(myTugas);
            }
        });

        GameManager.Instance.ChangeState(GameState.Menu);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SignedOut();
        }
    }

    public async void GetAllAnalyzeTugas(Action<AllAnalyzeTugas> OnGetTugas)
    {
        await db.GetReference("TUGAS_ANALYZE").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllAnalyzeTugas newData = new AllAnalyzeTugas();
                if (json != null)
                {
                    newData = JsonUtility.FromJson<AllAnalyzeTugas>(json);
                }
                OnGetTugas(newData);
            }
        });
    }

    public async void GetAllCreateTugas(Action<AllCreateTugas> OnGetTugas)
    {
        await db.GetReference("TUGAS_CREATE").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllCreateTugas newData = new AllCreateTugas();
                if (json != null)
                {
                    newData = JsonUtility.FromJson<AllCreateTugas>(json);
                }
                OnGetTugas(newData);
            }
        });
    }
    
    public async void GetAllEvaluateTugas(Action<AllEvaluateTugas> OnGetTugas)
    {
        await db.GetReference("TUGAS_EVALUATE").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllEvaluateTugas newData = new AllEvaluateTugas();
                if (json != null)
                {
                    newData = JsonUtility.FromJson<AllEvaluateTugas>(json);
                }
                OnGetTugas(newData);
            }
        });
    }

    public async void GetAllRememberTugas(Action<AllRememberTugas> OnGetTugas)
    {
        await db.GetReference("TUGAS_REMEMBER").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("Gagal ambil data dari server");
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                string json = snapshot.GetRawJsonValue();

                AllRememberTugas newData = new AllRememberTugas();
                if (json != null)
                {
                    newData = JsonUtility.FromJson<AllRememberTugas>(json);
                }
                OnGetTugas(newData);
            }
        });
    }
}

[System.Serializable]
public class AllCreateTugas
{
    public List<TugasCreate> tugas = new List<TugasCreate>();
}

[System.Serializable]
public class AllAnalyzeTugas
{
    public List<TugasAnalyze> tugas = new List<TugasAnalyze>();
}

[System.Serializable]
public class AllEvaluateTugas
{
    public List<TugasEvaluate> tugas = new List<TugasEvaluate>();
}

[System.Serializable]
public class AllRememberTugas
{
    public List<TugasRemember> tugas = new List<TugasRemember>();
}
