using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Yugant_Library.Controller2D
{
    public class BasePopUpHandler : MonoBehaviour
    {
        public EventID screenID;
        public GameObject mainContentPanel;
        public Ease ease = Ease.OutQuart;
        public float timeToShowPopUp = 0.4f;

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        protected void ShowPopUp(GameObject gm, EventID id)
        {
            gm.transform.localScale = Vector3.zero;
            gm.transform.DOScale(1, timeToShowPopUp).SetEase(ease);

            GameControllerScript.instance.SetCurrentEventID(id);
            GameControllerScript.instance.AddEventToGameStack(id);
        }
    }
}
