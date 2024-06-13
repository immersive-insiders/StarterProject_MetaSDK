using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CustomPalmMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _controlledObject;

    [SerializeField]
    private Color[] _colors;

    [SerializeField]
    private GameObject _rotationEnabledIcon;

    [SerializeField]
    private GameObject _rotationDisabledIcon;

    [SerializeField]
    private float _rotationLerpSpeed = 1f;

    [SerializeField]
    private TMP_Text _rotationDirectionText;

    [SerializeField]
    private string[] _rotationDirectionNames;

    [SerializeField]
    private GameObject[] _rotationDirectionIcons;

    [SerializeField]
    private Quaternion[] _rotationDirections;

    [SerializeField]
    private TMP_Text _elevationText;

    [SerializeField]
    private float _elevationChangeIncrement;

    [SerializeField]
    private float _elevationChangeLerpSpeed = 1f;

    private int _currentColorIdx;
    private bool _rotationEnabled;
    private int _currentRotationDirectionIdx;
    private Vector3 _targetPosition;
    private Material productMaterial;

    private void Start()
    {
        _currentColorIdx = _colors.Length;

        _rotationEnabled = false;
        ToggleRotationEnabled();

        _currentRotationDirectionIdx = _rotationDirections.Length;
        CycleRotationDirection();

        _targetPosition = _controlledObject.transform.position;
        IncrementElevation(true);
        IncrementElevation(false);
    }

    private void Update()
    {
        if (_rotationEnabled)
        {
            var rotation = Quaternion.Slerp(Quaternion.identity, _rotationDirections[_currentRotationDirectionIdx], _rotationLerpSpeed * Time.deltaTime);
            _controlledObject.transform.rotation = rotation * _controlledObject.transform.rotation;
        }

        _controlledObject.transform.position = Vector3.Lerp(_controlledObject.transform.position, _targetPosition, _elevationChangeLerpSpeed * Time.deltaTime);
    }

    /// <summary>
    /// Change the color of the controlled object to the next in the list of allowed colors, looping if the end of the list is reached.
    /// </summary>
    public void CycleColor()
    {
        _currentColorIdx += 1;
        if (_currentColorIdx >= _colors.Length)
        {
            _currentColorIdx = 0;
        }

        productMaterial.SetColor("_Color", _colors[_currentColorIdx]);
    }

    /// <summary>
    /// Toggle whether or not rotation is enabled, and set the icon of the controlling button to display what will happen next time the button is pressed.
    /// </summary>
    public void ToggleRotationEnabled()
    {
        _rotationEnabled = !_rotationEnabled;
        _rotationEnabledIcon.SetActive(!_rotationEnabled);
        _rotationDisabledIcon.SetActive(_rotationEnabled);
    }

    /// <summary>
    /// Change the rotation direction of the controlled object to the next in the list of allowed directions, looping if the end of the list is reached.
    /// Set the icon of the controlling button to display what will happen next time the button is pressed.
    /// </summary>
    public void CycleRotationDirection()
    {
        Debug.Assert(_rotationDirectionNames.Length == _rotationDirections.Length);
        Debug.Assert(_rotationDirectionNames.Length == _rotationDirectionIcons.Length);

        _currentRotationDirectionIdx += 1;
        if (_currentRotationDirectionIdx >= _rotationDirections.Length)
        {
            _currentRotationDirectionIdx = 0;
        }

        int nextRotationDirectionIdx = _currentRotationDirectionIdx + 1;
        if (nextRotationDirectionIdx >= _rotationDirections.Length)
        {
            nextRotationDirectionIdx = 0;
        }

        _rotationDirectionText.text = _rotationDirectionNames[nextRotationDirectionIdx];
        for (int idx = 0; idx < _rotationDirections.Length; ++idx)
        {
            _rotationDirectionIcons[idx].SetActive(idx == nextRotationDirectionIdx);
        }
    }

    /// <summary>
    /// Change the target elevation of the controlled object in the requested direction, within the limits [0.2, 2].
    /// Set the text to display the new target elevation.
    /// </summary>
    public void IncrementElevation(bool up)
    {
        float increment = _elevationChangeIncrement;
        if (!up)
        {
            increment *= -1f;
        }
        _targetPosition = new Vector3(_targetPosition.x, Mathf.Clamp(_targetPosition.y + increment, 0.2f, 2f), _targetPosition.z);
        _elevationText.text = "Elevation: " + _targetPosition.y.ToString("0.0");
    }

    public void SetProductRenderer(GameObject product)
    {
        productMaterial = product.GetComponent<Renderer>().material;
    }
}
