using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Tongue : MonoBehaviour
{
    private Camera _mainCamera;
    private Collider2D _collider;

    private int _length;
    private int _strength;
    private int _fishCount;

    private bool _canMove = true;
    private Tweener _cameraTween;

    private void Awake()
    {
        _mainCamera = Camera.main;
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (_canMove && Input.GetMouseButton(0))
        {
            Vector3 vector = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _mainCamera.transform.position.z));
            transform.position = new Vector3(vector.x, transform.position.y, transform.position.z);
        }
    }

    public void StartEating()
    {
        _length = 50;
        _strength = 3;
        _fishCount = 0;
        float time = (_length) * 0.01f;

        _cameraTween = _mainCamera.transform.DOMoveY(_length, 1 + time * 0.25f, false).OnUpdate(ExpandTongue).OnComplete(Eating);
    }

    private void ExpandTongue()
    {
        if (_mainCamera.transform.position.y >= 14)
            transform.SetParent(_mainCamera.transform);
    }

    private void Eating()
    {
        float time = (_length) * 0.01f;
        _collider.enabled = true;
        _cameraTween = _mainCamera.transform.DOMoveY(0, time * 5, false).OnUpdate(ShrinkTongue);
    }

    private void ShrinkTongue()
    {
        if (_mainCamera.transform.position.y <= 14f)
            StopFishing();
    }

    private void StopFishing()
    {
        _canMove = false;
        _cameraTween.Kill(false);
        _cameraTween = _mainCamera.transform.DOMoveY(0, 2, false).OnUpdate(GoBack).OnComplete(Something);
    }

    private void GoBack()
    {
        if (_mainCamera.transform.position.y <= 14)
        {
            transform.SetParent(null);
            transform.position = new Vector2(transform.position.x, 11.42f);
        }
    }

    private void Something()
    {
        transform.position = Vector2.down;
        _collider.enabled = true;
        int num = 0;
    }
}
