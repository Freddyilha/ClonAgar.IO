using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelHelper : MonoBehaviour
{
    public Button loginButton;
    public Button guestButton;
    public InputField usernameInput;
    public InputField passwordInput;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onGuestButtonPressed()
    {
        GameManagerBehaviour.instance.nameChosen = true;
        PlayerBehaviour.nickName = "Guest";
        StartCoroutine(GameManagerBehaviour.instance.waitForLogin());

    }

    public void onLoginButtonPressed()
    {
        GameManagerBehaviour.instance.nameChosen = true;
        PlayerBehaviour.nickName = usernameInput.text;
        StartCoroutine(GameManagerBehaviour.instance.waitForLogin());
    }

}
