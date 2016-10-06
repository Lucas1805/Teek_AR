using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public static class LoadingManager
    {
        public static void showLoadingIndicator(GameObject loadingPanel)
        {
            if(loadingPanel != null)
            {
                loadingPanel.SetActive(true);
            }
            else
            {
                Debug.Log("Loading panel is null");
            }
        }

        public static void hideLoadingIndicator(GameObject loadingPanel)
        {
            if (loadingPanel != null)
            {
                loadingPanel.SetActive(false);
            }
            else
            {
                Debug.Log("Loading panel is null");
            }
        }
    }
}
