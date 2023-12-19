using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class RandomColor : MonoBehaviour
{
    private Image _image;
    private Text _text;
    public float timeTochange = 0.1f;
    private float timeSinceChange = 0f;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _text = GetComponent<Text>();
    }

    private void Update()
    {
        timeSinceChange += Time.unscaledDeltaTime;

        if(_image != null && timeSinceChange >= timeTochange)
        {
            Color newColor = new Color(
                Random.value,
                Random.value,
                Random.value
                );

            _image.color = newColor;
            timeSinceChange = 0f;
        }
        
        if(_text != null && timeSinceChange >= timeTochange)
        {
            Color newColor = new Color(
                Random.value,
                Random.value,
                Random.value
            );

            _text.color = newColor;
            timeSinceChange = 0f;
        }
    }
}
