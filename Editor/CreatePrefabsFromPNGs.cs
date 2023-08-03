#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class CreatePrefabsFromPNGs
{
    [MenuItem("Assets/Create Prefabs From PNGs")]
    public static void CreatePrefabs()
    {
        // Open a dialog to select the input directory for PNGs
        string absoluteInputPath = EditorUtility.OpenFolderPanel("Select Input Folder", "", "");
        // Convert the absolute path to a relative path
        string inputPath = "Assets" + absoluteInputPath.Substring(Application.dataPath.Length);

        // Open a dialog to select the output directory for prefabs
        string absoluteOutputPath = EditorUtility.OpenFolderPanel("Select Output Folder", "", "");
        // Convert the absolute path to a relative path
        string outputPath = "Assets" + absoluteOutputPath.Substring(Application.dataPath.Length);

        // Create a directory for sprites
        AssetDatabase.CreateFolder(outputPath, "Sprites");
        string spritePath = outputPath + "/Sprites";

        // Get all assets in the input directory
        string[] assetPaths = AssetDatabase.FindAssets("t:Texture2D", new string[] { inputPath });

        foreach (var assetPath in assetPaths)
        {
            // Load the asset
            var asset = AssetDatabase.LoadAssetAtPath<Texture2D>(AssetDatabase.GUIDToAssetPath(assetPath));

            // Check if the asset is a Texture2D (a .png file)
            if (asset is Texture2D)
            {
                // Create a new Sprite
                float pixelsPerUnit = Mathf.Max(asset.width, asset.height);
                Sprite sprite = Sprite.Create(asset, new Rect(0, 0, asset.width, asset.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);


                // Save the sprite as an asset
                AssetDatabase.CreateAsset(sprite, spritePath + "/" + asset.name + ".asset");
                AssetDatabase.SaveAssets();

                // Create a new game object
                GameObject go = new GameObject(asset.name);
                // Add a Sprite Renderer component to the game object
                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                // Set the Sprite of the Sprite Renderer
                renderer.sprite = sprite;

                // Create a new Prefab at the specified path
                string prefabPath = outputPath + "/" + asset.name + ".prefab";
                var prefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);

                // Delete the game object
                Object.DestroyImmediate(go);
            }
        }
    }
}
#endif
