using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yugant_Library.Controller2D
{
    public class UI_HandlerScript : MonoBehaviour
    {
        public static UI_HandlerScript instance;
        public Button playButton;
        public Button settingButton;
        public Button shopButton;

        public BaseScreenScript[] screensArr;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        public void PlayButton()
        {
            Debug.Log("Play Button Clicked.");


        }

        public void SettingButton()
        {
            Debug.Log("Setting Button Clicked.");

        }

        public void ShopButton()
        {
            Debug.Log("Shop Button Clicked.");

        }
    }
}
