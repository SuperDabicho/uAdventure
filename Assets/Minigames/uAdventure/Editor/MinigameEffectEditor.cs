using UnityEngine;
using System.Collections;
using uAdventure.Core;
using uAdventure.Editor;
using UnityEditor;


namespace uAdventure.Minigame
{
public class MinigameEffectEditor : EffectEditor {

	MinigameEffect effect = new MinigameEffect();

	#region EffectEditor implementation

	public void draw ()
	{
		EditorGUILayout.LabelField ("Minigame test");
	}

	public EffectEditor clone ()
	{
		return new MinigameEffectEditor ();
	}

	public bool manages (AbstractEffect c)
	{
		return c is MinigameEffect;
	}

	public AbstractEffect Effect {
		get {
			return effect;
		}
		set {
			effect = value as MinigameEffect;
		}
	}

	public string EffectName {
		get {
			return "Launch minigame";
		}
	}

	public bool Collapsed {
		get;
		set;
	}

	public Rect Window {
		get;
		set;
	}

		public bool Usable {
			get;
		}

	#endregion
}
}