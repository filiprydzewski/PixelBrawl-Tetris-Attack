using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectHealth : MonoBehaviour
{
    public Animator animator;
    public HealthBar healthBar;
    public AudioSource soundEffect;
    public int maxHealth = 100;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        healthBar.SetMaxHealth(maxHealth);
    }

    // change name to stun?
    public void TakeDamage(int damage)
    {
        if (currentHealth > damage) {
            currentHealth -= damage;
        } else {
            currentHealth = 0;
        }
       
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
                    foreach (AudioSource source in audioSources)
                    {
                        soundEffect.Play();
                        source.Stop();
                        

                    }
                    Time.timeScale = 0f;
                    StartCoroutine(WaitAndLoadNextScene(5f));
        }
    }

    void Die()
    {
        Debug.Log("You died");
        
        animator.SetBool("IsDead", true);

        


        // nie działa mi to :(
        this.enabled = false;

    }

    IEnumerator WaitAndLoadNextScene(float waitTime)
{
    yield return new WaitForSecondsRealtime(waitTime);
    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    Time.timeScale = 1f;
}
}
