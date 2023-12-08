using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoginPage : Page
{
    [SerializeField] private TMP_InputField input_email;
    [SerializeField] private TMP_InputField input_pass;

    [SerializeField] private Button b_login, b_register;

    private void Start()
    {
        b_login.onClick.AddListener(Login);
        b_register.onClick.AddListener(() => GameManager.Instance.ChangeState(GameState.Register));
    }

    private void Login()
    {
        if(string.IsNullOrEmpty(input_email.text) || string.IsNullOrEmpty(input_pass.text))
        {
            GameManager.Instance.CreateNotification("isi bagian kosong!");
            return;
        }

        //login with firebase
        FirebaseManager.Instance.SignIn(input_email.text, input_pass.text , OnSuccess);
    }

    private void OnSuccess(bool isLogin)
    {
        if(isLogin)
        {
            //GameManager.Instance.ChangeState(GameState.Menu);
        }
    }
}