using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Glidrive.UI
{
    public class DiskEnergyView : MonoBehaviour
    {
        private Image view;

        private void Awake()
        {
            view = this.GetComponent<Image>();
        }

        public void UpdateFillAmount(float fillAmount)
        {
            this.view.fillAmount = fillAmount;
        }
    }
}