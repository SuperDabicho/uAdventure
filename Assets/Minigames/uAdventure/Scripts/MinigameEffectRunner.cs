using UnityEngine;
using System.Collections;
using uAdventure.Runner;
using uAdventure.Core;

namespace uAdventure.Minigame
{
	[CustomEffectRunner(typeof(MinigameEffect))]
	public class MinigameEffectRunner : CustomEffectRunner {

		MinigameEffect effect;
		//private GameObject QRPromptPrefab;
		//private QRPrompt prompt;
		bool first;

		public MinigameEffectRunner()
		{
			first = true;
			//QRPromptPrefab = Resources.Load<GameObject>("QRPrompt");
		}

		#region Secuence implementation

		public bool execute ()
		{
			if (first)
			{
				first = false;
				//prompt = GameObject.Instantiate(QRPromptPrefab).GetComponent<QRPrompt>();
				//prompt.Effect = this.Effect;
			}
			return first;
		}

		#endregion

		#region CustomEffectRunner implementation

		public Effect Effect {
			get {
				return effect;
			}
			set {
				effect = value as MinigameEffect;
			}
		}

		#endregion
	}
}