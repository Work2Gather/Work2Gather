using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TicketGame
{
    public class TicketGuest : MonoBehaviour
    {
        [SerializeField] private RawImage currImage;
        public Texture[] imageArray;

        public void SetImageByRandom()
        {
            System.Random random = new System.Random();

            currImage.texture = imageArray[random.Next(0, imageArray.Length)];
        }

        public void SetImageByIndex(int _index)
        {
            currImage.texture = imageArray[_index];
        }

        public void FadeInImage()
        {
            if (currImage.texture != null)
            {
                StartCoroutine(DoFadeInImage());
            }
        }

        private IEnumerator DoFadeInImage()
        {
            Color color = currImage.color;
            color.a = 0;
            currImage.color = color;

            for (float i = 0; i <= 1; i += 0.1f)
            {
                color.a = i;
                currImage.color = color;
                yield return new WaitForSeconds(0.02f);
            }

            color.a = 1;
            currImage.color = color;
        }

        public void FadeOutImage()
        {
            if (currImage.texture != null)
            {
                StartCoroutine(DoFadeOutImage());
            }
        }

        private IEnumerator DoFadeOutImage()
        {
            Color color = currImage.color;
            color.a = 1;
            currImage.color = color;

            for (float i = 1; i >= 0; i -= 0.1f)
            {
                color.a = i;
                currImage.color = color;
                yield return new WaitForSeconds(0.02f);
            }

            color.a = 0;
            currImage.color = color;
        }
    }
}