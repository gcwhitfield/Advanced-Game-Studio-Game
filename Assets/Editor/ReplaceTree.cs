using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

// Replaces Unity terrain trees with prefab GameObject.
// http://answers.unity3d.com/questions/723266/converting-all-terrain-trees-to-gameobjects.html
[ExecuteInEditMode]
public class ReplaceTree : EditorWindow
{
    [Header("Settings")]
    public GameObject _tree;

    [Header("References")]
    public Terrain _terrain;

    [MenuItem("Tools/TreeReplacer")]
    private static void Init()
    {
        ReplaceTree window = (ReplaceTree)GetWindow(typeof(ReplaceTree));
    }

    private void OnGUI()
    {
        _terrain = (Terrain)EditorGUILayout.ObjectField(_terrain, typeof(Terrain), true);
        _tree = (GameObject)EditorGUILayout.ObjectField(_tree, typeof(GameObject), true);

        GUILayout.Label("Create");

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Convert (Keep Previous)", GUILayout.Height(40f)))
        {
            Convert();
        }

        if (GUILayout.Button("Convert (Clear Previous)", GUILayout.Height(40f)))
        {
            Clear();
            Convert();
        }

        GUILayout.EndHorizontal();

        GUILayout.Label("Destroy");

        GUILayout.BeginHorizontal();

        Color oldColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;

        if (GUILayout.Button("Clear generated trees", GUILayout.Height(40f)))
        {
            Clear();
        }

        if (GUILayout.Button("ClearTerrainTreeInstances", GUILayout.Height(40f)))
        {
            ClearTerrainTreeInstances();
        }
        GUI.backgroundColor = oldColor;

        GUILayout.EndHorizontal();
    }

    public void Convert()
    {
        TerrainData data = _terrain.terrainData;
        float width = data.size.x;
        float height = data.size.z;
        float y = data.size.y;

        // Get all parents transform
        Vector3 offsetTransform = _terrain.transform.position;

        // Create parent
        GameObject parent = GameObject.Find("TREES_GENERATED");
        if (parent == null)
        {
            parent = new GameObject("TREES_GENERATED");
        }
        //parent.transform.position = offsetTransform;
        // Create trees
        for (int i = 0; i < data.treeInstances.Length; i++)
        {
            TreeInstance tree = data.treeInstances[i];

            Vector3 position = new Vector3(tree.position.x * width, tree.position.y * y, tree.position.z * height);
            position += offsetTransform;
            //Vector3 position = new Vector3(tree.position.x * data.detailWidth - (data.size.x / 2), tree.position.y * y - (data.size.y / 2), tree.position.z * data.detailHeight - (data.size.z / 2));

            // Instantiate as Prefab if is one, if not, instantiate as normal
            GameObject go = PrefabUtility.InstantiatePrefab(_tree) as GameObject;

            if (go != null)
            {
                go.name += " (" + i.ToString() + ")";
                go.transform.position = position;
                go.transform.parent = parent.transform;
            }
            else
            {
                Instantiate(_tree, position, Quaternion.identity, parent.transform);
            }
        }
    }

    public void Clear()
    {
        DestroyImmediate(GameObject.Find("TREES_GENERATED"));
    }

    public void ClearTerrainTreeInstances()
    {
        _terrain.terrainData.treeInstances = new List<TreeInstance>().ToArray();
    }
}