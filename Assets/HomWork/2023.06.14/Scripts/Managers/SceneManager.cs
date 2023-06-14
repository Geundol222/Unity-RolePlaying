using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace HomeWork0614
{
    public class SceneManager : MonoBehaviour
    {
        private LoadingUI loadingUI;

        private BaseScene curScene;
        public BaseScene CurScene
        {
            get
            {
                if (curScene == null)
                    curScene = GameObject.FindObjectOfType<BaseScene>();

                return curScene;
            }
        }

        private void Awake()
        {
            LoadingUI ui = Resources.Load<LoadingUI>("0605/UI/LoadingUI");
            loadingUI = Instantiate(ui);
            loadingUI.transform.SetParent(transform, false);
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadingRoutine(sceneName));
        }

        IEnumerator LoadingRoutine(string sceneName)
        {
            loadingUI.FadeOut();
            yield return new WaitForSeconds(1f);
            Time.timeScale = 0f;

            AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);

            while (!oper.isDone)
            {
                loadingUI.SetProgress(Mathf.Lerp(0f, 0.5f, oper.progress));
                yield return null;
            }

            // 추가적인 씬에서 준비할 로딩을 진행하고 넘어가야함
            CurScene.LoadAsync();
            while (CurScene.progress < 1f)
            {
                loadingUI.SetProgress(Mathf.Lerp(0.5f, 1.0f, CurScene.progress));
                yield return null;
            }

            Time.timeScale = 1f;
            loadingUI.FadeIn();
            yield return new WaitForSeconds(1f);
        }
    }
}

