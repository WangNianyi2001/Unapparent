using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unapparent {
	public static class ManagedAsset {
		public const string sceneFolderName = "Managed";
		public static string scenePath => Path.GetDirectoryName(SceneManager.GetActiveScene().path);

		public static string sceneFolderPath => Path.Combine(scenePath, sceneFolderName);

		public static string EstablishPath(string path) {
			if(string.IsNullOrEmpty(path)) {
				Debug.LogError("Path to establish is null or empty.");
				return null;
			}
			if(!Directory.Exists(path))
				Directory.CreateDirectory(path);
			return path;
		}

		public static string EstablishScene() => EstablishPath(sceneFolderPath);

		public static string EstablishSceneFolder(string name) {
			EstablishScene();
			return EstablishPath(Path.Combine(sceneFolderPath, name));
		}

		// Create asset and return GUID

		public static string CreateAsset(Object obj, string folderName, string name = null, string suffix = ".asset") {
			if(Application.isPlaying)
				return null;
			string folder = EstablishSceneFolder(folderName);
			string path = Path.Combine(folder, "Generated Asset" + suffix);
			path = AssetDatabase.GenerateUniqueAssetPath(path);
			AssetDatabase.CreateAsset(obj, path);
			string guid = AssetDatabase.AssetPathToGUID(path);
			string newName = Path.Combine(folder, (name != null ? name : guid) + suffix);
			newName = AssetDatabase.GenerateUniqueAssetPath(newName);
			newName = Path.GetFileName(newName);
			string res = AssetDatabase.RenameAsset(path, newName);
			if(!string.IsNullOrEmpty(res))
				Debug.LogWarning(res);
			return guid;
		}
	}
}
