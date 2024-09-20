using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Age : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdown;
    public int CurrentAge;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentAge = -1;
    }

    public void OnSelectDropdown()
    {
        switch (dropdown.value)
        {
            case 1:
                CurrentAge = 10;
                break;
            case 2:
                CurrentAge = 20;
                break;
            case 3:
                CurrentAge = 30;
                break;
            case 4:
                CurrentAge = 40;
                break;
            default:
                CurrentAge = -1;
                break;
        }

        Debug.Log("selected age dropdown value: " + dropdown.value);
    }

    public bool IsValidAge()
    {
        if (CurrentAge < 0)
            return false;
        else
            return true;
    }
}
