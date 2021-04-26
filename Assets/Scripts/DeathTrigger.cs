﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Ground")
            StartCoroutine(LevelRestart());
    }
     IEnumerator LevelRestart()
    {
       yield return new WaitForSeconds(2f);
       SceneManager.LoadScene("Game");
    }

    

}
