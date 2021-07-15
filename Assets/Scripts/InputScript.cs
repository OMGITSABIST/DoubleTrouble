using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    Vector2 move = Vector2.zero;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow)){
            move.x = 1;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow)){
            move.x = -1;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow)){
            move.y = 1;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow)){
            move.y = -1;
        }
        if (move != Vector2.zero){
            foreach (Controls Player in FindObjectsOfType<Controls>()){
                if (Player.ismoving == true) return;
            }
            foreach (Controls Player in FindObjectsOfType<Controls>()){
                Player.StartCoroutine(Player.Movement(move));
            }
            move = Vector2.zero;
        }
    }
}
