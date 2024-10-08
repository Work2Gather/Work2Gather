using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using System;

public class RegistrationCard : MonoBehaviour
{
    [SerializeField] Image Character;
    [SerializeField] TextMeshProUGUI Nickname;
    [SerializeField] TextMeshProUGUI Age;
    [SerializeField] TextMeshProUGUI Gender;
    [SerializeField] TextMeshProUGUI Job;
    [SerializeField] Button ConnectButton;

    public Sprite[] CharacterImageArray;
    private UserClass userClass;

    // 유저 create 후 get 하는부분
    private async void OnEnable()
    {
        userClass = await TitleManager.Instance.DatabaseManager.collectionManager.userCollectionManager.GetUserById(TitleManager.Instance.ClientRegistration.userClassId.ToString());
        InitRegistrationCard();
        ConnectButton.gameObject.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void InitRegistrationCard()
    {
        Character.sprite = CharacterImageArray[userClass.user_character_id - 1];
        Nickname.text = userClass.user_name;
        Age.text = ConvertAgeToText(userClass.user_age);
        Gender.text = ConvertGenderToText(userClass.user_gender);

        StringBuilder sb = new StringBuilder();
        sb.Append("");

        if (userClass.user_jobs.Count >= 1)
            sb.Append(userClass.user_jobs[0]);
        if (userClass.user_jobs.Count >= 2)
        {
            sb.Append(", ").Append(userClass.user_jobs[1]);
            if (userClass.user_jobs.Count > 2)
                sb.Append(", ").Append("...");
        }

        Job.text = sb.ToString();
    }

    string ConvertAgeToText(int age)
    {
        string ageText = "";

        if (age > 0 && age < 20)
            ageText = "10대";
        else if (age >= 20 && age < 30)
            ageText = "20대";
        else if (age >= 30 && age < 40)
            ageText = "30대";
        else if (age >= 40)
            ageText = "40대 이상";

        return ageText;
    }

    string ConvertGenderToText(string gender)
    {
        string genderText = "밝히지 않음";

        switch (gender)
        {
            case "male":
                genderText = "남자";
                break;
            case "female":
                genderText = "여자";
                break;
        }

        return genderText;
    }
}
