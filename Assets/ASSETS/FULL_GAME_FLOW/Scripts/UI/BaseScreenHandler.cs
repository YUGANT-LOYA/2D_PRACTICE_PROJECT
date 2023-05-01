using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Yugant_Library.Controller2D
{
    public class BaseScreenHandler : MonoBehaviour
    {
        public EventID screenID;
        public Ease ease = Ease.OutQuart;
        public float timeToShowPopUp = 0.2f;

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected void ShowScreen(EventID id)
        {
            int index = UI_HandlerScript.instance.GetGameObjOfEventID(id);
            BaseScreenHandler baseScreen = UI_HandlerScript.instance.screensArr[index];
            baseScreen.GetComponent<Canvas>().enabled = true;
            GameControllerScript.instance.SetCurrentEventID(id);
            GameControllerScript.instance.AddEventToGameStack(id);
        }

        protected void HideScreen(EventID id)
        {
            int index = UI_HandlerScript.instance.GetGameObjOfEventID(id);
            BaseScreenHandler baseScreen = UI_HandlerScript.instance.screensArr[index];
            baseScreen.GetComponent<Canvas>().enabled = false;

            GameControllerScript.instance.RemoveEventFromGameStack();
            GameControllerScript.instance.SetCurrentEventID(id);
        }
    }
}