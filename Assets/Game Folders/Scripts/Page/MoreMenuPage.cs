using UnityEngine;
using UnityEngine.UI;

public class MoreMenuPage : Page
{
    [SerializeField] private Button b_back;
    [SerializeField] private Button b_result;
    [SerializeField] private Button b_akun;
    [SerializeField] private Button b_logout;
    [SerializeField] private Button b_references;

    private void Start()
    {
        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Menu));
        b_akun.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Profile));
        b_references.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.References));
        b_logout.onClick.AddListener(() =>
        {
            FirebaseManager.Instance.SignedOut();

            PlayerData newPlayer = new PlayerData(null , null , null , null , null , null , null , null);
            GameManager.Instance.SetupPlayerData(newPlayer);

            TugasAnalyze analyze = new TugasAnalyze(null, null, null, null, 0, null, null);
            GameManager.Instance.SetupTugasAnalyze(analyze);
            
            TugasCreate create = new TugasCreate(null, null, null, null, 0, null, null, null);
            GameManager.Instance.SetupTugasCreate(create);
            
            TugasEvaluate evaluate = new TugasEvaluate(null, null, null, null, 0, null, null);
            GameManager.Instance.SetupTugasEvaluate(evaluate);
            
            TugasRemember remember = new TugasRemember(null, null, null, null, 0, null);
            GameManager.Instance.SetupTugasRemember(remember);
        });
    }
}
