/// Unity3D Scene Chase Cam script
/// Makes the editor's scene cam follow an(some) object(s).
/// 
/// How to use:
/// 	-Create a ChaseCam window by clicking Window->Chase Cam
/// 	-"Active" toggle will turn On/Off your script
/// 	(it will run only while the game is playing)
/// 	-If "Capture Selection" is On, this script will capture
/// 	any click in Hierarchy or Scene views and automatically
/// 	start chasing the selected transform
///		-"Transform to follow" manually set the transform to
/// 	follow by drag'n drop inside the box or by using
/// 	Unity3D's Object Picker.
/// 	-"Object name's Position" is the same as the Transform
/// 	Position in your inspector view.
/// 
/// 
/// Author: Andrea Giorgio "Muu?" Cerioli
/// Website: www.lanoiadimuu.it
/// License: Modified BSD License
/// 
/// Copyright (c) 2012, Andrea Giorgio Cerioli
/// All rights reserved.
/// 
/// Redistribution and use in source and binary forms, with or without
/// modification, are permitted provided that the following conditions are met:
///     * Redistributions of source code must retain the above copyright
///       notice, this list of conditions and the following disclaimer.
///     * Redistributions in binary form must reproduce the above copyright
///       notice, this list of conditions and the following disclaimer in the
///       documentation and/or other materials provided with the distribution.
///     * The name of the author may not be used to endorse or promote products
///       derived from this software without specific prior written permission.
/// 
/// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
/// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
/// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
/// DISCLAIMED. IN NO EVENT SHALL Andrea Giorgio Cerioli BE LIABLE FOR ANY
/// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
/// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
/// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
/// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
/// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
/// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
/// 
using UnityEngine;
using UnityEditor;
using System.Collections;

public class ChaseCam : EditorWindow {
	public bool isEnabled = false;						//Should this script run?/
	private float maxButtonWidth = 190;					//Max width for buttons and boxes
	private Vector2 scroll = Vector2.zero; 				//Stores the current scroll position if the window size is too small
	public Transform following = null;					//Stores the followed transform
	public bool canGetFromHierarchy = true;				//Check wheter the script can(not) get the followed transform by clicking on it on hierarchy view

	
	[MenuItem ("Window/Chase Cam")]
	public static void Init () {
		
		// Get an existing one or make a new one, then focus on it.
		ChaseCam window = (ChaseCam)EditorWindow.GetWindow<ChaseCam>();
		window.Focus();
    }
	
	//This past draw the visible GUI. Most, if not all, is explained in this script's header comment.
	public void OnGUI () {
		scroll = GUILayout.BeginScrollView(scroll, GUILayout.MaxWidth(maxButtonWidth+20));
		GUILayout.Label("Chase Cam Settings:", EditorStyles.boldLabel);
		isEnabled = EditorGUILayout.Toggle("Active: ", isEnabled, GUILayout.MaxWidth(maxButtonWidth));
		canGetFromHierarchy = EditorGUILayout.Toggle("Capture selection: ", canGetFromHierarchy, GUILayout.MaxWidth(maxButtonWidth));
		GUILayout.Space(15);
		GUILayout.Label("Transform to follow:", EditorStyles.label);
		if (canGetFromHierarchy && Selection.activeTransform != null)
			following = Selection.activeTransform;
		following = (Transform) EditorGUILayout.ObjectField(following, typeof(Transform), true, GUILayout.MaxWidth(maxButtonWidth));
		if (following != null)
			following.position = EditorGUILayout.Vector3Field(following.name + " position: ", following.position, GUILayout.MaxWidth(maxButtonWidth));
		GUILayout.EndScrollView();
		this.Repaint();
	}
	
	
	//This part runs the script
	//First off, it check if the Chase Cam is enabled and if the game is running.
	//Then, it takes every SceneView we have in our editor
	//and for each one change camera's pivot point to object position
	public void Update () {
		if (isEnabled && Application.isPlaying)
		{
			foreach (SceneView scene in SceneView.sceneViews)
			{
				if (following != null)
				{
					scene.pivot = following.position;
					scene.Repaint();
				}	
			}
		}
	}
}