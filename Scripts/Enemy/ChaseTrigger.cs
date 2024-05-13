using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseTrigger : MonoBehaviour
{
    [SerializeField] private BatChase[] _enemyArray;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            foreach (BatChase enemy in _enemyArray)
            {
                enemy._chase = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            foreach (BatChase enemy in _enemyArray)
            {
                enemy._chase = false;
            }
        }
    }
}
