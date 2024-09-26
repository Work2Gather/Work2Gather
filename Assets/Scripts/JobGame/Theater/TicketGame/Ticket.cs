using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicketGame
{
    public class Ticket : MonoBehaviour
    {
        #region Serialized Member
        [SerializeField] private GameObject[] ticketInfoObject;
        [SerializeField] private TextMeshProUGUI SeatInfoText;

        #endregion

        private TicketGameManager ticketGameManager;
        private TextMeshProUGUI[] ticketInfoTextArray;
        private Button[] ticketInfoButtonArray;
        private GameObject[] ticketInfoUnderlineArray;

        public string[,] VariationArray = {
            {"기가박스(Gigavox) K-Hackerton 점", "지지브이(CGV) K-Hackerton 점", "", ""},
            {"1관(Laser)","2관(Laser)","3관(Laser)","4관(Laser)"},
            {"아웃사이드인", "슈퍼 베프4", "", ""},
            {"17:35 ~ 20:05", "17:30 ~ 20:00", "", ""}
        };
        public int[] VariationCount;
        [HideInInspector] public string[] currInfoArray;
        private bool[] wrongInfoArray;
        [HideInInspector] public bool[] selectedInfoArray;
        private int selectedCount = 0;

        void Awake()
        {
            ticketInfoTextArray = new TextMeshProUGUI[ticketInfoObject.Length];
            ticketInfoButtonArray = new Button[ticketInfoObject.Length];
            ticketInfoUnderlineArray = new GameObject[ticketInfoObject.Length];
            currInfoArray = new string[ticketInfoObject.Length];
            wrongInfoArray = new bool[ticketInfoObject.Length];
            selectedInfoArray = new bool[ticketInfoObject.Length];

            for (int i = 0; i < ticketInfoObject.Length; i++)
            {
                ticketInfoTextArray[i] = ticketInfoObject[i].GetComponent<TextMeshProUGUI>();
                ticketInfoButtonArray[i] = ticketInfoObject[i].transform.GetChild(0).GetComponent<Button>();
                ticketInfoUnderlineArray[i] = ticketInfoObject[i].transform.GetChild(1).gameObject;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            ticketGameManager = TheaterManager.Instance.TicketGameManager;
        }

        // Update is called once per frame
        void Update()
        {
            if (selectedCount > 0)
            {
                ticketGameManager.EnableRejectButton();
            }
            else
            {
                ticketGameManager.DisableRejectButton();
            }
        }

        public void OnClickSelectButton(int index)
        {
            if (selectedInfoArray[index])
            {
                selectedInfoArray[index] = false;
                ticketInfoUnderlineArray[index].SetActive(false);
                selectedCount--;
            }
            else
            {
                selectedInfoArray[index] = true;
                ticketInfoUnderlineArray[index].SetActive(true);
                selectedCount++;
            }
        }

        void SetButtonWidth()
        {

        }

        void SetUnderlineWidth()
        {

        }

        public void SetTicketByWrongCount(int wrongCount)
        {
            System.Random random = new System.Random();

            for (int i = 0; i < wrongInfoArray.Length; i++)
            {
                wrongInfoArray[i] = false;
            }

            //하드 코딩 > 추후 수정할 것
            switch (wrongCount)
            {
                case 0:
                    break;
                case 1:
                    switch (random.Next(0, 4))
                    {
                        case 0:
                            wrongInfoArray[(int)E_SCREENINFO.TheaterName] = true;
                            break;
                        case 1:
                            wrongInfoArray[(int)E_SCREENINFO.TheaterNum] = true;
                            break;
                        case 2:
                            wrongInfoArray[(int)E_SCREENINFO.MovieName] = true;
                            break;
                        case 3:
                            wrongInfoArray[(int)E_SCREENINFO.ScreenTime] = true;
                            break;
                    }
                    break;
                case 2:
                    switch (random.Next(0, 6))
                    {
                        case 0:
                            wrongInfoArray[(int)E_SCREENINFO.TheaterName] = true;
                            wrongInfoArray[(int)E_SCREENINFO.TheaterNum] = true;
                            break;
                        case 1:
                            wrongInfoArray[(int)E_SCREENINFO.TheaterName] = true;
                            wrongInfoArray[(int)E_SCREENINFO.MovieName] = true;
                            break;
                        case 2:
                            wrongInfoArray[(int)E_SCREENINFO.TheaterName] = true;
                            wrongInfoArray[(int)E_SCREENINFO.ScreenTime] = true;
                            break;
                        case 3:
                            wrongInfoArray[(int)E_SCREENINFO.TheaterNum] = true;
                            wrongInfoArray[(int)E_SCREENINFO.MovieName] = true;
                            break;
                        case 4:
                            wrongInfoArray[(int)E_SCREENINFO.TheaterNum] = true;
                            wrongInfoArray[(int)E_SCREENINFO.ScreenTime] = true;
                            break;
                        case 5:
                            wrongInfoArray[(int)E_SCREENINFO.MovieName] = true;
                            wrongInfoArray[(int)E_SCREENINFO.ScreenTime] = true;
                            break;
                    }
                    break;
                case 3:
                    for (int i = 0; i < wrongInfoArray.Length; i++)
                    {
                        wrongInfoArray[i] = true;
                    }
                    switch (random.Next(0, 4))
                    {
                        case 0:
                            wrongInfoArray[(int)E_SCREENINFO.TheaterName] = false;
                            break;
                        case 1:
                            wrongInfoArray[(int)E_SCREENINFO.TheaterNum] = false;
                            break;
                        case 2:
                            wrongInfoArray[(int)E_SCREENINFO.MovieName] = false;
                            break;
                        case 3:
                            wrongInfoArray[(int)E_SCREENINFO.ScreenTime] = false;
                            break;
                    }
                    break;
                case 4:
                    for (int i = 0; i < wrongInfoArray.Length; i++)
                    {
                        wrongInfoArray[i] = true;
                    }
                    break;
                default:
                    break;
            }

            for (int i = 0; i < wrongInfoArray.Length; i++)
            {
                if (wrongInfoArray[i])
                {
                    currInfoArray[i] = VariationArray[i, random.Next(0, VariationCount[i]) + 1];
                    ticketInfoTextArray[i].text = currInfoArray[i];
                }
                else
                {
                    currInfoArray[i] = VariationArray[i, 0];
                    ticketInfoTextArray[i].text = currInfoArray[i];
                }
            }

            //Randomize Seat Number
            double tempRandom = random.Next(65, 81);
            int asciiNum = (int)Math.Floor(tempRandom);
            char seatCol = (char)asciiNum;
            tempRandom = random.Next(1, 28);
            string seatRow = PadZeroNum((int)tempRandom);
            SeatInfoText.text = $@"{seatCol}열 {seatRow}번";

            SetUnderlineWidth();

            for (int i = 0; i < ticketInfoUnderlineArray.Length; i++)
            {
                ticketInfoUnderlineArray[i].SetActive(false);
                selectedInfoArray[i] = false;
            }
            selectedCount = 0;
        }

        private string PadZeroNum(int num)
        {
            return num.ToString().PadLeft(3, '0');
        }
    }
}
