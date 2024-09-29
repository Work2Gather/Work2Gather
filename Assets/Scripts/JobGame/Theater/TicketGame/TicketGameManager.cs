using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TicketGame
{
    public enum E_SCREENINFO
    {
        TheaterName,
        TheaterNum,
        MovieName,
        ScreenTime,
    }

    public enum E_TICKET_DIFFICULTY
    {
        Low,
        Mid,
        High,
    }
    public class TicketGameManager : MonoBehaviour
    {
        [SerializeField] public UIManager UIManager;
        #region Serialized Member
        [SerializeField] private Button AcceptButton;
        [SerializeField] private Button RejectButton;
        [SerializeField] private Timer timer;
        [SerializeField] private TicketGuest ticketGuest;
        [SerializeField] private TicketScreenInfo ticketScreenInfo;
        [SerializeField] private Ticket ticket;
        [SerializeField] private TextMeshProUGUI ScoreText;

        #endregion

        #region Public Member

        public int TimerSec;
        public int currScore = 0;

        #endregion

        private void Start()
        {
            Debug.Log("Start Ticket Game");
        }
        #region Ticket Game

        public void StartTicketGame()
        {
            timer.StartTimer(TimerSec);
            timer.OnTimeOut(() => {TimeOut();});
            ticketScreenInfo.SetScreenInfo("기가박스(Gigavox) K-Hackerton 점", "1관(Laser)", "아웃사이드인", "17:35 ~ 20:05");
            SetNewGuest();
        }

        private void SetNewGuest()
        {
            System.Random random = new System.Random();

            ticketGuest.SetImageByRandom();
            ticketGuest.FadeInImage();
            ticket.SetTicketByWrongCount(random.Next(0, 5));
        }

        public void AcceptGuest()
        {
            if (CheckDecision(true))
            {
                ticketGuest.FadeOutImage();
                currScore++;
                ScoreText.text = $@"{currScore}";
                SetNewGuest();
            }
            else
            {
                ticketGuest.FadeOutImage();
                currScore--;
                ScoreText.text = $@"{currScore}";
                SetNewGuest();
            }
        }

        public void RejectGuest()
        {
            if (CheckDecision(false))
            {
                ticketGuest.FadeOutImage();
                currScore++;
                ScoreText.text = $@"{currScore}";
                SetNewGuest();
            }
            else
            {
                ticketGuest.FadeOutImage();
                currScore--;
                ScoreText.text = $@"{currScore}";
                SetNewGuest();
            }
        }

        public void EnableRejectButton()
        {
            RejectButton.gameObject.SetActive(true);
        }

        public void DisableRejectButton()
        {
            RejectButton.gameObject.SetActive(false);
        }

        private bool CheckDecision(bool decision)
        {
            bool result = false;
            int correctCount = ticket.currInfoArray.Length;

            if (decision)
            {
                for (int i = 0; i < ticketScreenInfo.screenInfo.Length; i++)
                {
                    if (ticketScreenInfo.screenInfo[i] != ticket.currInfoArray[i])
                        continue;
                    if (ticket.selectedInfoArray[i])
                        continue;
                    correctCount--;
                }

                if (correctCount == 0)
                    result = true;
            }
            else
            {
                for (int i = 0; i < ticketScreenInfo.screenInfo.Length; i++)
                {
                    if (ticketScreenInfo.screenInfo[i] == ticket.currInfoArray[i])
                        continue;
                    if (!ticket.selectedInfoArray[i])
                        continue;

                    correctCount--;
                }

                if (correctCount != ticket.currInfoArray.Length)
                {
                    currScore += (ticket.currInfoArray.Length - 1 - correctCount);
                    result = true;
                }
            }

            return result;
        }

        public void TimeOut()
        {
            //타임 아웃 시 보여줄 것
            TheaterManager.Instance.ReturnToMainSelect();
        }

        #endregion
    }
}