using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnluckSoftware;

// Token: 0x02000568 RID: 1384
[ExecuteInEditMode]
public class ZippyLights2D : MonoBehaviour
{
	// Token: 0x06001ED6 RID: 7894 RVA: 0x00099F61 File Offset: 0x00098161
	private void Awake()
	{
		this.Init();
		this.Noise();
		this.EmitParticles();
		this.cacheTransform = base.transform;
		if (this.staticLight)
		{
			base.enabled = false;
		}
	}

	// Token: 0x06001ED7 RID: 7895 RVA: 0x00099F90 File Offset: 0x00098190
	public void ForceNewMesh()
	{
		UnityEngine.Object.DestroyImmediate(this.lightMesh);
		UnityEngine.Object.DestroyImmediate(this.outlineMesh);
		this.lightMesh = null;
		this.outlineMesh = null;
		this.cacheTransform = base.transform;
		this.ForceUpdate();
	}

	// Token: 0x06001ED8 RID: 7896 RVA: 0x00099FC8 File Offset: 0x000981C8
	private void OnEnable()
	{
		if (this.particles)
		{
			this.particles.Clear();
		}
	}

	// Token: 0x06001ED9 RID: 7897 RVA: 0x00099FE4 File Offset: 0x000981E4
	private void Noise()
	{
		base.Invoke("Noise", this.noiseDelay);
		if (!this.lightEnabled || this.noise <= 0f)
		{
			return;
		}
		this.noiseVal = 0f;
		this.noiseVal = UnityEngine.Random.Range(-this.noise, this.noise);
	}

	// Token: 0x06001EDA RID: 7898 RVA: 0x0009A03C File Offset: 0x0009823C
	private void EmitParticles()
	{
		base.Invoke("EmitParticles", this.particleEmitDelay);
		if (!this.lightEnabled || !this.particles || this.pointsP == null)
		{
			return;
		}
		for (int i = 0; i < this.pointsPlenght; i++)
		{
			this.cacheParticleTransform.position = this.pointsP[i];
			this.particles.Emit(this.particleEmitAmount);
		}
	}

	// Token: 0x06001EDB RID: 7899 RVA: 0x0009A0B4 File Offset: 0x000982B4
	private void FindOrCreateLightObject()
	{
		if (this.meshTransform == null)
		{
			this.meshTransform = this.cacheTransform.Find("LightMesh");
		}
		if (this.meshTransform == null)
		{
			GameObject gameObject = new GameObject("LightMesh", new Type[]
			{
				typeof(MeshRenderer),
				typeof(MeshFilter)
			});
			this.meshTransform = gameObject.transform;
			this.meshTransform.parent = this.cacheTransform;
			this.meshTransform.localPosition = Vector3.zero;
			this.meshTransform.localRotation = Quaternion.identity;
		}
		this.cacheMeshFilter = this.meshTransform.GetComponent<MeshFilter>();
		this.cacheMeshRenderer = this.meshTransform.GetComponent<MeshRenderer>();
		this.cacheMeshRenderer.receiveShadows = false;
		this.cacheMeshRenderer.castShadows = false;
		this.meshTransform.gameObject.hideFlags = HideFlags.None;
		this.cacheMeshRenderer.sortingOrder = this.sortingOrder;
		this.cacheMeshRenderer.sortingLayerID = this.sortingLayer;
		if (this.duplicatedLights != null)
		{
			for (int i = 0; i < this.duplicatedLights.Count; i++)
			{
				MeshRenderer component = this.duplicatedLights[i].GetComponent<MeshRenderer>();
				component.sortingOrder = this.sortingOrder;
				component.sortingLayerID = this.sortingLayer;
			}
		}
		MeshRenderer component2 = this.cacheTransform.GetComponent<MeshRenderer>();
		if (component2)
		{
			this.cacheMeshRenderer.sharedMaterial = component2.sharedMaterial;
			UnityEngine.Object.Destroy(component2);
		}
		MeshFilter component3 = this.cacheTransform.GetComponent<MeshFilter>();
		if (component3)
		{
			this.cacheMeshFilter.sharedMesh = component3.sharedMesh;
			UnityEngine.Object.Destroy(component3);
		}
	}

	// Token: 0x06001EDC RID: 7900 RVA: 0x0009A264 File Offset: 0x00098464
	public void Init()
	{
		if (this.lightMesh == null)
		{
			this.lightMesh = new Mesh();
		}
		if (this.outlineMesh == null)
		{
			this.outlineMesh = new Mesh();
		}
		this.FindOrCreateLightObject();
		if (!this.cacheTransform)
		{
			this.cacheTransform = base.transform;
		}
		if (this.particles && !this.cacheParticleTransform)
		{
			this.cacheParticleTransform = this.particles.transform;
		}
		if (this.points == null)
		{
			this.points = new Vector3[this.resolution];
		}
		this.pointsF = new Vector3[this.resolution];
		this.degreeResolution = this.degrees / (float)this.resolution;
		this.Brighten(true);
	}

	// Token: 0x06001EDD RID: 7901 RVA: 0x0009A332 File Offset: 0x00098532
	private void LateUpdate()
	{
		if (this.oldDegrees == this.degrees)
		{
			this.Brighten(false);
			return;
		}
		this.ForceUpdate();
		this.oldDegrees = this.degrees;
	}

	// Token: 0x06001EDE RID: 7902 RVA: 0x0009A35C File Offset: 0x0009855C
	public void Brighten(bool forceBrighten = false)
	{
		if (this.follow && Application.isPlaying)
		{
			this.cacheTransform.position = new Vector3(this.follow.position.x, this.follow.position.y, 0f);
			if (this.degrees < 360f)
			{
				this.cacheTransform.eulerAngles = new Vector3(0f, 0f, this.follow.eulerAngles.z - this.degrees * 0.5f);
			}
		}
		if (!this.cacheMeshRenderer.isVisible)
		{
			this.lightEnabled = false;
			return;
		}
		if (!this.lightEnabled)
		{
			this.lightEnabled = true;
		}
		this.lightTime = Time.time;
		if (Application.isPlaying && this.moveToUpdate && this.savePos == this.cacheTransform.position && this.saveRot == this.cacheTransform.rotation)
		{
			return;
		}
		this.ScanPoints();
		this.MeshGen();
		this.MeshGenOutline();
		this.savePos = this.cacheTransform.position;
		this.saveRot = this.cacheTransform.rotation;
		if (!this.lightEnabled)
		{
			return;
		}
		if (this.ColorCycleEnabled)
		{
			Color color = this.ColorCycle.Evaluate(this.lightTime * this.ColorCycleSpeed % 1f);
			this.vertexColor = color;
			if (this.unityLight)
			{
				this.unityLight.color = color;
			}
		}
		if (this.ColorCycleOuterEnabled)
		{
			Color color2 = this.ColorCycleOuter.Evaluate(this.lightTime * this.ColorCycleSpeedOuter % 1f);
			this.vertexColorOuter = color2;
			if (this.unityLight && !this.ColorCycleEnabled)
			{
				this.unityLight.color = color2;
			}
		}
		if (this.animateRange)
		{
			this.range = this.rangeAnimation.Evaluate(this.lightTime * this.animateRangeSpeed % 1f) * this.animateRangeScale;
		}
	}

	// Token: 0x06001EDF RID: 7903 RVA: 0x0009A570 File Offset: 0x00098770
	private void ScanPoints()
	{
		this.degreeResolution = this.degrees / (float)this.resolution;
		if (this.points == null || this.points.Length != this.resolution)
		{
			this.points = new Vector3[this.resolution];
		}
		if (this.pointsP == null || this.pointsP.Length != this.resolution)
		{
			this.pointsP = new Vector3[this.resolution];
		}
		if (this.str == null || this.str.Length != this.resolution)
		{
			this.str = new float[this.resolution];
		}
		this.pointsPlenght = 0;
		Vector3 up = this.cacheTransform.up;
		int num = (int)((float)this.resolution * this.particleRayAmount);
		Vector3 forward = Vector3.forward;
		for (int i = 0; i < this.resolution; i++)
		{
			Vector3 vector = Quaternion.AngleAxis((float)i * this.degreeResolution + this.noiseVal, forward) * up;
			RaycastHit2D hit = Physics2D.Raycast(this.cacheTransform.position, vector, this.range, this.layers);
			float distance = hit.distance;
			if (hit && (this.tags.Length == 0 | this.tags.Contains(hit.collider.tag)))
			{
				if (this.particles && distance < this.particleRangeLimitMax && distance > this.particleRangeLimitMin && i % num == UnityEngine.Random.Range(0, num + 1))
				{
					this.pointsP[this.pointsPlenght] = hit.point;
					this.pointsPlenght++;
				}
				this.points[i] = hit.point;
				this.str[i] = 1f - distance / this.range;
				if (this.offset != 0f)
				{
					Vector2 vector2;
					if (this.offsetSpherify == 0f)
					{
						vector2 = Vector2.MoveTowards(this.points[i], base.transform.position, -this.offset);
						if (this.enableVertexColors)
						{
							float num2 = Vector2.Distance(this.points[i], vector2);
							this.str[i] = 1f - (distance + num2) / this.range;
						}
					}
					else
					{
						vector2 = Vector2.MoveTowards(this.points[i], base.transform.position, -this.offset * (1f + this.str[i] * this.offsetSpherify));
						float num3 = Vector2.Distance(this.points[i], vector2);
						this.str[i] = 1f - (distance + num3) / this.range;
					}
					this.points[i] = vector2;
				}
			}
			else
			{
				this.points[i] = this.cacheTransform.position + vector * this.range;
				this.str[i] = 0f;
			}
			if (i < this.pointsF.Length)
			{
				this.pointsF[i] = Vector2.MoveTowards(this.points[i], base.transform.position, -this.falloff * (1f + this.str[i] * this.falloffAfterglow));
			}
		}
	}

	// Token: 0x06001EE0 RID: 7904 RVA: 0x0009A910 File Offset: 0x00098B10
	public void ResizeArray(int size, ref Vector3[] arr)
	{
		Vector3[] array = new Vector3[size];
		for (int i = 0; i < size; i++)
		{
			array[i] = arr[i];
		}
		arr = array;
	}

	// Token: 0x06001EE1 RID: 7905 RVA: 0x0009A944 File Offset: 0x00098B44
	private bool ComparePoints()
	{
		if (this.points.Length != this.pointsX.Length)
		{
			return true;
		}
		if (this.pointsX.Length == 0)
		{
			return true;
		}
		for (int i = 0; i < this.points.Length; i++)
		{
			if (this.points[i] != this.pointsX[i])
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06001EE2 RID: 7906 RVA: 0x0009A9A8 File Offset: 0x00098BA8
	private void MeshGenOutline()
	{
		if (this.falloff == 0f || this.idle)
		{
			return;
		}
		if (this.currentResolution != this.resolution)
		{
			this.vertsF = null;
			this.trisF = null;
			this.colorsX = null;
			this.uF = null;
		}
		int num = this.pointsF.Length * 2;
		if (this.vertsF == null || this.vertsF.Length != num)
		{
			this.vertsF = new Vector3[num];
			this.trisF = new int[num * 6 + 3];
			this.colorsX = new Color[num];
		}
		int num2 = 0;
		for (int i = 0; i < this.pointsF.Length; i++)
		{
			this.vertsF[num2] = this.cacheTransform.InverseTransformPoint(this.pointsF[i]);
			if (this.colors != null)
			{
				this.colorsX[num2] = this.colors[i + 1];
				this.colorsX[num2] = Color.black;
				this.colorsX[num2].a = this.colors[i + 1].a;
				this.colorsX[num2 + 1] = this.colors[i + 1];
				this.colorsX[num2 + 1].a = this.colors[i + 1].a;
			}
			num2++;
			this.vertsF[num2] = this.cacheTransform.InverseTransformPoint(this.points[i]);
			num2++;
		}
		if (this.resolution != this.verts.Length)
		{
			this.outlineMesh.Clear();
			int num3 = 0;
			if (this.degrees < 360f)
			{
				num3 = -2;
			}
			int num4 = 0;
			if (this.falloffMobileFix)
			{
				for (int j = 1; j < this.vertsF.Length + num3; j++)
				{
					this.trisF[num4] = j % this.vertsF.Length;
					num4++;
					this.trisF[num4] = (j - 1) % this.vertsF.Length;
					num4++;
					this.trisF[num4] = (j + 1) % this.vertsF.Length;
					num4++;
				}
			}
			else
			{
				for (int k = 1; k < this.vertsF.Length + num3; k++)
				{
					this.trisF[num4] = k % this.vertsF.Length;
					num4++;
					this.trisF[num4] = (k - 1) % this.vertsF.Length;
					num4++;
					this.trisF[num4] = (k + 1) % this.vertsF.Length;
					num4++;
					this.trisF[num4] = (k + 1) % this.vertsF.Length;
					num4++;
					this.trisF[num4] = (k - 1) % this.vertsF.Length;
					num4++;
					this.trisF[num4] = k % this.vertsF.Length;
					num4++;
				}
			}
			if (this.degrees < 360f)
			{
				this.trisF[num4] = this.vertsF.Length - 1;
				num4++;
				this.trisF[num4] = this.vertsF.Length - 2;
				num4++;
				this.trisF[num4] = this.vertsF.Length - 3;
			}
			else
			{
				this.trisF[num4] = 0;
				num4++;
				this.trisF[num4] = this.vertsF.Length - 1;
				num4++;
				this.trisF[num4] = 1;
			}
			this.outlineMesh.vertices = this.vertsF;
			this.outlineMesh.triangles = this.trisF;
		}
		else
		{
			this.outlineMesh.vertices = this.verts;
		}
		this.outlineMesh.colors = this.colorsX;
		if (this.CreateUV)
		{
			if (this.uF == null || this.uF.Length != this.vertsF.Length)
			{
				this.uF = new Vector2[this.vertsF.Length];
			}
			float num5 = this.range * this.UVScale;
			for (int l = 0; l < this.vertsF.Length; l++)
			{
				float num6 = num5 * 0.5f;
				this.uF[l] = new Vector2(this.vertsF[l].x / num6 + 0.5f, this.vertsF[l].y / num6 + 0.5f);
			}
			this.outlineMesh.uv = this.uF;
		}
		else
		{
			this.outlineMesh.uv = null;
		}
		CombineInstance[] array = new CombineInstance[2];
		array[0].mesh = this.lightMesh;
		array[0].transform = this.cacheTransform.localToWorldMatrix.transpose;
		array[1].mesh = this.outlineMesh;
		array[1].transform = this.cacheTransform.localToWorldMatrix.transpose;
		this.combinedLightMeshes = new Mesh();
		this.combinedLightMeshes.CombineMeshes(array);
		this.cacheMeshFilter.mesh = this.combinedLightMeshes;
		if (this.duplicatedLights != null)
		{
			for (int m = 0; m < this.duplicatedLights.Count; m++)
			{
				this.duplicatedLights[m].mesh = this.combinedLightMeshes;
			}
		}
		Vector3 eulerAngles = this.cacheTransform.rotation.eulerAngles;
		eulerAngles.z = this.cacheTransform.eulerAngles.z + this.cacheTransform.eulerAngles.z;
		this.meshTransform.eulerAngles = eulerAngles;
	}

	// Token: 0x06001EE3 RID: 7907 RVA: 0x0009AF70 File Offset: 0x00099170
	private void MeshGen()
	{
		if (this.currentResolution != this.resolution)
		{
			this.currentResolution = this.resolution;
			this.verts = null;
			this.tris = null;
			this.colors = null;
			this.u = null;
		}
		if (Application.isPlaying && this.pointsX != null && !this.ComparePoints())
		{
			this.idle = true;
			return;
		}
		this.idle = false;
		int num = this.points.Length + 2;
		if (this.verts == null)
		{
			this.verts = new Vector3[num];
		}
		if (this.tris == null)
		{
			this.tris = new int[num * 3 + 3];
		}
		if (this.colors == null)
		{
			this.colors = new Color[num];
		}
		if (this.verts.Length != 0)
		{
			this.verts[0] = this.cacheTransform.InverseTransformPoint(this.cacheTransform.position);
		}
		if (this.colors.Length != 0)
		{
			this.colors[0] = this.vertexColor;
		}
		for (int i = 0; i < this.points.Length + 1; i++)
		{
			int num2 = i + 1;
			this.verts[num2] = this.cacheTransform.InverseTransformPoint(this.points[i % this.points.Length]);
			if (this.enableVertexColors)
			{
				float num3 = 1f;
				float num4 = this.str[i % this.points.Length];
				if (this.vertexFade)
				{
					num3 = num4;
				}
				if (this.enableOuterColor || this.ColorCycleOuterEnabled)
				{
					this.colors[num2] = new Color(this.vertexColor.r * num4 + this.vertexColorOuter.r * (1f - num4) / 2f, this.vertexColor.g * num4 + this.vertexColorOuter.g * (1f - num4) / 2f, this.vertexColor.b * num4 + this.vertexColorOuter.b * (1f - num4) / 2f, this.vertexColor.a * num3);
				}
				else
				{
					this.colors[num2] = new Color(this.vertexColor.r, this.vertexColor.g, this.vertexColor.b, this.vertexColor.a * num3);
				}
			}
		}
		if (this.resolution != this.verts.Length)
		{
			this.lightMesh.Clear();
			int num5 = 0;
			if (this.degrees < 360f)
			{
				num5 = -2;
			}
			int num6 = 0;
			for (int j = 0; j < this.verts.Length + num5; j++)
			{
				this.tris[num6] = (j + 1) % this.verts.Length;
				num6++;
				this.tris[num6] = j % this.verts.Length;
				num6++;
				this.tris[num6] = 0;
				num6++;
			}
			this.lightMesh.vertices = this.verts;
			this.lightMesh.triangles = this.tris;
		}
		else
		{
			this.lightMesh.vertices = this.verts;
		}
		if (this.enableVertexColors)
		{
			this.lightMesh.colors = this.colors;
		}
		else
		{
			this.lightMesh.colors = null;
		}
		if (this.CreateUV)
		{
			if (this.u == null)
			{
				this.u = new Vector2[this.verts.Length];
			}
			float num7 = this.range * this.UVScale;
			for (int k = 0; k < this.verts.Length; k++)
			{
				float num8 = num7 * 0.5f;
				this.u[k] = new Vector2(this.verts[k].x / num8 + 0.5f, this.verts[k].y / num8 + 0.5f);
			}
			this.lightMesh.uv = this.u;
		}
		else
		{
			this.lightMesh.uv = null;
		}
		if (this.falloff == 0f)
		{
			this.cacheMeshFilter.mesh = this.lightMesh;
		}
		if (this.pointsX != null && this.pointsX.Length != this.resolution)
		{
			return;
		}
		int num9 = this.points.Length;
		if (this.pointsX == null)
		{
			this.pointsX = new Vector3[num9];
		}
		Array.Copy(this.points, this.pointsX, num9);
		PolygonCollider2D component = base.GetComponent<PolygonCollider2D>();
		if (component && component.enabled)
		{
			component.points = this.AsV2(this.points);
		}
	}

	// Token: 0x06001EE4 RID: 7908 RVA: 0x0009B420 File Offset: 0x00099620
	public Vector2[] AsV2(Vector3[] src)
	{
		Vector2[] array = new Vector2[src.Length];
		for (int i = 0; i < src.Length; i++)
		{
			array[i] = new Vector2(src[i].x - base.transform.position.x, src[i].y - base.transform.position.y);
		}
		return array;
	}

	// Token: 0x06001EE5 RID: 7909 RVA: 0x0009B48B File Offset: 0x0009968B
	public Color CombineColors(Color c1, Color c2)
	{
		return (new Color(0f, 0f, 0f, 0f) + c1 + c2) / 2f;
	}

	// Token: 0x06001EE6 RID: 7910 RVA: 0x0009B4BC File Offset: 0x000996BC
	public void ForceUpdate()
	{
		this.resolution++;
		this.Init();
		this.Brighten(false);
		this.resolution--;
	}

	// Token: 0x040018A8 RID: 6312
	public bool idle;

	// Token: 0x040018A9 RID: 6313
	public bool lightEnabled = true;

	// Token: 0x040018AA RID: 6314
	[Tooltip("Disables light script on start, used for lights that never changes shape.")]
	[SerializeField]
	public bool staticLight;

	// Token: 0x040018AB RID: 6315
	[Tooltip("How many rays and points the light emits.")]
	[Range(10f, 720f)]
	public int resolution = 50;

	// Token: 0x040018AC RID: 6316
	[Tooltip("Light area degrees.")]
	[Range(10f, 360f)]
	public float degrees = 360f;

	// Token: 0x040018AD RID: 6317
	public float oldDegrees = 360f;

	// Token: 0x040018AE RID: 6318
	[Tooltip("Extend the light mesh beyond colliders.")]
	public float offset;

	// Token: 0x040018AF RID: 6319
	[Tooltip("Extend closer colliders more.")]
	public float offsetSpherify;

	// Token: 0x040018B0 RID: 6320
	[Tooltip("Only update the light if it is moving.")]
	public bool moveToUpdate;

	// Token: 0x040018B1 RID: 6321
	[Tooltip("What physics layers light interacts with.")]
	public LayerMask layers = -1;

	// Token: 0x040018B2 RID: 6322
	[Tooltip("What tags light interacts with.")]
	public string[] tags = new string[0];

	// Token: 0x040018B3 RID: 6323
	[Tooltip("Unity light to apply colors to.")]
	public Light unityLight;

	// Token: 0x040018B4 RID: 6324
	[Tooltip("Object to follow.")]
	public Transform follow;

	// Token: 0x040018B5 RID: 6325
	[Tooltip("How far the light travels.")]
	public float range = 15f;

	// Token: 0x040018B6 RID: 6326
	[Tooltip("Enable range animation.")]
	public bool animateRange;

	// Token: 0x040018B7 RID: 6327
	[Tooltip("How to scale range over time.")]
	public AnimationCurve rangeAnimation;

	// Token: 0x040018B8 RID: 6328
	[Tooltip("Animated light range speed.")]
	[Range(0f, 5f)]
	public float animateRangeSpeed = 1f;

	// Token: 0x040018B9 RID: 6329
	[Tooltip("Animated light distance.")]
	public float animateRangeScale = 1f;

	// Token: 0x040018BA RID: 6330
	[Tooltip("Enable vertex colors.")]
	public bool enableVertexColors = true;

	// Token: 0x040018BB RID: 6331
	[Tooltip("Fade edge transparancy.")]
	public bool vertexFade;

	// Token: 0x040018BC RID: 6332
	[Tooltip("Main color of the light.")]
	public Color vertexColor = Color.white;

	// Token: 0x040018BD RID: 6333
	[Tooltip("Enables different color on the outside rim of the light.")]
	public bool enableOuterColor;

	// Token: 0x040018BE RID: 6334
	[Tooltip("Secondary outside color of the light.")]
	public Color vertexColorOuter = Color.white;

	// Token: 0x040018BF RID: 6335
	[Tooltip("Enable inner light color animation.")]
	public bool ColorCycleEnabled;

	// Token: 0x040018C0 RID: 6336
	[Tooltip("Colors to apply over time.")]
	public Gradient ColorCycle;

	// Token: 0x040018C1 RID: 6337
	[Tooltip("How fast to cycle colors over time.")]
	public float ColorCycleSpeed = 1f;

	// Token: 0x040018C2 RID: 6338
	[Tooltip("Enable mesh color animation for edge of mesh.")]
	public bool ColorCycleOuterEnabled;

	// Token: 0x040018C3 RID: 6339
	[Tooltip("Colors to apply over time.")]
	public Gradient ColorCycleOuter;

	// Token: 0x040018C4 RID: 6340
	[Tooltip("How fast to cycle outer colors over time.")]
	public float ColorCycleSpeedOuter = 1f;

	// Token: 0x040018C5 RID: 6341
	[Tooltip("Enable UV generation in mesh.")]
	public bool CreateUV = true;

	// Token: 0x040018C6 RID: 6342
	[Tooltip("Size adjustment of mesh UV.")]
	public float UVScale = 1f;

	// Token: 0x040018C7 RID: 6343
	[Tooltip("Randomize positions of mesh verts.")]
	[Range(0f, 2f)]
	public float noise;

	// Token: 0x040018C8 RID: 6344
	[Tooltip("Delay between each randomization.")]
	[Range(0.01f, 0.5f)]
	public float noiseDelay = 0.05f;

	// Token: 0x040018C9 RID: 6345
	private float noiseVal;

	// Token: 0x040018CA RID: 6346
	[Tooltip("Sprite sorting order.")]
	public int sortingOrder = 1;

	// Token: 0x040018CB RID: 6347
	[Tooltip("Sprite sorting layer.")]
	[SortingLayer]
	public int sortingLayer;

	// Token: 0x040018CC RID: 6348
	[Tooltip("Particles emitted when a light ray hits something.")]
	public ParticleSystem particles;

	// Token: 0x040018CD RID: 6349
	[Tooltip("Delay between each particle emit.")]
	[Range(0.02f, 0.1f)]
	public float particleEmitDelay = 0.1f;

	// Token: 0x040018CE RID: 6350
	[Tooltip("How many rays emit particles.")]
	[Range(0.02f, 0.1f)]
	public float particleRayAmount = 0.1f;

	// Token: 0x040018CF RID: 6351
	[Tooltip("How many particles to emit.")]
	[Range(1f, 10f)]
	public int particleEmitAmount = 2;

	// Token: 0x040018D0 RID: 6352
	[Tooltip("Minimum distance light have to travel to emit particle.")]
	public float particleRangeLimitMin = 0.05f;

	// Token: 0x040018D1 RID: 6353
	[Tooltip("Maximum distance light can travel to emit particle.")]
	public float particleRangeLimitMax = 5f;

	// Token: 0x040018D2 RID: 6354
	[Tooltip("Gameobject containing the lights MeshRenderer.")]
	public Transform meshTransform;

	// Token: 0x040018D3 RID: 6355
	public List<MeshFilter> duplicatedLights;

	// Token: 0x040018D4 RID: 6356
	public float falloffAfterglow;

	// Token: 0x040018D5 RID: 6357
	public float falloff;

	// Token: 0x040018D6 RID: 6358
	public bool falloffMobileFix;

	// Token: 0x040018D7 RID: 6359
	private float lightTime;

	// Token: 0x040018D8 RID: 6360
	private Vector3[] points;

	// Token: 0x040018D9 RID: 6361
	private Vector3[] pointsF;

	// Token: 0x040018DA RID: 6362
	private Vector3[] pointsX;

	// Token: 0x040018DB RID: 6363
	private Vector3[] pointsP;

	// Token: 0x040018DC RID: 6364
	private float[] str;

	// Token: 0x040018DD RID: 6365
	public Vector3 savePos;

	// Token: 0x040018DE RID: 6366
	public Quaternion saveRot;

	// Token: 0x040018DF RID: 6367
	private Mesh lightMesh;

	// Token: 0x040018E0 RID: 6368
	private Mesh outlineMesh;

	// Token: 0x040018E1 RID: 6369
	private Mesh combinedLightMeshes;

	// Token: 0x040018E2 RID: 6370
	public Transform cacheTransform;

	// Token: 0x040018E3 RID: 6371
	public Transform cacheParticleTransform;

	// Token: 0x040018E4 RID: 6372
	private MeshRenderer cacheMeshRenderer;

	// Token: 0x040018E5 RID: 6373
	private MeshFilter cacheMeshFilter;

	// Token: 0x040018E6 RID: 6374
	private int pointsPlenght;

	// Token: 0x040018E7 RID: 6375
	private float degreeResolution;

	// Token: 0x040018E8 RID: 6376
	private Vector3[] verts;

	// Token: 0x040018E9 RID: 6377
	private Color[] colors;

	// Token: 0x040018EA RID: 6378
	private Color[] colorsX;

	// Token: 0x040018EB RID: 6379
	private Vector3[] vertsF;

	// Token: 0x040018EC RID: 6380
	private int[] trisF;

	// Token: 0x040018ED RID: 6381
	private Vector2[] u;

	// Token: 0x040018EE RID: 6382
	private Vector2[] uF;

	// Token: 0x040018EF RID: 6383
	private int[] tris;

	// Token: 0x040018F0 RID: 6384
	private int currentResolution = -1;

	// Token: 0x040018F1 RID: 6385
	private Vector2[] pointsC;
}
