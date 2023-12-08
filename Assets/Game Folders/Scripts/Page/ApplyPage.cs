using UnityEngine;
using UnityEngine.UI;

public class ApplyPage : Page
{
    [SerializeField] private Button b_back;
    [SerializeField] private Button b_video;
    [SerializeField] private Button b_youtube;
    [SerializeField, TextArea(2, 2)] private string uri;

    [SerializeField] private GameObject panel_video;
    [SerializeField] private GameObject panel_button;

    private void OnEnable()
    {
        panel_video.SetActive(false);
        panel_button.SetActive(true);
    }

    private void Start()
    {
        b_back.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Menu));
        b_video.onClick.AddListener(() => 
        { 
            panel_video.SetActive(true);
            panel_button.SetActive(false);
        });
        b_youtube.onClick.AddListener(() => Application.OpenURL(uri));
    }
}
