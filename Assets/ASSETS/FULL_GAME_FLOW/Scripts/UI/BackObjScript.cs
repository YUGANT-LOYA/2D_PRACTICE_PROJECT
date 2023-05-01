using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yugant_Library.Controller2D
{
    public class BackObjScript : MonoBehaviour
    {
        public bool isPopUp, isScreen;
        public Button closeButton;

        private void Awake()
        {
            if (isPopUp)
            {
                closeButton.onClick.AddListener(() =>
                {
                    GameControllerScript.instance.RemoveEventFromGameStack();
                    Destroy(this.gameObject);
                });
            }
            else if (isScreen)
            {
                closeButton.onClick.AddListener(() =>
                {
                    GameControllerScript.instance.RemoveEventFromGameStack();
                    DoOff();
                });
            }
        }


        public void DoOff()
        {
            this.gameObject.SetActive(false);
        }

    }
}
