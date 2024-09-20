using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

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

    private void OnEnable()
    {
        userClass = TitleManager.Instance.HTTPManager.CurrentUserClass;
        InitRegistrationCard();
        ConnectButton.gameObject.SetActive(true);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void InitRegistrationCard()
    {
        Character.sprite = CharacterImageArray[userClass.user_character_id - 1];
        Nickname.text = userClass.user_name;
        Age.text = userClass.user_age.ToString();
        Gender.text = userClass.user_gender;

        StringBuilder sb = new StringBuilder();

        sb.Append(userClass.user_job_id[0]);
        if (userClass.user_job_id.Count >= 2)
        {
            sb.Append(", ").Append(userClass.user_job_id[1]);
            if (userClass.user_job_id.Count > 2)
                sb.Append(", ").Append("...");
        }

        Job.text = sb.ToString();
    }
}
