using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    
    #region sinleton
    public static GameManager instance;
    void Awake()
    {
        if(instance==null){instance=this;}
    }

    #endregion

    [Header("Level Settings")]
    public int level = 0;
    public int part = 0;
    [SerializeField] private int maxLevel = 1;
    [Space]

    UIManager ui;
    LevelManager levelManager;
    Magnet magnet;

    [Header("Hole Settings")]
    public GameObject hole;
    public bool canMove = true;
    public Vector3 startPos;
    public bool autoMod = false;
    public float hallPassSpeed = 0.2f;

    [Header("Camera Settings")]
    public Camera camera;
    public Vector3 cam0, cam1;

    [Header("Hall Settings")]
    public GameObject gates;
    public float gateSpeed = 10;

    [Header("Gameover Animation")]
    public float shakeMagnitude = 0.3f;
    public float shakeTime = 1.5f;
    public float shakeDelay = 0.05f;

    [Space]
    [Header("Levelup Animation")]
    [SerializeField] ParticleSystem Confetti;

    Vector3 camPos;


    // Start is called before the first frame update
    void Start()
    {
        autoMod = false;
        ui = FindObjectOfType<UIManager>();
        levelManager = FindObjectOfType<LevelManager>();
        magnet = FindObjectOfType<Magnet>();
        maxLevel = levelManager.maxLevel();
        startPos = hole.transform.position;
        cam0 = camera.transform.position;
        cam1 = cam0+new Vector3(0f,0f,9.55f);
    }


    IEnumerator levelUpAnim(){
        magnet.magnetEnable = false;
        magnet.clearMagnet();
        canMove=false;
        Confetti.Play();
        yield return new WaitForSeconds(Confetti.time+1);

        part = 0;
        ui.updateBar();
        levelManager.loadLevel();
        hole.transform.position = startPos;
        camera.transform.position = cam0;
        gates.transform.position = new Vector3(0f,0f,0f);
        ui.startUI.SetActive(true);
        
        yield return new WaitForSeconds(1);
        canMove=true;
        magnet.magnetEnable = true;
        StopCoroutine("levelUpAnim");

    }

    [ContextMenu("Level Up")]
    public void levelUP(){
        Debug.Log("Level Up");
        if(level+1<=maxLevel){
            level +=1;
        }
        else{
            level = level;
        }
        StartCoroutine("levelUpAnim");


        
    }

    void shakeCamera(){
        float shakeXOffset = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        float shakeYOffset = Random.value * shakeMagnitude * 2 - shakeMagnitude;
        camera.transform.position = camPos + new Vector3(shakeXOffset, shakeYOffset, 0f);
    }

    IEnumerator gameOverAnim(){
        magnet.magnetEnable = false;
        magnet.clearMagnet();
        canMove=false;
        camPos = camera.transform.position;
        InvokeRepeating("shakeCamera", 0f, shakeDelay);
        yield return new WaitForSeconds(shakeTime);//Invoke("Stop",time);
        CancelInvoke("shakeCamera");

        part = 0;
        ui.updateBar();
        levelManager.loadLevel();
        hole.transform.position = startPos;
        camera.transform.position = cam0;
        gates.transform.position = new Vector3(0f,0f,0f);
        ui.startUI.SetActive(true);
        
        yield return new WaitForSeconds(1);
        canMove=true;
        magnet.magnetEnable = true;
        StopCoroutine("gameOverAnim");

    }

    public void gameOver(){
        Debug.Log("Game Over");
        StartCoroutine("gameOverAnim");
    }

    [ContextMenu("Next Part")]
    public void nextPart(){
        magnet.magnetEnable=false;
        magnet.clearMagnet();
        canMove = false;
        autoMod = true;
        part++;
    }

    // Update is called once per frame
    void Update()
    {
        if(autoMod){
            Vector3 hp = hole.transform.position;
            if(hp.x != 0f || gates.transform.position.y != -1f){
                gates.transform.position = Vector3.MoveTowards(gates.transform.position, new Vector3(0f, -1f, 0f), gateSpeed*Time.deltaTime);
                hole.transform.position = Vector3.MoveTowards(hp, new Vector3(0f, hp.y, hp.z), hallPassSpeed*Time.deltaTime);
            }else{
                if(hp.z != 2.75f || camera.transform.position.z != cam1.z){
                    hole.transform.position = Vector3.MoveTowards(hp, new Vector3(hp.x, hp.y, 2.75f), hallPassSpeed*Time.deltaTime);
                    camera.transform.position = Vector3.MoveTowards(camera.transform.position, cam1, hallPassSpeed*Time.deltaTime);
                }else{
                    canMove = true;
                    autoMod = false;
                    magnet.magnetEnable = true;
                }
            }
        }
    }
}
