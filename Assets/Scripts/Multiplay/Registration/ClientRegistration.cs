using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientRegistration : MonoBehaviour
{
    #region Serialized Members
    [Header("Page 1")]
    [SerializeField] Nickname Nickname;
    [SerializeField] Age Age;
    [SerializeField] Gender Gender;
    [SerializeField] Job Job;

    [Header("Page 2")]
    [SerializeField] Character Character;

    [Header("Common")]
    [SerializeField] Button PreviousButton;
    [SerializeField] Button NextButton;
    [SerializeField] GameObject[] PageObjectArray;
    #endregion

    UserClass userClass = null;
    private int pageIndex = 1;

    void InitUserClass()
    {
        if (!Nickname.IsValidNickname)
        {
            Debug.Log("Invalid Nickname");
            return;
        }

        if (!Age.IsValidAge())
        {
            Debug.Log("Invalid Age");
            return;
        }

        if (!Gender.IsValidGender())
        {
            Debug.Log("Invalid Gender");
            return;
        }

        if (!Job.IsValidJob())
        {
            Debug.Log("Invalid Job");
            return;
        }

        if (!Character.IsValidCharacter())
        {
            Debug.Log("Invalid Character");
            return;
        }

        userClass = new UserClass(Nickname.CurrentNickname, Character.CurrentCharacter, Age.CurrentAge, Gender.CurrentGender, Job.CurrentJobList);
    }

    bool CheckNextButtonActivation(int index)
    {
        switch (index)
        {
            case 1:
                if (!Nickname.IsValidNickname)
                    return false;
                if (!Age.IsValidAge())
                    return false;
                if (!Gender.IsValidGender())
                    return false;
                if (!Job.IsValidJob())
                    return false;
                return true;
            case 2:
                if (!Character.IsValidCharacter())
                    return false;
                return true;
            default:
                return false;
        }
    }

    public void OnClickPreviousButton()
    {
        switch (pageIndex)
        {
            case 2:
                PageObjectArray[0].SetActive(true);
                PageObjectArray[1].SetActive(false);

                pageIndex = 1;
                break;
        }
    }

    public void OnClickNextButton()
    {
        switch (pageIndex)
        {
            case 1:
                PageObjectArray[0].SetActive(false);
                PageObjectArray[1].SetActive(true);

                pageIndex = 2;
                break;
            case 2:
                InitUserClass();
                TitleManager.Instance.HTTPManager.PostUserClass(userClass);
                TitleManager.Instance.UIManager.RegistrationCard.SetActive(true);
                gameObject.SetActive(false);
                break;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TitleManager.Instance.UIManager.WelcomeText.gameObject.SetActive(false);
        userClass = new UserClass("", 0, 0, "", null);

        PageObjectArray[0].SetActive(true);
        PageObjectArray[1].SetActive(false);
    }

    void Update()
    {
        if (CheckNextButtonActivation(pageIndex))
        {
            NextButton.gameObject.SetActive(true);
        }
        else
        {
            NextButton.gameObject.SetActive(false);
        }

        if (pageIndex == 1)
            PreviousButton.gameObject.SetActive(false);
        else
            PreviousButton.gameObject.SetActive(true);
    }
}
