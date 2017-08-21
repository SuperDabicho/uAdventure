using UnityEngine;
using System.Collections;
using uAdventure.Core;
using uAdventure.Editor;
using UnityEditor;
namespace uAdventure.Minigame
{
	[EditorWindowExtension(160, typeof(Minigame))]
	public class MinigamesWindowsExtension : ReorderableListEditorWindowExtension {

		private Minigame selectedMinigame;
		private static Rect panelAll;

		public MinigamesWindowsExtension(Rect rect, GUIStyle style, params GUILayoutOption[] options)
			: base(rect, new GUIContent("Minigames"), style, options)
		{
			var content = new GUIContent();


			// Button
			content.image = (Texture2D) Resources.Load("EAdventureData/img/icons/items", typeof(Texture2D));
			content.text = "Minigames";
			ButtonContent = content;
		}



		public override void Draw (int aID)
		{

			var windowWidth = m_Rect.width;
			var windowHeight = m_Rect.height;

			panelAll = new Rect (0f, 0.1f * windowHeight, windowWidth, windowHeight);

			if (selectedMinigame == null) {
				EditorGUILayout.LabelField ("Select or create a new Minigame");
				return;
				//Controller.Instance.SelectedChapterDataControl;
			}

			selectedMinigame.Id = EditorGUILayout.DelayedTextField ("Minigame Name", selectedMinigame.getId ());

			GUILayout.BeginVertical ();
			GUILayout.Space (20);
			GUILayout.EndVertical ();
			
			for (int i = 0; i < selectedMinigame.Preguntas.Length; i++) {

				GUILayout.BeginHorizontal ();
				{
					selectedMinigame.Preguntas [i] = EditorGUILayout.DelayedTextField ("Option" + (i+1), selectedMinigame.Preguntas [i], GUILayout.Width(0.45f*windowWidth));

					GUILayout.Space (0.08f * windowWidth);

					selectedMinigame.Respuestas [i] = EditorGUILayout.DelayedTextField ("Answer" + (i+1), selectedMinigame.Respuestas [i], GUILayout.Width(0.45f*windowWidth));

				}
				GUILayout.EndHorizontal ();
			}

			GUILayout.BeginVertical ();
			GUILayout.Space (10);
			GUILayout.EndVertical ();

			GUILayout.BeginHorizontal ();
			{
				GUILayout.Space (0.1f * windowWidth);

				if (GUILayout.Button("Add Option", GUILayout.Width(0.3f*windowWidth)))
				{
					Debug.Log ("boton add option");
					selectedMinigame.addOption ();
				}

				GUILayout.Space (0.2f * windowWidth);

				if (GUILayout.Button("Delete Option", GUILayout.Width(0.3f*windowWidth))) //mas adelante será un boton por cada opcion. ahora eliminamos la ultima.
				{
					Debug.Log ("boton delete option");
					selectedMinigame.delOption ();
				}

				GUILayout.Space (0.1f * windowWidth);
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginVertical ();
			GUILayout.Space (20);
			GUILayout.EndVertical ();

			EditorGUILayout.LabelField("Documentation");
			selectedMinigame.Documentation = EditorGUILayout.TextArea(selectedMinigame.Documentation, GUILayout.Height(50));

			GUILayout.BeginVertical ();
			GUILayout.Space (20);
			GUILayout.EndVertical ();

			GUILayout.BeginHorizontal ();
			{
				GUILayout.Space (0.05f * windowWidth);

				if (GUILayout.Button ("Discard changes", GUILayout.Width(0.4f*windowWidth))) 
				{
					Debug.Log ("boton Discard. guarda el minigame en el archivo de conf");
					//selectedMinigame.setId (idName);
				}

				GUILayout.Space (0.1f * windowWidth);

				if (GUILayout.Button ("Save changes", GUILayout.Width(0.4f*windowWidth))) 
				{
					Debug.Log ("boton Save. carga de nuevo el archivo de conf");
					//selectedMinigame.setId (idName);
				}

			}
			GUILayout.EndHorizontal ();
			/*

			EditorGUILayout.BeginVertical ();
			{
				EditorGUI.BeginChangeCheck ();
				GUILayout.BeginArea (panelAll);
				string idName = EditorGUILayout.DelayedTextField ("Minigame Name", selectedMinigame.getId());
				GUILayout.EndArea ();
				//Initialize the first text field		
				//if(EditorGUI.EndChangeCheck()){}



				GUILayout.BeginArea (optionsPanelRect);
				for (int i = 0; i < selectedMinigame.Preguntas.Length; i++) {
					selectedMinigame.Preguntas [i] = EditorGUILayout.DelayedTextField ("Option" + i, selectedMinigame.Preguntas [i]);
				}
				GUILayout.EndArea ();

				GUILayout.BeginArea (answersPanelRect);
				for (int i = 0; i < selectedMinigame.Respuestas.Length; i++) {
					selectedMinigame.Respuestas [i] = EditorGUILayout.DelayedTextField ("Answer" + i, selectedMinigame.Respuestas [i]);
				}
				GUILayout.EndArea ();
		
				GUILayout.BeginArea (panelFin);
				EditorGUILayout.LabelField("Documentation");
				selectedMinigame.Documentation = EditorGUILayout.TextArea(selectedMinigame.Documentation,GUILayout.Height(20));

				if (GUILayout.Button("Save changes"))
				{
					Debug.Log ("boton save hanges minigame nuevo");
					selectedMinigame.setId (idName);
				}
				GUILayout.EndArea ();
			}
			EditorGUILayout.EndVertical ();*/
		
		}
			


		protected override void OnElementNameChanged (UnityEditorInternal.ReorderableList r, int index, string newName)
		{
			Controller.Instance.SelectedChapterDataControl.getObjects<Minigame>()[index].Id = newName;
		}

		protected override void OnAdd (UnityEditorInternal.ReorderableList r)
		{
			Controller.Instance.SelectedChapterDataControl.getObjects<Minigame>().Add(new Minigame("newMinigame"));
		}

		protected override void OnUpdateList (UnityEditorInternal.ReorderableList r)
		{
			r.list = Controller.Instance.SelectedChapterDataControl.getObjects<Minigame>().ConvertAll(minigame => minigame.Id);
		}

		protected override void OnAddOption (UnityEditorInternal.ReorderableList r, string option)
		{}

		protected override void OnSelect (UnityEditorInternal.ReorderableList r)
		{
			if(r.index == -1)
			{
				selectedMinigame = null;
				return;
			}

			var newSelection = Controller.Instance.SelectedChapterDataControl.getObjects<Minigame>()[r.index];
			if(newSelection != null && newSelection != selectedMinigame)
			{
				selectedMinigame = newSelection;
				//RegenerateQR();
			}
		}
		protected override void OnRemove (UnityEditorInternal.ReorderableList r)
		{
			Controller.Instance.SelectedChapterDataControl.getObjects<Minigame>().RemoveAt(r.index);
		}
		protected override void OnReorder (UnityEditorInternal.ReorderableList r)
		{
			string idToMove = r.list [r.index] as string;
			var temp = Controller.Instance.SelectedChapterDataControl.getObjects<Minigame> ();
			Minigame toMove = temp.Find (minigame => minigame.getId () == idToMove);
			temp.Remove (toMove);
			temp.Insert (r.index, toMove);
		}



		protected override void OnButton ()
		{
			selectedMinigame = null;
			reorderableList.index = -1;
		}




		}
}
