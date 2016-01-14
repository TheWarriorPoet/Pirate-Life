using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Player))]
public class PlayerInspector : Editor
{
	public override void OnInspectorGUI()
	{
		Player p = target as Player;

		GUILayout.Label("Player Settings", EditorStyles.boldLabel);

		p.playerMode = (Player.PlayerMode) EditorGUILayout.EnumPopup("Player Mode", p.playerMode);

		p.drunkenness = EditorGUILayout.IntSlider("Drunkenness", p.drunkenness, 0, 100);

        p.drunkDelay = EditorGUILayout.FloatField("Drunk Delay", p.drunkDelay);

        p.rumStrength = EditorGUILayout.IntField("Rum Strength", p.rumStrength);
		p.waterStrength = EditorGUILayout.IntField("Water Strength", p.waterStrength);

		Vector2 runSpeed = EditorGUILayout.Vector2Field("Run Speed (Sober, Drunk)", new Vector2(p.minRunSpeed, p.maxRunSpeed));
		p.minRunSpeed = runSpeed.x;
		p.maxRunSpeed = runSpeed.y;

		Vector2 jumpHeight = EditorGUILayout.Vector2Field("Jump Height (Sober, Drunk)", new Vector2(p.minJumpHeight, p.maxJumpHeight));
		p.minJumpHeight = jumpHeight.x;
		p.maxJumpHeight = jumpHeight.y;

		Vector2 laneDelay = EditorGUILayout.Vector2Field("Lane Delay (Sober, Drunk)", new Vector2(p.minLaneDelay, p.maxLaneDelay));
		p.minLaneDelay = laneDelay.x;
		p.maxLaneDelay = laneDelay.y;

		p.laneDistance = EditorGUILayout.FloatField("Lane Distance", p.laneDistance);

		p.currentLane = EditorGUILayout.IntField("Current Lane", p.currentLane);

		p.isTurning = EditorGUILayout.Toggle("Player Is Turning", p.isTurning);

		GUILayout.Label("Touch Settings", EditorStyles.boldLabel);

		p.swipeDistance = EditorGUILayout.FloatField("Swipe Distance", p.swipeDistance);
		p.swipeTime = EditorGUILayout.FloatField("Swipe Time", p.swipeTime);

		//base.OnInspectorGUI();
	}
}
