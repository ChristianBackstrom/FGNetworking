using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SavingUserNameUI : MonoBehaviour
{

    [SerializeField] private InputField userNameField;


    void Awake()
    {
        PlayerPrefs.DeleteAll();
        String userName = PlayerPrefs.GetString("UserName");
        if (userName.Trim().Equals(String.Empty)) return;
        GoToScene();
    }

    public void SaveButtonClicked()
    {

        String userName = userNameField.text;
        if (userName.Length >= 3 && userName.Length <= 20)
        {
            PlayerPrefs.SetString("UserName", userName);
            GoToScene();
        }

    }


    private void GoToScene()
    {
        SceneManager.LoadScene("FirstScene");
    }


}
