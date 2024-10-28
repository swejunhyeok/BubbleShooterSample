using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JH
{
    namespace BBS
    {
        public class ScreenLogo : MonoBehaviour
        {
            // Start is called before the first frame update
            void Start()
            {
                StartCoroutine(SceneChange());
            }

            IEnumerator SceneChange()
            {
                yield return new WaitForSeconds(1);
                SceneManager.LoadScene(ConstantData.TITLE_SCENE);
            }
        }
    }
}
