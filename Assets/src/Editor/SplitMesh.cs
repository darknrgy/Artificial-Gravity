#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

public class SplitMesh : MonoBehaviour {

	public Mesh MeshAsset;
	public int Division;
	public Material ProductMaterial;

	// Use this for initialization
	void Start () {
		Debug.Log ("Attempting to split mesh based on parameters ");
		Vector3[] verticies = MeshAsset.vertices;
		Mesh[] meshes = new Mesh[Division];
		int num = MeshAsset.vertexCount / Division;
		for (int i = 0; i < Division; ++i) {
			meshes [i] = new Mesh ();
			Vector3[] subverts = new Vector3[num];
			Vector2[] uvs = new Vector2[num];
			Vector3[] normals = new Vector3[num];
			for (int j = i * num; j - (i * num) < num; ++j) {
				subverts [j - (i * num)] = verticies [j];
				uvs [j - (i * num)] = MeshAsset.uv [j];
				normals [j - (i * num)] = MeshAsset.normals [j];

			}
			meshes [i].vertices = subverts;
			meshes [i].triangles = Enumerable.Range(0, subverts.Length).Reverse().ToArray();
			meshes [i].uv = uvs;
			meshes [i].normals = normals;
			meshes [i].Optimize ();
		}

		Debug.Log ("Creating Prefab...");
		GameObject parent = new GameObject ();
		// Handle Rounding Down....
		for (int m = 0; m < Division; ++m) {
			GameObject child = new GameObject ();
			child.name = "AsteroidPiece" + m;
			child.AddComponent<MeshFilter> ().mesh = meshes [m];
			child.AddComponent<MeshRenderer> ().material = ProductMaterial;
			child.AddComponent<MeshCollider> ();
			child.AddComponent<Rigidbody> ().isKinematic = true;
			child.GetComponent<Rigidbody> ().useGravity = false;
			child.GetComponent<Rigidbody> ().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			child.GetComponent<Rigidbody> ().interpolation = RigidbodyInterpolation.Extrapolate;
			child.transform.parent = parent.transform;

			Debug.Log ("Creating Mesh...");
			AssetDatabase.CreateAsset(meshes[m], "Assets/Models/Asteroid Chunks/AsteroidSubMesh" + m + ".asset");
		}


		PrefabUtility.CreatePrefab("Assets/Prefabs/SplitAsteroid.prefab", parent);
		Debug.Log("Proccess Finished...");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
#endif