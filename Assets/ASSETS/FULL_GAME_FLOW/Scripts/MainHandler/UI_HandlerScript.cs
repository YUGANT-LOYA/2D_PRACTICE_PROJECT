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
        public Transform popUpContainer;

        public BaseScreenHandler[] screensArr;


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

      
        public int GetGameObjOfEventID(EventID id)
        {
            for(int i = 0; i < screensArr.Length; i++)
            {
                if(screensArr[i].screenID == id)
                {
                    return i;
                }
            }

            Debug.LogError("Id not Found !");
            return -1;
        } 


        public void PlayButton()
        {
            Debug.Log("Play Button Clicked.");
            GameControllerScript.instance.SwitchScreen(EventID.CATEGORY_SCREEN);

        }

        public void SettingButton()
        {
            Debug.Log("Setting Button Clicked.");
            SettingPanelScript.SetUp();
        }

        public void ShopButton()
        {
            Debug.Log("Shop Button Clicked.");
            ShopPanelScript.SetUp();
        }
    }
}
