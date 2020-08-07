using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class Magnet : MonoBehaviour
{

    [Header("Magnet Settings")]
    public bool magnetEnable = true;
    public float magnetForce = 1f;
    List<Rigidbody> effectedObjects = new List<Rigidbody>();
    GameManager gm;
    // Start is called before the first frame update
    void Start()
    {
        effectedObjects.Clear();
        gm = FindObjectOfType<GameManager>();
    }

    public void clearMagnet(){
        effectedObjects.Clear();
    }

    public void removeFromMagnet(Rigidbody rb){
        effectedObjects.Remove(rb);
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if(gm.canMove && (other.tag.Equals("Obstacle") || other.tag.Equals("Object")) && magnetEnable){
            effectedObjects.Add(other.attachedRigidbody);
        }
    }

    private void OnTriggerExit(Collider other) {
        if(gm.canMove && (other.tag.Equals("Obstacle") || other.tag.Equals("Object"))){
            effectedObjects.Remove(other.attachedRigidbody);
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if(gm.canMove && magnetEnable){
            foreach(Rigidbody rb in effectedObjects){
                rb.AddForce((transform.position-rb.position)*magnetForce*Time.fixedDeltaTime);
            }
        }
    }
}
