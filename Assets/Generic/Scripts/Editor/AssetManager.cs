using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unapparent {
	public static class AssetManager {
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

		public static string CreateAsset(Object obj, string path) {
			if(Application.isPlaying)
				return null;
			path = AssetDatabase.GenerateUniqueAssetPath(path);
			AssetDatabase.CreateAsset(obj, path);
			return AssetDatabase.AssetPathToGUID(path);
		}

		public static string CreateManagedAsset(Object obj, string folderName, string name = null, string suffix = ".asset") {
			EstablishPath(sceneFolderPath);
			string folder = EstablishPath(Path.Combine(sceneFolderPath, folderName));
			string path = Path.Combine(folder, "Generated Asset" + suffix);
			string guid = CreateAsset(obj, path);
			path = AssetDatabase.GUIDToAssetPath(guid);
			string newName = Path.Combine(folder, (name != null ? name : guid) + suffix);
			newName = AssetDatabase.GenerateUniqueAssetPath(newName);
			newName = Path.GetFileName(newName);
			string res = AssetDatabase.RenameAsset(path, newName);
			if(!string.IsNullOrEmpty(res))
				Debug.LogWarning(res);
			return guid;
		}

		public static string CurrentFolderPath {
			get {
				foreach(Object obj in Selection.GetFiltered<Object>(SelectionMode.Assets)) {
					string path = AssetDatabase.GetAssetPath(obj);
					if(string.IsNullOrEmpty(path))
						continue;
					if(Directory.Exists(path))
						return path;
					else if(File.Exists(path))
						return Path.GetDirectoryName(path);
				}
				return "Assets";
			}
		}

		public static string CreateAssetInCurrentFolder(Object obj, string name = null, string suffix = ".asset") =>
			CreateAsset(obj, Path.Combine(CurrentFolderPath, name + suffix));
	}
}
