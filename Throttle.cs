﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

// Class controls how throttle is moved, and actions on movement

public class Throttle : MonoBehaviour
{

    public Slider throttleSlider;  // slider added to script via Unity Engine Inspector
    private RectTransform airspeedNeedle;

    // const variables are set at compile time
    private const float sliderMinValue = 1;
    private const float sliderMaxValue = 3;
    private const float sliderIncrementValue = 1;  // slider increments in values of 1 on keyboard key input

    // SerializedField lets private varaible show in the inspector, allowing the ParticleSystems to be added from inspector
    [SerializeField] private List<ParticleSystem> particleList;

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        controls.ControllerInput.LeftThumbstick.performed += context => MoveUiThrottle(context.ReadValue<Vector2>());// context cant be used to get input information
    }
    // Start is called before the first frame update
    void Start()
    {
        controls.ControllerInput.Enable();  // Start with the keyboard controls enabled
        throttleSlider.minValue = sliderMinValue;  // Slider max value (top) is 3
        throttleSlider.maxValue = sliderMaxValue;  // Slider min value (bottom) is 1
        airspeedNeedle = GameObject.FindGameObjectWithTag("SpeedNeedle").GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))  // Faster
        {
            throttleSlider.value += sliderIncrementValue;
        }
    
        if (Input.GetKeyDown(KeyCode.X))  // Slower
        {
            throttleSlider.value -= sliderIncrementValue;
        }

        UpdateClouds(); // update the cloud movement speed based on throttle movement
        MoveAirspeedNeedle(throttleSlider.value);  // Move pointer in relation to throttle slider value
    }

    public void UpdateClouds()
    {
            foreach (ParticleSystem cloud in particleList)
            {
                ParticleSystem.VelocityOverLifetimeModule velocity = cloud.velocityOverLifetime;
                velocity.speedModifier = throttleSlider.value;  // between 1 and 3

                ParticleSystem.EmissionModule emission = cloud.emission;
                emission.rateOverTime = (throttleSlider.value / 10) * 5;
            }
    }

    private void MoveAirspeedNeedle(float sliderValue)
    {
        float newSpeedNeedleValue = sliderValue * 142;
        airspeedNeedle.anchoredPosition = new Vector2(airspeedNeedle.anchoredPosition.x, newSpeedNeedleValue);  // 142 * throttle number of 1-3 corresponds to needle x axis position
    }


    private void MoveUiThrottle(Vector2 context)
    {
        Vector2 leftStickCoords = context;
        throttleSlider.value = leftStickCoords.y + 2;
    }

}
