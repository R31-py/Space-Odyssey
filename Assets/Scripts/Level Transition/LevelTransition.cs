using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    [Tooltip("Scene Name to load when transitioning.")]
    public string sceneToLoad;

    [Tooltip("Check this if the transition requires a key press (e.g., door or spaceship).")]
    public bool requireKeyPress = false;

    [Tooltip("Assign a loading screen prefab here.")]
    public GameObject loadingScreen;

    [Tooltip("Optional: Link a progress bar here.")]
    public Slider progressBar;

    private bool playerInRange = false;
    private bool isLoading = false;

    void Update()
    {
        if (requireKeyPress && playerInRange && !isLoading)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                StartCoroutine(TransitionToNextLevel());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isLoading)
        {
            playerInRange = true;

            if (!requireKeyPress)
            {
                StartCoroutine(TransitionToNextLevel());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private IEnumerator TransitionToNextLevel()
    {
        isLoading = true;
        PlayerSaveManager.SaveLoadState.loadingFromSave = false;

        if (loadingScreen != null) loadingScreen.SetActive(true);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoad);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            if (progressBar != null)
                progressBar.value = progress;

            if (asyncLoad.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0.5f);
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
