using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JH
{
    namespace BBS
    {
        public class ScreenTitle : MonoBehaviour
        {
            public void SceneChangePlay()
            {
                SceneManager.LoadScene(ConstantData.PLAY_SCENE);
            }
        }
    }
}
