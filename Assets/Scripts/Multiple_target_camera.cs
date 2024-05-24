using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Camera))]
public class Multiple_target_camera : MonoBehaviour
{
    public List<Transform> players;

    public Vector3 offset;
    public float smoothTime = .5f;
    private Vector3 velocity;

    public float minZoom = 40f;
    public float maxZoom = 10f;
    public float minZoom2 = 60f; 
    public float zoomLimiter = 50f;

    private Camera cam;

    void Start(){
        cam = GetComponent<Camera>();
    }

    void LateUpdate(){//move camera after the two players have moved in the scene, lateupdate is also called every frame but just after the update function.
        if(players.Count == 0){//esta condicion es unicamente para evitar errores si llega a suceder que no hay nigun jugador 
            return;
        }

        Move();
        Zoom();
    }

    void Zoom(){
       float newZoom = Mathf.Lerp(maxZoom,minZoom,GetGreatestDistance()/zoomLimiter);
       cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);

       float newZoom2 = Mathf.Lerp(maxZoom,minZoom2,GetGreatestDistance2()/30f);
       cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom2, Time.deltaTime);
    }

    float GetGreatestDistance(){
        var bounds = new Bounds(players[0].position, Vector3.zero);
        for(int i = 0; i < players.Count; i++){
            bounds.Encapsulate(players[i].position);
        }

        return bounds.size.x;
    }
    float GetGreatestDistance2()
    {
        var bounds = new Bounds(players[0].position, Vector3.zero);
        for (int i = 0; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].position);
        }

        return bounds.size.y;

    }
    void Move(){
        Vector3 center = GetCenterPoint();

        Vector3 newPosition =  center + offset;

        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);

    }

    Vector3 GetCenterPoint(){
        if(players.Count == 1){
            return players[0].position;
        }

        var bounds = new Bounds(players[0].position, Vector3.zero);
        for(int i = 0; i < players.Count; i++){
            bounds.Encapsulate(players[i].position);
        }

        return bounds.center;
    }
}
