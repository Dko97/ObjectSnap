using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This script is only there as an extra feature to give the user flexibility to reset the position of the player
 * cube which might be able to aid in good user experience. 
 */

public class GameController : MonoBehaviour
{
    GameObject Player;
    Vector3 initialPosition;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        initialPosition = Player.transform.position;
    }

    public void ResetPlayer()
    {
        Player.transform.position = initialPosition;    
    }
}
