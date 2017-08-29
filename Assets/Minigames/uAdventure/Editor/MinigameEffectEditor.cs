using UnityEngine;
using System.Collections;

using uAdventure.Core;
using uAdventure.Editor;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

namespace uAdventure.Minigame
{
	public class MinigameEffectEditor : EffectEditor
	{
		private string[] minigames;
		private MinigameEffect effect;
		private bool collapsed = false;
		public bool Collapsed { get { return collapsed; } set { collapsed = value; } }
		private Rect window = new Rect(0, 0, 300, 0);
		public Rect Window
		{
			get
			{
				if (collapsed) return new Rect(window.x, window.y, 50, 30);
				else return window;
			}
			set
			{
				if (collapsed) window = new Rect(value.x, value.y, window.width, window.height);
				else window = value;
			}
		}

		public bool Usable
		{
			get
			{
				return (Controller.Instance.SelectedChapterDataControl.getObjects<Minigame>().ConvertAll(mg=>mg.Id).Count>0);
			}
		}
		public MinigameEffectEditor()
		{
			minigames = Controller.Instance.SelectedChapterDataControl.getObjects<Minigame>().ConvertAll(mg=>mg.Id).ToArray();
			this.effect = new MinigameEffect(minigames.Length > 0 ? minigames[0] : "");
		}

		public void draw()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Minigame");

			effect.setMinigameId(minigames[EditorGUILayout.Popup(Array.IndexOf(minigames, effect.getMinigameId()), minigames)]);

			EditorGUILayout.EndHorizontal();

			EditorGUILayout.HelpBox("Select the minigame to launch", MessageType.Info);
		}

		public AbstractEffect Effect
		{
			get { return effect; }
			set { effect = value as MinigameEffect; }
		}

		public string EffectName { get { return "Launch minigame"; } }
		public EffectEditor clone() { return new MinigameEffectEditor(); }

		public bool manages(AbstractEffect c)
		{
			return c.GetType() == effect.GetType();
		}


	}
}
