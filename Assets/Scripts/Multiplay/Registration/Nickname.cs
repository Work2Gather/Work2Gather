using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class Nickname : MonoBehaviour
{
    [SerializeField] TMP_InputField inputField;
    [SerializeField] GameObject ErrorHandler;   //0: Already Exist, 1: Invalid, 2: Available
    public string CurrentNickname = null;
    public bool IsValidNickname = false;

    public Task<bool> CheckNicknameDuplication()
    {
        return TitleManager.Instance.DatabaseManager.collectionManager.userCollectionManager.CheckUserNameExists(inputField.text);
    }

    public async void OnClickDuplicateCheckButton()
    {
        for(int i = 0; i < ErrorHandler.transform.childCount; i++)
        {
            ErrorHandler.transform.GetChild(i).gameObject.SetActive(false);
        }

        if (CheckValidNickname(inputField.text))
        {
            ErrorHandler.transform.GetChild(1).gameObject.SetActive(true);
            CurrentNickname = null;
            IsValidNickname = false;
            return;
        }

        if (await CheckNicknameDuplication())
        {
            ErrorHandler.transform.GetChild(0).gameObject.SetActive(true);
            CurrentNickname = null;
            IsValidNickname = false;
            return;
        }

        ErrorHandler.transform.GetChild(2).gameObject.SetActive(true);
        CurrentNickname = inputField.text;
        IsValidNickname = true;
    }

    bool CheckValidNickname(string nickname)
    {
        Regex regex = new Regex(@"^(\w|\-){1,10}$");
        //Regex korRegex = new Regex("[\u3131-\uD79D]");

        if (regex.IsMatch(nickname))
        {
            return false;
            // if (korRegex.IsMatch(nickname))
            //     return false;
            // else
            //     return true;
        }
        else
            return true;
    }
}
