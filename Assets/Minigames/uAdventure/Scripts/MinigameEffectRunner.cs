using UnityEngine;
using System.Collections;
using uAdventure.Runner;
using uAdventure.Core;

namespace uAdventure.Minigame
{
	[CustomEffectRunner(typeof(MinigameEffect))]
	public class MinigameEffectRunner : CustomEffectRunner {

		MinigameEffect effect;
		private GameObject mgPrefab;
		private MinigamePrompt prompt;
		bool first;
	

		public MinigameEffectRunner()
		{
			first = true;
			mgPrefab = Resources.Load<GameObject>("mgPrompt");
		}

		#region Secuence implementation

		public bool execute ()
		{
			if (first)
			{
				first = false;
				effect = (MinigameEffect)Effect;
				mgPrefab.GetComponent<MinigamePrompt> ().idMinigame = effect.getMinigameId();
				prompt = GameObject.Instantiate(mgPrefab).GetComponent<MinigamePrompt>();
				prompt.Effect = this.Effect;
			}
			return prompt && prompt.execute();
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