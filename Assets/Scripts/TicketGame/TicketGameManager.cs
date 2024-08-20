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
    public class TicketGameManager : MonoBehaviour
    {
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
        // 싱글톤 패턴을 사용하기 위한 인스턴스 변수
        private static TicketGameManager _instance;
        // 인스턴스에 접근하기 위한 프로퍼티
        public static TicketGameManager Instance
        {
            get
            {
                // 인스턴스가 없는 경우에 접근하려 하면 인스턴스를 할당해준다.
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(TicketGameManager)) as TicketGameManager;

                    if (_instance == null)
                        Debug.Log("no Singleton obj");
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            // 인스턴스가 존재하는 경우 새로생기는 인스턴스를 삭제한다.
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
            // 아래의 함수를 사용하여 씬이 전환되더라도 선언되었던 인스턴스가 파괴되지 않는다.
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            timer.StartTimer(TimerSec);
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
            if(CheckDecision(true))
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
            if(CheckDecision(false))
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

                if(correctCount == 0)
                result = true;
            }
            else
            {
                for(int i = 0; i < ticketScreenInfo.screenInfo.Length; i++)
                {
                    if(ticketScreenInfo.screenInfo[i] == ticket.currInfoArray[i])
                    continue;
                    if(!ticket.selectedInfoArray[i])
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
    }
}