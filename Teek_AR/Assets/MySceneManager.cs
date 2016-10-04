using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

namespace Assets
{
    public static class MySceneManager
    {
        private static string lastScene;

        public static void setLastScene(string sceneName)
        {
            lastScene = sceneName;
        }

        public static string getLastScene()
        {
            return lastScene;
        }

        public static void loadPreviousScene()
        {
            if(lastScene != null && lastScene.Length > 0)
                SceneManager.LoadSceneAsync(lastScene);
        }
    }
}
