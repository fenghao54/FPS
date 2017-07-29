using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddShells : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var player = FindObjectOfType<PlayerState>();
            var curshells = player.shells.Get();
            var shells_add = 10;
            player.shells.Set(curshells + shells_add);

        }
    }
}
