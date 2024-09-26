using HUDIndicator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("0. Title_Client")]
    public FadeEffect FadePanel;
    public FadeEffect WelcomeText;
    public Button[] TitleButtonArray;
    public GameObject RegistrationCard;

    [Space(10f)]

    [Header("Common")]
    public TextMeshProUGUI CurrentMapText;
    public TextMeshProUGUI JobiText;

    [Space(10f)]

    [Header("1. MainTown")]
    public GameObject FImage;
    public TextMeshProUGUI FImageText;
    public IndicatorRenderer IndicatorRenderer;

    [Space(10f)]

    [Header("2-1. Theater")]
    public GameObject InGameUI;
    public GameObject MainSelect;
    public GameObject TicketGame;

    // [Space(10f)]

    // [Header("2-2. Exhibition")]
    // public GameObject FImage;
    // public TextMeshProUGUI FImageText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
}
