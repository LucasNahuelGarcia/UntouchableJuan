using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject CanvasMenu;
    public GameObject CanvasLoading;
    public TMP_Text Titulo;
    public TMP_Text SubTitulo;
    public CanvasGroup Botones;
    [Space]
    [SerializeField] private AudioSource AudioSource;
    [SerializeField] private AudioClip SoundEffectIntro;
    [SerializeField] private AudioClip SoundEffectPlay;
    [SerializeField] private AudioClip SoundEffectExit;
    [Space]
    [SerializeField] private AnimationCurve AnimacionTitulo;
    [SerializeField] private AnimationCurve AnimacionBotones;
    void Start()
    {
        StartCoroutine(AnimarMenu());
    }

    private IEnumerator AnimarMenu()
    {
        Botones.alpha = 0f;
        Titulo.alpha = 0f;
        SubTitulo.alpha = 0f;
        PlayClip(SoundEffectIntro);
        float startTime = Time.time;
        for (int i = 0; Titulo.alpha < 1; i++)
        {
            yield return new WaitForFixedUpdate();
            float animationTime = Time.time - startTime;
            Titulo.alpha = Mathf.Clamp(AnimacionTitulo.Evaluate(animationTime), 0, 1);
        }
        startTime = Time.time;
        for (int i = 0; SubTitulo.alpha < 1; i++)
        {
            yield return new WaitForFixedUpdate();
            float animationTime = Time.time - startTime;
            SubTitulo.alpha = Mathf.Clamp(AnimacionTitulo.Evaluate(animationTime), 0, 1);
        }


        startTime = Time.time;
        for (int i = 0; Botones.alpha < 1; i++)
        {
            yield return new WaitForFixedUpdate();
            float animationTime = Time.time - startTime;
            Botones.alpha = Mathf.Clamp(AnimacionBotones.Evaluate(animationTime), 0, 1);
        }
    }

    public void StartGame()
    {
        CanvasMenu.SetActive(false);
        CanvasLoading.SetActive(true);
        PlayClip(SoundEffectPlay);
        StartCoroutine(LoadRing());
    }

    IEnumerator LoadRing()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("Ring");
    }

    public void ExitGame()
    {
        PlayClip(SoundEffectExit);
        CanvasMenu.SetActive(false);
        StartCoroutine(WaitAndExitGame());
    }

    IEnumerator WaitAndExitGame()
    {
        yield return new WaitForSeconds(2f);
        Application.Quit();
    }

    private void PlayClip(AudioClip clip)
    {
        AudioSource.Stop();
        AudioSource.clip = clip;
        AudioSource.Play();
    }

}
