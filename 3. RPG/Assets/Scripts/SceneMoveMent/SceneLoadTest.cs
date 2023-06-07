using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoadTest : MonoBehaviour
{
    public static SceneLoadTest instance;

    public Image fader;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            fader.rectTransform.sizeDelta = new Vector2(Screen.width + 20 , Screen.height + 20);
            fader.gameObject.SetActive(false);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(int index)
    {
        instance.StartCoroutine(FadeScene(index));
    }

    public IEnumerator FadeScene(int index)
    {
        fader.gameObject.SetActive(true);


        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, i));
            yield return null;
        }

        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(index);

        while(!asyncOperation.isDone)
        {
            yield return null;
        }

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            fader.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, i));
            yield return null;
        }


        fader.gameObject.SetActive(false);
    }
}
