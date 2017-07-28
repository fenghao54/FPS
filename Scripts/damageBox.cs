using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageBox : MonoBehaviour
{
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = FindObjectOfType<PlayerState>();
            var curHealth = player.health.Get();
            var damage = 20;
            player.health.Set(curHealth - damage);
        }
    }



    void Update () {
		
	}
}
