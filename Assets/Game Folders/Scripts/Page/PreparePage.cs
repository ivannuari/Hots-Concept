using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreparePage : Page
{
    [SerializeField] private Button b_back;

    private void Start()
    {
        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Understand));
    }
}
