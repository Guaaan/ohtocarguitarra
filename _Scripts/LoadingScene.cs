using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image LoadingBarFill;


    public void LoadScene(int SceneId)
    {
        StartCoroutine(LoadSceneAsync(SceneId));
    }
    IEnumerator LoadSceneAsync(int SceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneId);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progressValue;

            yield return null;


        }
    }
}
