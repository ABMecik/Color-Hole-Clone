using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{

    [Header("Vibration Settings")]
    public bool vibrate = true;

    GameManager gm;
    LevelManager levelManager;
    UIManager ui;

    Magnet magnet;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        levelManager = FindObjectOfType<LevelManager>();
        ui = FindObjectOfType<UIManager>();
        magnet = FindObjectOfType<Magnet>();
    }

    public void vibrateSetting(){
        vibrate = !vibrate;
    }


    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;

        if(tag.Equals("Object")){
            if(!gm.autoMod){
                if(gm.part == 0){
                    levelManager.part0Current--;
                    ui.updatePB(0);
                    if(levelManager.part0Current == 0){
                        gm.nextPart();
                    }
                }else{
                    levelManager.part1Current--;
                    ui.updatePB(1);
                    if(levelManager.part1Current == 0){
                        gm.levelUP();
                    }
                }
                if(vibrate){
                    Handheld.Vibrate();
                }
            }
        }else if(tag.Equals("Obstacle")){
            if(!gm.autoMod && gm.canMove){
                gm.gameOver();
            }
        }
        Destroy(other.gameObject);
    }
}
