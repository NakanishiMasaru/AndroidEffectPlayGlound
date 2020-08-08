using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragControl : MonoBehaviour , IDragHandler, IEndDragHandler
{
    private Vector3 _targetPos;
    [SerializeField] private Vector2 contVal;
    private bool _first = true;
    [SerializeField] private Material shader;
    private Camera cam;
    private Vector2 _aspect;
    private static readonly int Speed = Shader.PropertyToID("_speed");
    private static readonly int Gain = Shader.PropertyToID("_gain");

    // Start is called before the first frame update
    void Start()
    {
        
        cam = Camera.main;
        if (cam != null)
        {
            _aspect.x = cam.pixelWidth;
            _aspect.y = cam.pixelHeight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Camera.main != null)
        {
            
            var nowPos = new Vector3(eventData.position.x/_aspect.x,eventData.position.y/_aspect.y);
            if (_first)
            {
                _targetPos = nowPos;
                _first = false;
            }
            contVal.x =  _targetPos.x - nowPos.x;
            contVal.y = _targetPos.y - nowPos.y;
            _targetPos = nowPos;
            var speed = shader.GetFloat(Speed);
            shader.SetFloat(Speed,Mathf.Clamp(speed-contVal.y*10,1f,50f));
            var gain = shader.GetFloat(Gain);
            shader.SetFloat(Gain,Mathf.Clamp(gain-contVal.x,0f,1f));
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _first = true;
        contVal = Vector2.zero;
    }
}
