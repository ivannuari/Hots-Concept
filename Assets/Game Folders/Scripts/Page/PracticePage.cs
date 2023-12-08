using UnityEngine;
using UnityEngine.UI;

public class PracticePage : Page
{
    [SerializeField] private Button b_back;

    private void Start()
    {
        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Understand));
    }
}
