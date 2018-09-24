using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

public class explorerManager : MonoBehaviour {

    public GameObject previewer;

    private string pathFBX;

    private static string _sourceExtension = ".fbx";
    private static string _targetExtension = ".asset";

    // Use this for initialization
    void Start () {
        
        pathFBX = Application.dataPath + "/Resources/";

        var fileList = ReadDirectory();
        print(fileList[0].ToString());
        var mesh = ExtractMeshes(fileList[0].ToString());
        print(mesh.name);
        previewer.GetComponent<MeshFilter>().mesh = mesh;


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private ArrayList ReadDirectory()
    {
        var info = new DirectoryInfo(pathFBX);
        var fileInfo = info.GetFiles();
        var filesList = new ArrayList();
        foreach (var file in fileInfo)
        {
            var filename = file.ToString();
            if(filename.Contains("fbx") && !filename.Contains("meta"))
            {
                filesList.Add(filename);
            }
        }

        return filesList;
    }

   /* private Mesh GetAMeshFrom3DModel(string MeshPath)
    {
        Mesh mesh = (Mesh)Resources.Load(MeshPath, typeof(Mesh)); ;

        return mesh;
    }*/

    private static Mesh ExtractMeshes(string selectedObjectPath)
    {
        //Create Folder Hierarchy
        //string selectedObjectPath = AssetDatabase.GetAssetPath(selectedObject);

        string objectFolderName = "Assets\\FBX";
        string objectFolderPath = "\\" + objectFolderName;
        string meshFolderName = "Meshes";
        string meshFolderPath = objectFolderPath + "\\" + meshFolderName;

        if (!AssetDatabase.IsValidFolder(objectFolderPath))
        {
            AssetDatabase.CreateFolder("", objectFolderName);

            if (!AssetDatabase.IsValidFolder(meshFolderPath))
            {
                AssetDatabase.CreateFolder("", meshFolderName);
            }
        }

        //Create Meshes
        Object[] objects = Resources.LoadAll("");

        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i] is Mesh)
            {

                Mesh mesh = Object.Instantiate(objects[i]) as Mesh;
                AssetDatabase.CreateAsset(mesh, meshFolderPath + "\\" + objects[i].name + _targetExtension);
            }
        }

        //Cleanup
        //AssetDatabase.MoveAsset(selectedObjectPath, objectFolderPath + "/" + selectedObject.name + _sourceExtension);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Mesh meshout = Object.Instantiate(objects[0]) as Mesh;
        return meshout;
    }

}
