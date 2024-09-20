using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    [SerializeField] Button[] CharacterButton;
    public int CurrentCharacter = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentCharacter = 0;
    }

    public void OnClickCharacterButton(int index)
    {
        foreach (var button in CharacterButton)
        {
            button.transform.GetChild(1).gameObject.SetActive(false);
        }

        if (CurrentCharacter == index)
        {
            CurrentCharacter = 0;
            CharacterButton[index - 1].transform.GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            CurrentCharacter = index;
            CharacterButton[index - 1].transform.GetChild(1).gameObject.SetActive(true);
        }

    }

    public bool IsValidCharacter()
    {
        if (CurrentCharacter == 0)
            return false;
        else
            return true;
    }
}
