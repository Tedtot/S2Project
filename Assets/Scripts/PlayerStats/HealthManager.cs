using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    [SerializeField] Vector3 startingPos;
    [SerializeField] GameObject player;

    [SerializeField] Image healthBar;
    [SerializeField] private float health;

    private float healthBarSize;
    private float healthBarScale;

    private float iFrameTimer;
    private float lastTime;
    private bool iFramesActive;
    
    [SerializeField] private MenuUI menu;
    
    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        startingPos = player.transform.position;

        healthBarSize = 150f;
        healthBarScale = 225f;

        iFrameTimer = 1.5f;
        iFramesActive = false;
    }

    private void Update() {
        if (health <= 0) {
            menu.openMenu();
            health = 100;
            player.transform.position = startingPos;
            setSize();
        }

        if (Input.GetKeyDown(KeyCode.Return)) takeDamage(20, true);
        if (Input.GetKeyDown(KeyCode.L)) healPlayer(5);
    }

    public void takeDamage(float damage, bool iFrames) {

        if (iFrames && !iFramesActive) {
            health -= damage;
            StartCoroutine(iFramesAmount(iFrameTimer));
        }

        else if (!iFrames) health -= damage;

        setSize();
    }

    IEnumerator iFramesAmount(float amount) {
        iFramesActive = true;
        healthBar.GetComponent<Image>().color = Color.cyan;
        yield return new WaitForSeconds(amount);
        healthBar.GetComponent<Image>().color = Color.green;
        iFramesActive = false;
    }

    public void healPlayer(float healAmount) {
        health += healAmount;
        health = Mathf.Clamp(health, 0 , 100);
        setSize();
    }

    private void setSize() {
        healthBar.rectTransform.sizeDelta = new Vector2(healthBar.rectTransform.sizeDelta.x, (health / healthBarSize) * healthBarScale);
    }
}
