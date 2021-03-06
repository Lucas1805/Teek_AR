﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.SceneManagement;

namespace Assets
{
    public static class MySceneManager
    {
        private static Stack<string> LastSceneStack = new Stack<string>();

        public static void setLastScene(string sceneName)
        {
            if(LastSceneStack.Count > 0)
            {
                if (LastSceneStack.Peek() != sceneName)
                {
                    LastSceneStack.Push(sceneName);
                    return;
                }
                else
                {
                    return;
                }
            }
            LastSceneStack.Push(sceneName);

        }

        public static string getLastScene()
        {
            return LastSceneStack.Pop();
        }

        public static void loadPreviousScene()
        {
            if(LastSceneStack.Peek() != null && LastSceneStack.Peek().Length > 0)
                SceneManager.LoadSceneAsync(LastSceneStack.Pop());
        }
    }
}
