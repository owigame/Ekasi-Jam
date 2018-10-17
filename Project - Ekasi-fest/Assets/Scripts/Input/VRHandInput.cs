using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using VRUtilities;

public class VRHandInput : MonoBehaviour {

	public Hand hand;

	[Header ("Settings")]
	public bool debugOutput = false;
	public bool debugLogOutput = false;
	public float triggerThreshold = 0.2f;
	public float gripThreshold = 0.2f;

	[Header ("Input Events")]
	public UnityEvent OnMenuPressed;
	public UnityEvent OnMenuReleased;
	public UnityEvent OnTriggerPressed;
	public UnityEvent OnTriggerReleased;
	public UnityEvent OnGripPressed;
	public UnityEvent OnGripReleased;
	public UnityEvent OnTrackpadPressed;
	public UnityEvent OnTrackpadReleased;
	public UnityEvent OnTrackpadTouchBegin;
	public UnityEvent OnTrackpadTouchEnd;

	[Header ("Axis Outputs")]
	public float triggerAxis;
	public float gripAxis;
	public float touchpadHorizontalAxis;
	public float touchpadVerticalAxis;

	//Privates
	TextMeshPro debugOutputText;
	bool trackpadFirst = true;
	bool gripFirst = true;

	void Start () {
		debugOutputText = GameObject.Find ("DebugOutput").GetComponent<TextMeshPro> ();
	}

	void Update () {
		InputTrigger ();
		InputGrip ();
		InputTrackpad ();
		InputMenu ();
	}

	void InputTrigger () {
		triggerAxis = Input.GetAxis (hand.Prefix () + "TriggerSqueeze");
		if (Input.GetButtonDown (hand.Prefix () + "TriggerPress")) {
			DebugInput ("Trigger Pressed");
			OnTriggerPressed.Invoke ();
		}
		if (triggerAxis > triggerThreshold) {
			if (Input.GetButton (hand.Prefix () + "TriggerPress")) {
				DebugInput ("Trigger Squeeze: " + triggerAxis);
			}
		}
		if (Input.GetButtonUp (hand.Prefix () + "TriggerPress")) {
			triggerAxis = 0;
			DebugInput ("Trigger Released");
			OnTriggerReleased.Invoke ();
		}
	}
	void InputGrip () {
		gripAxis = Input.GetAxis (hand.Prefix () + "GripSqueeze");

		if (gripAxis > gripThreshold) {
			if (gripFirst) {
				DebugInput ("Grip Pressed");
				OnGripPressed.Invoke ();
				gripFirst = false;
			}
			DebugInput ("Grip Squeeze: " + gripAxis);
		} else {
			if (!gripFirst) {
				DebugInput ("Grip Released");
				OnGripReleased.Invoke ();
				gripFirst = true;
			}
		}
	}

	void InputTrackpad () {
		if (Input.GetButtonDown (hand.Prefix () + "TrackpadPress")) {
			DebugInput ("Trackpad Pressed");
			OnTrackpadPressed.Invoke ();
		}
		if (Input.GetButton (hand.Prefix () + "TrackpadTouch")) {
			if (trackpadFirst) {
				DebugInput ("Trackpad Touch Begin");
				OnTrackpadTouchBegin.Invoke ();
				trackpadFirst = false;
			}
			touchpadHorizontalAxis = Input.GetAxis (hand.Prefix () + "TrackpadHorizontal");
			touchpadVerticalAxis = Input.GetAxis (hand.Prefix () + "TrackpadVertical");
			DebugInput ("Trackpad Touch: " + touchpadHorizontalAxis + " | " + touchpadVerticalAxis);
		} else {
			if (!trackpadFirst) {
				DebugInput ("Trackpad Touch End");
				OnTrackpadTouchEnd.Invoke ();
				trackpadFirst = true;
			}
		}
		if (Input.GetButtonUp (hand.Prefix () + "TrackpadPress")) {
			touchpadHorizontalAxis = 0;
			touchpadVerticalAxis = 0;
			DebugInput ("Trackpad Released");
			OnTrackpadReleased.Invoke ();
		}
	}

	void InputMenu () {
		if (Input.GetButtonDown (hand.Prefix () + "MenuPress")) {
			DebugInput ("Menu Pressed");
			OnMenuPressed.Invoke ();
		}
		if (Input.GetButtonUp (hand.Prefix () + "MenuPress")) {
			DebugInput ("Menu Released");
			OnMenuReleased.Invoke ();
		}
	}

	void DebugInput (string _text) {
		if (debugOutput) {
			debugOutputText.text = _text;
		}
		if (debugLogOutput) {
			Debug.Log (_text);
		}
	}

}