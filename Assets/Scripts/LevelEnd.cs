using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    [SerializeField] public Collider2D playerCollider;
    [SerializeField] public Animator transition;
    [SerializeField] public float transitionDuration;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider == playerCollider)
        {
            StartCoroutine(LoadNextLevel(SceneManager.GetActiveScene().buildIndex + 1));
        }
    }

    IEnumerator LoadNextLevel(int levelIndex)
    {
        transition.SetTrigger("StartTransition");

        yield return new WaitForSeconds(transitionDuration);

        SceneManager.LoadScene(levelIndex);
    }
}
