using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
	[ExecuteInEditMode]
	public class Grayscale : ImageEffectBase
	{
		public Texture textureRamp;

		[Range(-1f, 1f)]
		public float rampOffset;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", textureRamp);
			base.material.SetFloat("_RampOffset", rampOffset);
			Graphics.Blit(source, destination, base.material);
		}
	}
}
