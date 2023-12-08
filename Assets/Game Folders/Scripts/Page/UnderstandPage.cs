using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnderstandPage : Page
{
    [SerializeField] private Button b_back;

    [SerializeField] private Button b_plan;
    [SerializeField] private Button b_prepare;
    [SerializeField] private Button b_practice;
    [SerializeField] private Button b_commonEnglish;

    private void Start()
    {
        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Menu));

        b_plan.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Plan));
        b_prepare.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Prepare));
        b_practice.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Practice));
        b_commonEnglish.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.CommonEnglish));
    }
}
