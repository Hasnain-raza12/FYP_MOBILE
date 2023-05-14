using System;
using System.Collections.Generic;
using AmplifyColor;
using UnityEngine;

[AddComponentMenu("")]
public class AmplifyColorBase : MonoBehaviour
{
	public const int LutSize = 32;

	public const int LutWidth = 1024;

	public const int LutHeight = 32;

	private const int DepthCurveLutRange = 1024;

	public float Exposure = 1f;

	public bool UseToneMapping;

	public bool UseDithering;

	public Quality QualityLevel = Quality.Standard;

	public float BlendAmount;

	public Texture LutTexture;

	public Texture LutBlendTexture;

	public Texture MaskTexture;

	public bool UseDepthMask;

	public AnimationCurve DepthMaskCurve = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

	public bool UseVolumes;

	public float ExitVolumeBlendTime = 1f;

	public Transform TriggerVolumeProxy;

	public LayerMask VolumeCollisionMask = -1;

	private Camera ownerCamera;

	private Shader shaderBase;

	private Shader shaderBlend;

	private Shader shaderBlendCache;

	private Shader shaderMask;

	private Shader shaderMaskBlend;

	private Shader shaderDepthMask;

	private Shader shaderDepthMaskBlend;

	private Shader shaderProcessOnly;

	private RenderTexture blendCacheLut;

	private Texture2D defaultLut;

	private Texture2D depthCurveLut;

	private Color32[] depthCurveColors;

	private ColorSpace colorSpace = ColorSpace.Uninitialized;

	private Quality qualityLevel = Quality.Standard;

	private Material materialBase;

	private Material materialBlend;

	private Material materialBlendCache;

	private Material materialMask;

	private Material materialMaskBlend;

	private Material materialDepthMask;

	private Material materialDepthMaskBlend;

	private Material materialProcessOnly;

	private bool blending;

	private float blendingTime;

	private float blendingTimeCountdown;

	private Action onFinishBlend;

	private AnimationCurve prevDepthMaskCurve = new AnimationCurve();

	private bool volumesBlending;

	private float volumesBlendingTime;

	private float volumesBlendingTimeCountdown;

	private Texture volumesLutBlendTexture;

	private float volumesBlendAmount;

	private Texture worldLUT;

	private AmplifyColorVolumeBase currentVolumeLut;

	private RenderTexture midBlendLUT;

	private bool blendingFromMidBlend;

	private float worldExposure = 1f;

	private float currentExposure = 1f;

	private float blendExposure = 1f;

	private float effectVolumesBlendAdjust;

	private List<AmplifyColorVolumeBase> enteredVolumes = new List<AmplifyColorVolumeBase>();

	private AmplifyColorTriggerProxyBase actualTriggerProxy;

	[HideInInspector]
	[SerializeField]
	private string sharedInstanceID = string.Empty;

	public Texture2D DefaultLut
	{
		get
		{
			if (defaultLut == null)
			{
				return CreateDefaultLut();
			}
			return defaultLut;
		}
	}

	public bool IsBlending => blending;

	private float effectVolumesBlendAdjusted => Mathf.Clamp01((effectVolumesBlendAdjust >= 0.99f) ? 1f : ((volumesBlendAmount - effectVolumesBlendAdjust) / (1f - effectVolumesBlendAdjust)));

	public string SharedInstanceID => sharedInstanceID;

	public bool WillItBlend
	{
		get
		{
			if (LutTexture != null && LutBlendTexture != null)
			{
				return !blending;
			}
			return false;
		}
	}

	public void NewSharedInstanceID()
	{
		sharedInstanceID = Guid.NewGuid().ToString();
	}

	private void ReportMissingShaders()
	{
		Debug.LogError("[AmplifyColor] Failed to initialize shaders. Please attempt to re-enable the Amplify Color Effect component. If that fails, please reinstall Amplify Color.");
		base.enabled = false;
	}

	private void ReportNotSupported()
	{
		Debug.LogWarning("[AmplifyColor] This image effect is not supported on this platform.");
		base.enabled = false;
	}

	private bool CheckShader(Shader s)
	{
		if (s == null)
		{
			ReportMissingShaders();
			return false;
		}
		if (!s.isSupported)
		{
			ReportNotSupported();
			return false;
		}
		return true;
	}

	private bool CheckShaders()
	{
		if (CheckShader(shaderBase) && CheckShader(shaderBlend) && CheckShader(shaderBlendCache) && CheckShader(shaderMask) && CheckShader(shaderMaskBlend))
		{
			return CheckShader(shaderProcessOnly);
		}
		return false;
	}

	private bool CheckSupport()
	{
		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
		{
			ReportNotSupported();
			return false;
		}
		return true;
	}

	private void OnEnable()
	{
		if (CheckSupport() && CreateMaterials())
		{
			Texture2D texture2D = LutTexture as Texture2D;
			Texture2D texture2D2 = LutBlendTexture as Texture2D;
			if ((texture2D != null && texture2D.mipmapCount > 1) || (texture2D2 != null && texture2D2.mipmapCount > 1))
			{
				Debug.LogError("[AmplifyColor] Please disable \"Generate Mip Maps\" import settings on all LUT textures to avoid visual glitches. Change Texture Type to \"Advanced\" to access Mip settings.");
			}
		}
	}

	private void OnDisable()
	{
		if (actualTriggerProxy != null)
		{
			UnityEngine.Object.DestroyImmediate(actualTriggerProxy.gameObject);
			actualTriggerProxy = null;
		}
		ReleaseMaterials();
		ReleaseTextures();
	}

	private void VolumesBlendTo(Texture blendTargetLUT, float blendTimeInSec)
	{
		volumesLutBlendTexture = blendTargetLUT;
		volumesBlendAmount = 0f;
		volumesBlendingTime = blendTimeInSec;
		volumesBlendingTimeCountdown = blendTimeInSec;
		volumesBlending = true;
	}

	public void BlendTo(Texture blendTargetLUT, float blendTimeInSec, Action onFinishBlend)
	{
		LutBlendTexture = blendTargetLUT;
		BlendAmount = 0f;
		this.onFinishBlend = onFinishBlend;
		blendingTime = blendTimeInSec;
		blendingTimeCountdown = blendTimeInSec;
		blending = true;
	}

	private void CheckCamera()
	{
		if (ownerCamera == null)
		{
			ownerCamera = GetComponent<Camera>();
		}
		if (UseDepthMask && (ownerCamera.depthTextureMode & DepthTextureMode.Depth) == 0)
		{
			ownerCamera.depthTextureMode |= DepthTextureMode.Depth;
		}
	}

	private void Start()
	{
		CheckCamera();
		worldLUT = LutTexture;
		worldExposure = Exposure;
		blendExposure = (currentExposure = worldExposure);
	}

	private void Update()
	{
		CheckCamera();
		bool flag = false;
		if (volumesBlending)
		{
			volumesBlendAmount = (volumesBlendingTime - volumesBlendingTimeCountdown) / volumesBlendingTime;
			volumesBlendingTimeCountdown -= Time.smoothDeltaTime;
			if (volumesBlendAmount >= 1f)
			{
				volumesBlendAmount = 1f;
				flag = true;
			}
		}
		else
		{
			volumesBlendAmount = Mathf.Clamp01(volumesBlendAmount);
		}
		if (blending)
		{
			BlendAmount = (blendingTime - blendingTimeCountdown) / blendingTime;
			blendingTimeCountdown -= Time.smoothDeltaTime;
			if (BlendAmount >= 1f)
			{
				LutTexture = LutBlendTexture;
				BlendAmount = 0f;
				blending = false;
				LutBlendTexture = null;
				if (onFinishBlend != null)
				{
					onFinishBlend();
				}
			}
		}
		else
		{
			BlendAmount = Mathf.Clamp01(BlendAmount);
		}
		if (UseVolumes)
		{
			if (actualTriggerProxy == null)
			{
				GameObject gameObject = new GameObject(base.name + "+ACVolumeProxy")
				{
					hideFlags = HideFlags.HideAndDontSave
				};
				if (TriggerVolumeProxy != null && TriggerVolumeProxy.GetComponent<Collider2D>() != null)
				{
					actualTriggerProxy = gameObject.AddComponent<AmplifyColorTriggerProxy2D>();
				}
				else
				{
					actualTriggerProxy = gameObject.AddComponent<AmplifyColorTriggerProxy>();
				}
				actualTriggerProxy.OwnerEffect = this;
			}
			UpdateVolumes();
		}
		else if (actualTriggerProxy != null)
		{
			UnityEngine.Object.DestroyImmediate(actualTriggerProxy.gameObject);
			actualTriggerProxy = null;
		}
		if (flag)
		{
			LutTexture = volumesLutBlendTexture;
			volumesBlendAmount = 0f;
			volumesBlending = false;
			volumesLutBlendTexture = null;
			effectVolumesBlendAdjust = 0f;
			currentExposure = blendExposure;
			if (blendingFromMidBlend && midBlendLUT != null)
			{
				midBlendLUT.DiscardContents();
			}
			blendingFromMidBlend = false;
		}
	}

	public void EnterVolume(AmplifyColorVolumeBase volume)
	{
		if (!enteredVolumes.Contains(volume))
		{
			enteredVolumes.Insert(0, volume);
		}
	}

	public void ExitVolume(AmplifyColorVolumeBase volume)
	{
		if (enteredVolumes.Contains(volume))
		{
			enteredVolumes.Remove(volume);
		}
	}

	private void UpdateVolumes()
	{
		_ = volumesBlending;
		if (volumesBlending)
		{
			Exposure = Mathf.Lerp(currentExposure, blendExposure, effectVolumesBlendAdjusted);
		}
		Transform transform = ((!(TriggerVolumeProxy == null)) ? TriggerVolumeProxy : base.transform);
		if (actualTriggerProxy.transform.parent != transform)
		{
			actualTriggerProxy.Reference = transform;
			actualTriggerProxy.gameObject.layer = transform.gameObject.layer;
		}
		AmplifyColorVolumeBase amplifyColorVolumeBase = null;
		int num = int.MinValue;
		for (int i = 0; i < enteredVolumes.Count; i++)
		{
			AmplifyColorVolumeBase amplifyColorVolumeBase2 = enteredVolumes[i];
			if (amplifyColorVolumeBase2.Priority > num)
			{
				amplifyColorVolumeBase = amplifyColorVolumeBase2;
				num = amplifyColorVolumeBase2.Priority;
			}
		}
		if (!(amplifyColorVolumeBase != currentVolumeLut))
		{
			return;
		}
		currentVolumeLut = amplifyColorVolumeBase;
		Texture texture = ((!(amplifyColorVolumeBase == null)) ? amplifyColorVolumeBase.LutTexture : worldLUT);
		float num2 = ((!(amplifyColorVolumeBase == null)) ? amplifyColorVolumeBase.EnterBlendTime : ExitVolumeBlendTime);
		if (volumesBlending && !blendingFromMidBlend && texture == LutTexture)
		{
			LutTexture = volumesLutBlendTexture;
			volumesLutBlendTexture = texture;
			volumesBlendingTimeCountdown = num2 * ((volumesBlendingTime - volumesBlendingTimeCountdown) / volumesBlendingTime);
			volumesBlendingTime = num2;
			currentExposure = Mathf.Lerp(currentExposure, blendExposure, effectVolumesBlendAdjusted);
			effectVolumesBlendAdjust = 1f - volumesBlendAmount;
			volumesBlendAmount = 1f - volumesBlendAmount;
		}
		else
		{
			if (volumesBlending)
			{
				materialBlendCache.SetFloat("_LerpAmount", volumesBlendAmount);
				if (blendingFromMidBlend)
				{
					Graphics.Blit(midBlendLUT, blendCacheLut);
					materialBlendCache.SetTexture("_RgbTex", blendCacheLut);
				}
				else
				{
					materialBlendCache.SetTexture("_RgbTex", LutTexture);
				}
				materialBlendCache.SetTexture("_LerpRgbTex", (!(volumesLutBlendTexture != null)) ? defaultLut : volumesLutBlendTexture);
				Graphics.Blit(midBlendLUT, midBlendLUT, materialBlendCache);
				blendCacheLut.DiscardContents();
				currentExposure = Mathf.Lerp(currentExposure, blendExposure, effectVolumesBlendAdjusted);
				effectVolumesBlendAdjust = 0f;
				blendingFromMidBlend = true;
			}
			VolumesBlendTo(texture, num2);
		}
		blendExposure = ((!(amplifyColorVolumeBase == null)) ? amplifyColorVolumeBase.Exposure : worldExposure);
	}

	private void SetupShader()
	{
		colorSpace = QualitySettings.activeColorSpace;
		qualityLevel = QualityLevel;
		shaderBase = Shader.Find("Hidden/Amplify Color/Base");
		shaderBlend = Shader.Find("Hidden/Amplify Color/Blend");
		shaderBlendCache = Shader.Find("Hidden/Amplify Color/BlendCache");
		shaderMask = Shader.Find("Hidden/Amplify Color/Mask");
		shaderMaskBlend = Shader.Find("Hidden/Amplify Color/MaskBlend");
		shaderDepthMask = Shader.Find("Hidden/Amplify Color/DepthMask");
		shaderDepthMaskBlend = Shader.Find("Hidden/Amplify Color/DepthMaskBlend");
		shaderProcessOnly = Shader.Find("Hidden/Amplify Color/ProcessOnly");
	}

	private void ReleaseMaterials()
	{
		SafeRelease(ref materialBase);
		SafeRelease(ref materialBlend);
		SafeRelease(ref materialBlendCache);
		SafeRelease(ref materialMask);
		SafeRelease(ref materialMaskBlend);
		SafeRelease(ref materialDepthMask);
		SafeRelease(ref materialDepthMaskBlend);
		SafeRelease(ref materialProcessOnly);
	}

	private Texture2D CreateDefaultLut()
	{
		defaultLut = new Texture2D(1024, 32, TextureFormat.RGB24, mipChain: false, linear: true)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		defaultLut.name = "DefaultLut";
		defaultLut.hideFlags = HideFlags.DontSave;
		defaultLut.anisoLevel = 1;
		defaultLut.filterMode = FilterMode.Bilinear;
		Color32[] array = new Color32[32768];
		for (int i = 0; i < 32; i++)
		{
			int num = i * 32;
			for (int j = 0; j < 32; j++)
			{
				int num2 = num + j * 1024;
				for (int k = 0; k < 32; k++)
				{
					float num3 = (float)k / 31f;
					float num4 = (float)j / 31f;
					float num5 = (float)i / 31f;
					byte r = (byte)(num3 * 255f);
					byte g = (byte)(num4 * 255f);
					byte b = (byte)(num5 * 255f);
					array[num2 + k] = new Color32(r, g, b, byte.MaxValue);
				}
			}
		}
		defaultLut.SetPixels32(array);
		defaultLut.Apply();
		return defaultLut;
	}

	private Texture2D CreateDepthCurveLut()
	{
		SafeRelease(ref depthCurveLut);
		depthCurveLut = new Texture2D(1024, 1, TextureFormat.Alpha8, mipChain: false, linear: true)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		depthCurveLut.name = "DepthCurveLut";
		depthCurveLut.hideFlags = HideFlags.DontSave;
		depthCurveLut.anisoLevel = 1;
		depthCurveLut.wrapMode = TextureWrapMode.Clamp;
		depthCurveLut.filterMode = FilterMode.Bilinear;
		depthCurveColors = new Color32[1024];
		return depthCurveLut;
	}

	private void UpdateDepthCurveLut()
	{
		if (depthCurveLut == null)
		{
			CreateDepthCurveLut();
		}
		float num = 0f;
		int num2 = 0;
		while (num2 < 1024)
		{
			depthCurveColors[num2].a = (byte)Mathf.FloorToInt(Mathf.Clamp01(DepthMaskCurve.Evaluate(num)) * 255f);
			num2++;
			num += 0.0009775171f;
		}
		depthCurveLut.SetPixels32(depthCurveColors);
		depthCurveLut.Apply();
	}

	private void CheckUpdateDepthCurveLut()
	{
		bool flag = false;
		if (DepthMaskCurve.length != prevDepthMaskCurve.length)
		{
			flag = true;
		}
		else
		{
			float num = 0f;
			int num2 = 0;
			while (num2 < DepthMaskCurve.length)
			{
				if (Mathf.Abs(DepthMaskCurve.Evaluate(num) - prevDepthMaskCurve.Evaluate(num)) > float.Epsilon)
				{
					flag = true;
					break;
				}
				num2++;
				num += 0.0009775171f;
			}
		}
		if (depthCurveLut == null || flag)
		{
			UpdateDepthCurveLut();
			prevDepthMaskCurve = new AnimationCurve(DepthMaskCurve.keys);
		}
	}

	private void CreateHelperTextures()
	{
		ReleaseTextures();
		blendCacheLut = new RenderTexture(1024, 32, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		blendCacheLut.name = "BlendCacheLut";
		blendCacheLut.wrapMode = TextureWrapMode.Clamp;
		blendCacheLut.useMipMap = false;
		blendCacheLut.anisoLevel = 0;
		blendCacheLut.Create();
		midBlendLUT = new RenderTexture(1024, 32, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear)
		{
			hideFlags = HideFlags.HideAndDontSave
		};
		midBlendLUT.name = "MidBlendLut";
		midBlendLUT.wrapMode = TextureWrapMode.Clamp;
		midBlendLUT.useMipMap = false;
		midBlendLUT.anisoLevel = 0;
		midBlendLUT.Create();
		CreateDefaultLut();
		if (UseDepthMask)
		{
			CreateDepthCurveLut();
		}
	}

	private bool CheckMaterialAndShader(Material material, string name)
	{
		if (material == null || material.shader == null)
		{
			Debug.LogWarning("[AmplifyColor] Error creating " + name + " material. Effect disabled.");
			base.enabled = false;
		}
		else if (!material.shader.isSupported)
		{
			Debug.LogWarning("[AmplifyColor] " + name + " shader not supported on this platform. Effect disabled.");
			base.enabled = false;
		}
		else
		{
			material.hideFlags = HideFlags.HideAndDontSave;
		}
		return base.enabled;
	}

	private bool CreateMaterials()
	{
		SetupShader();
		if (!CheckShaders())
		{
			return false;
		}
		ReleaseMaterials();
		materialBase = new Material(shaderBase);
		materialBlend = new Material(shaderBlend);
		materialBlendCache = new Material(shaderBlendCache);
		materialMask = new Material(shaderMask);
		materialMaskBlend = new Material(shaderMaskBlend);
		materialDepthMask = new Material(shaderDepthMask);
		materialDepthMaskBlend = new Material(shaderDepthMaskBlend);
		materialProcessOnly = new Material(shaderProcessOnly);
		if (1 == 0 || !CheckMaterialAndShader(materialBase, "BaseMaterial") || !CheckMaterialAndShader(materialBlend, "BlendMaterial") || !CheckMaterialAndShader(materialBlendCache, "BlendCacheMaterial") || !CheckMaterialAndShader(materialMask, "MaskMaterial") || !CheckMaterialAndShader(materialMaskBlend, "MaskBlendMaterial") || !CheckMaterialAndShader(materialDepthMask, "DepthMaskMaterial") || !CheckMaterialAndShader(materialDepthMaskBlend, "DepthMaskBlendMaterial") || !CheckMaterialAndShader(materialProcessOnly, "ProcessOnlyMaterial"))
		{
			return false;
		}
		CreateHelperTextures();
		return true;
	}

	private void SetMaterialKeyword(string keyword, bool state)
	{
		bool flag = materialBase.IsKeywordEnabled(keyword);
		if (state && !flag)
		{
			materialBase.EnableKeyword(keyword);
			materialBlend.EnableKeyword(keyword);
			materialBlendCache.EnableKeyword(keyword);
			materialMask.EnableKeyword(keyword);
			materialMaskBlend.EnableKeyword(keyword);
			materialDepthMask.EnableKeyword(keyword);
			materialDepthMaskBlend.EnableKeyword(keyword);
			materialProcessOnly.EnableKeyword(keyword);
		}
		else if (!state && materialBase.IsKeywordEnabled(keyword))
		{
			materialBase.DisableKeyword(keyword);
			materialBlend.DisableKeyword(keyword);
			materialBlendCache.DisableKeyword(keyword);
			materialMask.DisableKeyword(keyword);
			materialMaskBlend.DisableKeyword(keyword);
			materialDepthMask.DisableKeyword(keyword);
			materialDepthMaskBlend.DisableKeyword(keyword);
			materialProcessOnly.DisableKeyword(keyword);
		}
	}

	private void UpdateShaderKeywords()
	{
		SetMaterialKeyword("AC_QUALITY_MOBILE", QualityLevel == Quality.Mobile);
		SetMaterialKeyword("AC_DITHERING", UseDithering);
		SetMaterialKeyword("AC_TONEMAPPING", UseToneMapping);
	}

	private void SafeRelease<T>(ref T obj) where T : UnityEngine.Object
	{
		if ((UnityEngine.Object)obj != (UnityEngine.Object)null)
		{
			UnityEngine.Object.DestroyImmediate(obj);
			obj = null;
		}
	}

	private void ReleaseTextures()
	{
		SafeRelease(ref blendCacheLut);
		SafeRelease(ref midBlendLUT);
		SafeRelease(ref defaultLut);
		SafeRelease(ref depthCurveLut);
	}

	public static bool ValidateLutDimensions(Texture lut)
	{
		bool result = true;
		if (lut != null)
		{
			if (lut.width / lut.height != lut.height)
			{
				Debug.LogWarning("[AmplifyColor] Lut " + lut.name + " has invalid dimensions.");
				result = false;
			}
			else if (lut.anisoLevel != 0)
			{
				lut.anisoLevel = 0;
			}
		}
		return result;
	}

	private void UpdatePostEffectParams()
	{
		if (UseDepthMask)
		{
			CheckUpdateDepthCurveLut();
		}
		Exposure = Mathf.Max(Exposure, 0f);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		BlendAmount = Mathf.Clamp01(BlendAmount);
		if (colorSpace != QualitySettings.activeColorSpace || qualityLevel != QualityLevel)
		{
			CreateMaterials();
		}
		UpdatePostEffectParams();
		UpdateShaderKeywords();
		bool num = ValidateLutDimensions(LutTexture);
		bool flag = ValidateLutDimensions(LutBlendTexture);
		bool flag2 = LutTexture == null && LutBlendTexture == null && volumesLutBlendTexture == null;
		Texture texture = ((!(LutTexture == null)) ? LutTexture : defaultLut);
		Texture lutBlendTexture = LutBlendTexture;
		int pass = ((colorSpace == ColorSpace.Linear) ? 2 : 0) + (ownerCamera.allowHDR ? 1 : 0);
		bool flag3 = BlendAmount != 0f || blending;
		bool flag4 = flag3 || (flag3 && lutBlendTexture != null);
		bool flag5 = flag4;
		bool num2 = !num || !flag || flag2;
		Material material = (num2 ? materialProcessOnly : ((flag4 || volumesBlending) ? ((!UseDepthMask) ? ((!(MaskTexture != null)) ? materialBlend : materialMaskBlend) : materialDepthMaskBlend) : ((!UseDepthMask) ? ((!(MaskTexture != null)) ? materialBase : materialMask) : materialDepthMask)));
		material.SetFloat("_Exposure", Exposure);
		material.SetFloat("_LerpAmount", BlendAmount);
		if (MaskTexture != null)
		{
			material.SetTexture("_MaskTex", MaskTexture);
		}
		if (UseDepthMask)
		{
			material.SetTexture("_DepthCurveLut", depthCurveLut);
		}
		if (!num2)
		{
			if (volumesBlending)
			{
				volumesBlendAmount = Mathf.Clamp01(volumesBlendAmount);
				materialBlendCache.SetFloat("_LerpAmount", volumesBlendAmount);
				if (blendingFromMidBlend)
				{
					materialBlendCache.SetTexture("_RgbTex", midBlendLUT);
				}
				else
				{
					materialBlendCache.SetTexture("_RgbTex", texture);
				}
				materialBlendCache.SetTexture("_LerpRgbTex", (!(volumesLutBlendTexture != null)) ? defaultLut : volumesLutBlendTexture);
				Graphics.Blit(texture, blendCacheLut, materialBlendCache);
			}
			if (flag5)
			{
				materialBlendCache.SetFloat("_LerpAmount", BlendAmount);
				RenderTexture renderTexture = null;
				if (volumesBlending)
				{
					renderTexture = RenderTexture.GetTemporary(blendCacheLut.width, blendCacheLut.height, blendCacheLut.depth, blendCacheLut.format, RenderTextureReadWrite.Linear);
					Graphics.Blit(blendCacheLut, renderTexture);
					materialBlendCache.SetTexture("_RgbTex", renderTexture);
				}
				else
				{
					materialBlendCache.SetTexture("_RgbTex", texture);
				}
				materialBlendCache.SetTexture("_LerpRgbTex", (!(lutBlendTexture != null)) ? defaultLut : lutBlendTexture);
				Graphics.Blit(texture, blendCacheLut, materialBlendCache);
				if (renderTexture != null)
				{
					RenderTexture.ReleaseTemporary(renderTexture);
				}
				material.SetTexture("_RgbBlendCacheTex", blendCacheLut);
			}
			else if (volumesBlending)
			{
				material.SetTexture("_RgbBlendCacheTex", blendCacheLut);
			}
			else
			{
				if (texture != null)
				{
					material.SetTexture("_RgbTex", texture);
				}
				if (lutBlendTexture != null)
				{
					material.SetTexture("_LerpRgbTex", lutBlendTexture);
				}
			}
		}
		Graphics.Blit(source, destination, material, pass);
		if (flag5 || volumesBlending)
		{
			blendCacheLut.DiscardContents();
		}
	}
}
