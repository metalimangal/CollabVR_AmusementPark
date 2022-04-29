using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using EscapeRoom01;


	/// <summary>
	/// This will be picked up automatically by the wrist watch when it get spawn in the scene by the Interaction toolkit
	/// and setup the buttons and the linked events on the canvas
	/// </summary>
	public class ER01_UIHook : MyWatchScript.IUIHook
	{
		public GameObject LeftUILineRenderer;
		public GameObject RightUILineRenderer;
		
		public GameObject gameSceneManager;
		
		public override void GetHook(MyWatchScript watch)
		{
			//watch.AddButton("Scene Lobby", () => { SceneManager.LoadScene(0); });
			//watch.AddButton("Scene Lobby", () => { gameSceneManager.GetComponent<GameSceneManager>().LeaveRoom; });
			/*watch.AddButton("Menu", () => { 
						gameSceneManager.GetComponent<GameSceneManager>().ShouldLeaveRoom = true;
						gameSceneManager.GetComponent<GameSceneManager>().BackToMainLobby = false;
						gameSceneManager.GetComponent<GameSceneManager>().BackToSubLobby = true;
						});*/
			/*watch.AddButton("Help", () => {
						watch.ToggleInstructions();
						//gameSceneManager.GetComponent<GameSceneManager>().BackToMainLobby = true;
						//gameSceneManager.GetComponent<GameSceneManager>().BackToSubLobby = false;
						});*/
			watch.AddButton("Home", () => {
						gameSceneManager.GetComponent<GameSceneManager>().ShouldLeaveRoom = true;
						gameSceneManager.GetComponent<GameSceneManager>().BackToMainLobby = true;
						gameSceneManager.GetComponent<GameSceneManager>().BackToSubLobby = false;
						});
			//watch.AddToggle("Closed Caption", (state) => { CCManager.Instance.gameObject.SetActive(state); });

			LeftUILineRenderer.SetActive(false);
			RightUILineRenderer.SetActive(false);
			
			//LeftUILineRenderer.SetActive(true);
			//RightUILineRenderer.SetActive(true);

			watch.LeftUILineRenderer = LeftUILineRenderer;
			watch.RightUILineRenderer = RightUILineRenderer;
		}
	}

