using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class WackButton : MonoBehaviour
{
    [SerializeField] Material _normalMaterial;
    [SerializeField] Material _activeMaterial;
    [SerializeField] MeshRenderer _meshRender;

    public UnityEvent<WackButton> OnHit = new UnityEvent<WackButton>();

    [SerializeField] private TextMeshProUGUI _characterDisplay;

    bool _active = false;
    private void Awake()
    {
        if (_characterDisplay == null)
        {
            _characterDisplay = GetComponentInChildren<TextMeshProUGUI>();
            if (_characterDisplay == null)
                Debug.LogError("Character display is not set for a WackButton!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        //if (other.Tag == "Boxes")
        
        if (_active)
        {
            
            Deactivate();

            OnHit.Invoke(this);
        }
        

    }

    public string Character
    {
        get { return _characterDisplay.text; }
        set { _characterDisplay.text = value; }
    }

    public void Activate()
    {
        _meshRender.material = _activeMaterial;
        _active = true;
        
    }

    public void Deactivate()
    {

        _meshRender.material = _normalMaterial;
        _active = false;
        

    }

   
}
