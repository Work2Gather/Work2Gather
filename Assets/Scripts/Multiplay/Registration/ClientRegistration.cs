using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClientRegistration : MonoBehaviour
{
    [SerializeField] Nickname Nickname;
    [SerializeField] Age Age;
    [SerializeField] Gender Gender;
    [SerializeField] Job Job;
    [SerializeField] Button NextButton;
    [SerializeField] GameObject[] PageObjectArray;

    UserClass userClass = null;

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

        userClass = new UserClass(Nickname.CurrentNickname, 1, Age.CurrentAge, Gender.CurrentGender, Job.CurrentJobList);
    }

    bool CheckNextButtonActivation()
    {
        if (!Nickname.IsValidNickname)
            return false;
        if (!Age.IsValidAge())
            return false;
        if (!Gender.IsValidGender())
            return false;
        if (!Job.IsValidJob())
            return false;
        return true;
    }

    public void OnClickNextButton()
    {
        //TitleManager.Instance.HTTPManager.PostUserClass(userClass);

        PageObjectArray[0].SetActive(false);
        PageObjectArray[1].SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        userClass = new UserClass("", 0, 0, "", null);
    }

    void Update()
    {
        if (CheckNextButtonActivation())
        {
            NextButton.gameObject.SetActive(true);
        }
        else
        {
            NextButton.gameObject.SetActive(false);
        }
    }
}
