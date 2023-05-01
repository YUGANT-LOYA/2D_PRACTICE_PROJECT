using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yugant_Library.Controller2D
{
    public class ShopPanelScript : BasePopUpHandler
    {
        public static ShopPanelScript instance;

        public static void SetUp()
        {
            ShopPanelScript obj = Instantiate(Resources.Load<ShopPanelScript>(DataHelper.SHOP_PANEL), UI_HandlerScript.instance.popUpContainer);

            if (instance == null)
            {
                instance = obj;
            }
            else if (instance != obj)
            {
                Destroy(obj);
            }

            Debug.Log("Shop Instance Created.");
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
