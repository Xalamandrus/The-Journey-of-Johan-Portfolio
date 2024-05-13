using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private string[] _dialogueLines;
    [SerializeField] private TextMesh _textMesh;
    [SerializeField] private SpriteRenderer _buttonERenderer;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private AxeThrow _axeThrow;
    [SerializeField] private Rigidbody2D _rb;

    private int _index = 0;

    private bool _talkable = false;
    private bool _talking = false;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && _talkable == true)
        {
            _textMesh.text = _dialogueLines[_index];
            _index++;
            _talkable = false;
            _talking = true;
            _playerMovement.enabled = false;
            _axeThrow.enabled = false;
            _rb.velocity = new Vector3(0, 0, 0);
        }
        else if(Input.GetKeyDown(KeyCode.E) && _talking == true && _index < _dialogueLines.Length)
        {
            _textMesh.text = _dialogueLines[_index];
            _index++;
        }
        else if(Input.GetKeyDown(KeyCode.E) && _index == _dialogueLines.Length && _talking == true)
        {
            _talking = false;
            _index = 0;
            _textMesh.text = "";
            _playerMovement.enabled = true;
            _axeThrow.enabled = true;
            _talkable = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            _buttonERenderer.enabled = true;
            _talkable = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _buttonERenderer.enabled = false;
            _talkable = false;
        }
    }
}
