using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;
    public Image fadeImage;
    public float fadeSpeed = 1.5f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        // Fade Out
        float a = 0;
        while (a < 1)
        {
            a += Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone)
            yield return null;

        // Fade In
        while (a > 0)
        {
            a -= Time.deltaTime * fadeSpeed;
            fadeImage.color = new Color(0, 0, 0, a);
            yield return null;
        }
    }
}
