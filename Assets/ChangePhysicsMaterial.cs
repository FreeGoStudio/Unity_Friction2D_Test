using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class ChangePhysicsMaterial : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private GameObject board;
    [SerializeField] private TextMeshProUGUI boxFrictionText;
    [SerializeField] private TextMeshProUGUI boardFrictionText;
    [SerializeField] private TMP_InputField boxFrictionInput;
    [SerializeField] private TMP_InputField boardFrictionInput;
    [SerializeField] private TMP_InputField gravityScaleInput;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI historyText;

    private Collider2D _boardCollider2d;

    private GameObject _currentBox;

    private float _timer;

    private float _boxInputValue;
    private float _boardInputValue;
    private float _gravityScaleValue;

    public void Restart()
    {
        RecordScore();
        _timer = 0;

        Destroy(_currentBox);
        _currentBox = Instantiate(boxPrefab);

        var boardInputText = boardFrictionInput.text;
        boardFrictionText.text = boardInputText;
        _boardInputValue = float.Parse(boardInputText);
        ChangeFriction(_boardInputValue, _boardCollider2d);

        var boxCollider2D = _currentBox.GetComponent<Collider2D>();
        var boxInputText = boxFrictionInput.text;
        boxFrictionText.text = boxInputText;
        _boxInputValue = float.Parse(boxInputText);
        ChangeFriction(_boxInputValue, boxCollider2D);

        var gravityScaleInputText = gravityScaleInput.text;
        _gravityScaleValue = float.Parse(gravityScaleInputText);
        var rigidbody2d= _currentBox.GetComponent<Rigidbody2D>();
        rigidbody2d.gravityScale = _gravityScaleValue;

    }

    // Start is called before the first frame update
    void Start()
    {
        _boardCollider2d = board.GetComponent<Collider2D>();
        boxFrictionInput.text = "0";
        boardFrictionInput.text = "0";
        gravityScaleInput.text = "1";
        historyText.text = "";
    }

    private void ChangeFriction(float frictionValue, Collider2D collider)
    {
        var boardPhysicsMaterial = new PhysicsMaterial2D
        {
            friction = frictionValue
        };
        collider.sharedMaterial = boardPhysicsMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        timerText.text = _timer.ToString();
        if (_currentBox == null)
        {
            return;
        }

        if (_currentBox.transform.position.y > -5)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            RecordScore();
            Destroy(_currentBox);
        }
    }

    private void RecordScore()
    {
        if (_currentBox == null)
        {
            return;
        }
        
        var score = "Inf";
        if (_currentBox.transform.position.y <= -5)
        {
            score = _timer.ToString();
        }

        historyText.text += $"{_boxInputValue}\t{_boardInputValue}\t{_gravityScaleValue}\t{score}\n";
    }
}