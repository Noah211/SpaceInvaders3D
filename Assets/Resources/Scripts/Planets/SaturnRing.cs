using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Original Author: Board to Bits Games, https://github.com/boardtobits/planet-ring-mesh
// Additions: Noah Logan

public class SaturnRing : MonoBehaviour
{
	[Range(3, 360)]
	public int segments = 180;
	public float innerRadius = 0.7f;
	public float thickness = 0.5f;
	public Material ringMaterial;

	private GameObject ring;
	private Mesh ringMesh;
	private MeshFilter ringMF;
	private MeshRenderer ringMR;

	void OnEnable()
	{
		if (ring == null || ringMesh == null)
		{
			SetUpRing();
		}

		BuildRingMesh();
	}

	void OnValidate()
	{
		if (ring == null || ringMesh == null)
		{
			SetUpRing();
		}

		BuildRingMesh();
	}

	private void SetUpRing()
	{
		//check if ring is null and there are no children
		if (ring == null && transform.childCount == 0)
		{
			//create ring object
			ring = new GameObject("Saturn Ring");
			ring.transform.parent = transform;
			ring.transform.SetAsFirstSibling();
			ring.transform.localScale = Vector3.one;
			ring.transform.localPosition = Vector3.zero;
			ring.transform.localRotation = Quaternion.identity;
			ringMF = ring.AddComponent<MeshFilter>();
			ringMR = ring.AddComponent<MeshRenderer>();
			ringMR.material = ringMaterial;
		}
		else
		{
			ring = transform.GetChild(0).gameObject;
			ringMF = ring.GetComponent<MeshFilter>();
			ringMR = ring.GetComponent<MeshRenderer>();
		}

		ringMesh = new Mesh();
		ringMF.sharedMesh = ringMesh;
	}

	private void BuildRingMesh()
	{
		// I realize that magic numbers are not good practice but I felt that having a bunch of variable names did not fit in this instance.

		Vector3[] vertices = new Vector3[(segments + 1) * 2 * 2]; // * 2 * 2 for top-side and bottom-side
		int[] triangles = new int[segments * 6 * 2];              
		Vector2[] uv = new Vector2[(segments + 1) * 2 * 2];       
		int halfway = (segments + 1) * 2;						  // First index of the bottom-side

		for (int i = 0; i < segments + 1; i++)
		{
			float progress = (float)i / (float)segments;
			float angle = Mathf.Deg2Rad * progress * 360;
			float x = Mathf.Sin(angle);
			float z = Mathf.Cos(angle);

			vertices[i * 2] = vertices[i * 2 + halfway] = new Vector3(x, 0f, z) * (innerRadius + thickness);	// top-side
			vertices[i * 2 + 1] = vertices[i * 2 + 1 + halfway] = new Vector3(x, 0f, z) * innerRadius;          // bottom-side
			uv[i * 2] = uv[i * 2 + halfway] = new Vector2(0f, progress);										// moving vertically across bottom of texture
			uv[i * 2 + 1] = uv[i * 2 + 1 + halfway] = new Vector2(1f, progress);                                // moving vertically across top of texture

			if (i != segments)	// if not on last loop
			{
				// triangles for top-side
				triangles[i * 12] = i * 2;	// use 12 because were creating 4 sets of triangles, 2 for top-side, 2 for bottom-side
				triangles[i * 12 + 1] = triangles[i * 12 + 4] = (i + 1) * 2;
				triangles[i * 12 + 2] = triangles[i * 12 + 3] = i * 2 + 1;
				triangles[i * 12 + 5] = (i + 1) * 2 + 1;

				// triangles for bottom-side
				triangles[i * 12 + 6] = i * 2 + halfway;
				triangles[i * 12 + 7] = triangles[i * 12 + 10] = i * 2 + 1 + halfway;
				triangles[i * 12 + 8] = triangles[i * 12 + 9] = (i + 1) * 2 + halfway;
				triangles[i * 12 + 11] = (i + 1) * 2 + 1 + halfway;
			}
		}

		if (vertices.Length < ringMesh.vertices.Length)
		{
			ringMesh.triangles = triangles;
			ringMesh.vertices = vertices;
		}
		else
		{
			ringMesh.vertices = vertices;
			ringMesh.triangles = triangles;
		}

		ringMesh.uv = uv;
		ringMesh.RecalculateNormals();
	}
}
