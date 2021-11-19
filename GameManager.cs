using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public Image fill = null;
    public Text youDied = null;

    private bool once = true;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadStartScene()
    {
        if(once)
            StartCoroutine(LoadScene("Start Scene",0.3f));
    }

    public void LoadGameScene()
    {
        if (once)
            StartCoroutine(LoadScene("Game Scene 1", 0.3f));
    }

    public void Quit()
    {
        if (once)
            Application.Quit();
    }

    public void LoadEndScene()
    {
        if (once)
            StartCoroutine(LoadScene("End Scene",1f));
    }

    private IEnumerator LoadScene(string scene,float time)
    {
        once = false;

        Color color = fill.color;

        while(fill.color.a < 1)
        {
            color.a += Time.deltaTime;
            fill.color = color;
            yield return null;
        }

        if(youDied != null)
        {
            color = youDied.color;

            while (youDied.color.a < 1)
            {
                color.a += 0.8f * Time.deltaTime;
                youDied.color = color;
                yield return null;
            }
        }
        
        yield return new WaitForSecondsRealtime(time);

       SceneManager.LoadScene(scene);
    }
}
