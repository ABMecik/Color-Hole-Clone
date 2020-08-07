using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMove : MonoBehaviour
{
    [Header("Game Area Settings")]
    public PolygonCollider2D ground2DCollider;

    [Header("Hole Speed Settings")]
    public float speed = 5f;
    public float Pspeed = 5f;
    public float maxStep = 40f;
    
    
    float x,y;
    Vector3 targetPos;
    Touch touch;
    Vector2[][] limits = new Vector2[2][];
    float holeRad;

    GameManager gm;
    UIManager ui;

    // Start is called before the first frame update
    void Start()
    {

        gm = FindObjectOfType<GameManager>();
        ui = FindObjectOfType<UIManager>();
        

        Vector2[] edges = ground2DCollider.GetPath(0);

        limits[1] = new Vector2[] {edges[1], edges[11]};
        limits[0] = new Vector2[] {edges[5], edges[7]};

        holeRad = transform.localScale.z/2;
    }

    void MoveHole(){
        touch = Input.GetTouch(0);
        if(touch.phase == TouchPhase.Moved){
            Debug.Log("hi");
            x = touch.deltaPosition.x;
            y = touch.deltaPosition.y;
            targetPos = transform.position+new Vector3(x,0f,y);

            float step = Mathf.Clamp((speed*Time.deltaTime), 0, maxStep);

            targetPos = Vector3.Lerp(transform.position, targetPos, step);

            transform.position = new Vector3(
                Mathf.Clamp(targetPos.x, limits[gm.part][0].x+holeRad, limits[gm.part][1].x-holeRad),
                targetPos.y,
                Mathf.Clamp(targetPos.z, limits[gm.part][0].y+holeRad, limits[gm.part][1].y-holeRad)
            );

        }
    }

    void MoveForPc(){
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");
        targetPos = transform.position+new Vector3(x,0f,y);

        float step = Mathf.Clamp((Pspeed*Time.deltaTime), 0, maxStep);

        targetPos = Vector3.Lerp(transform.position, targetPos, step);


        transform.position = new Vector3(
            Mathf.Clamp(targetPos.x, limits[gm.part][0].x+holeRad, limits[gm.part][1].x-holeRad),
            targetPos.y,
            Mathf.Clamp(targetPos.z, limits[gm.part][0].y+holeRad, limits[gm.part][1].y-holeRad)
            );

    }


    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount > 0 && !gm.autoMod && gm.canMove){
            if(ui.startUI.activeSelf){
                ui.startUI.SetActive(false);
            }
            MoveHole();
        }
        else if (Input.GetMouseButton(0) && !gm.autoMod && gm.canMove){
            if(ui.startUI.activeSelf){
                ui.startUI.SetActive(false);
            }
            MoveForPc();
        }
    }
}
