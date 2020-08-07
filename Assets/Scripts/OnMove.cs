using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMove : MonoBehaviour
{
    public PolygonCollider2D hole2DCollider;
    public PolygonCollider2D ground2DCollider;
    public MeshCollider generatedMeshCollider;
    public Collider groundCollider;
    private float initialScale = .5f;
    Mesh generatedMesh;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GameObject[] allGO = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var go in allGO)
        {
            if(go.layer == LayerMask.NameToLayer("Obstacle")){
                Physics.IgnoreCollision(go.GetComponent<Collider>(), generatedMeshCollider, true);
            }
        }
    }

    private /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(transform.hasChanged == true){
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            hole2DCollider.transform.localScale = transform.localScale * initialScale;
            hole2D();
            make3DMeshCollider();
        }
    }

    private /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, true);
        Physics.IgnoreCollision(other, generatedMeshCollider, false);
    }

    /// <summary>
    /// OnTriggerExit is called when the Collider other has stopped touching the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, groundCollider, false);
        Physics.IgnoreCollision(other, generatedMeshCollider, true);
    }

    private void hole2D(){
        Vector2[] pointPositions =  hole2DCollider.GetPath(0);

        for(int i=0; i<pointPositions.Length; i++){
            pointPositions[i] = hole2DCollider.transform.TransformPoint(pointPositions[i]);
            //pointPositions[i] += (Vector2)hole2DCollider.transform.position;
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, pointPositions);
    }

    private void make3DMeshCollider(){
        if(generatedMesh != null){Destroy(generatedMesh);}
        generatedMesh = ground2DCollider.CreateMesh(true, true);
        generatedMeshCollider.sharedMesh = generatedMesh;

    }
}
