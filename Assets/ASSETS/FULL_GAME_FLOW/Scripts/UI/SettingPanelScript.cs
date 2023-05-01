using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yugant_Library.Controller2D
{
    public class SettingPanelScript : BasePopUpHandler
    {
        public static SettingPanelScript instance;

        public static void SetUp()
        {
            SettingPanelScript obj = Instantiate(Resources.Load<SettingPanelScript>(DataHelper.SETTING_PANEL), UI_HandlerScript.instance.popUpContainer);

            if (instance == null)
            {
                instance = obj;
            }
            else if (instance != obj)
            {
                Destroy(obj);
            }

            Debug.Log("Setting Instance Created.");
            instance.ShowPopUp(instance.mainContentPanel, instance.screenID);
            Debug.Log("Pop Up Showed.");
        }

        protected override void Start()
        {

            base.Start();
        }


        protected override void Update()
        {
            base.Update();
        }
    }
}
