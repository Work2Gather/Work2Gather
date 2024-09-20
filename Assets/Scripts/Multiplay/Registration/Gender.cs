using TMPro;
using UnityEngine;

public class Gender : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    public string CurrentGender;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentGender = "";
    }

    public void OnSelectDropdown()
    {
        switch (dropdown.value)
        {
            case 1:
                CurrentGender = "male";
                break;
            case 2:
                CurrentGender = "female";
                break;
            case 3:
                CurrentGender = "hide";
                break;
            default:
                CurrentGender = "";
                break;
        }
    }

    public bool IsValidGender()
    {
        if (CurrentGender == "")
            return false;
        else
            return true;
    }

}
