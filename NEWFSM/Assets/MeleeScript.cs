using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeScript : MonoBehaviour
{
    public bool canAttack = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            canAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            canAttack = false;
        }
    }
}
