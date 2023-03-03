using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMove : MonoBehaviour
{
    [SerializeField]
    private Vector2 _offsetDir;
    [SerializeField]
    private float _speed = 1f;

    private MeshRenderer _meshRenderer = null;

    private Vector2 _offset = Vector2.zero;

    private void Start() {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Update() {
        SetOffset();
    }
    private void SetOffset(){
        _offset += _offsetDir * Time.deltaTime * _speed;
        _meshRenderer.material.SetTextureOffset("_MainTex",_offset);
    }
}
