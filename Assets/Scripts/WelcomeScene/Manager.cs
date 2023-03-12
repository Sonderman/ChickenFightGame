using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WelcomeScene
{
    public class Manager : MonoBehaviour
    {
        private AudioSource _audioSource;
        public AudioClip startClip;

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void NewGame()
        {
            _audioSource.PlayOneShot(startClip);
            StartCoroutine(WaitForSeconds(()=>SceneManager.LoadScene(1),1f));
        }

        IEnumerator WaitForSeconds(Action action, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            action();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}