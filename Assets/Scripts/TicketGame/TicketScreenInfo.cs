using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TicketGame
{
    public class TicketScreenInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI screenInfoText;
        public string[] screenInfo;

        #region Private Member

        private string round;
        private string dimension;
        private string age;
        private string date;

        #endregion

        public void SetScreenInfo(string _theaterName, string _theaterNum, string _movieName, string _screenTime)
        {
            screenInfo[(int)E_SCREENINFO.TheaterName] = _theaterName;
            screenInfo[(int)E_SCREENINFO.TheaterNum] = _theaterNum;
            screenInfo[(int)E_SCREENINFO.MovieName] = _movieName;
            screenInfo[(int)E_SCREENINFO.ScreenTime] = _screenTime;
        }
    }
}