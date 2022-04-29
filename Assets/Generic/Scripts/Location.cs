using UnityEngine;

public class Location : MonoBehaviour {
	public static Location Of(Transform tr) {
		for(; tr != null; tr = tr.transform.parent) {
			Location res = tr.GetComponent<Location>();
			if(res != null)
				return res;
		}
		return null;
	}
}
