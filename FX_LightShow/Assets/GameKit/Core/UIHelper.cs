using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public static class UIHelper
{
	/*public static string mainStyle = "MainStyle";
	public static string subStyle1 = "SubStyle1";
    public static string subStyle2 = "SubStyle2";
    public static string buttonStyle = "Button";
    public static string redButtonStyle = "RedButton";
    public static string greenButtonStyle = "GreenButton";
    public static string warningStyle = "Warning";
    public static string headerStyle = "Button";*/
	
	public static GUIStyle mainStyle;
	public static GUIStyle subStyle1;
	public static GUIStyle subStyle2;
	public static GUIStyle subStyle3;
	public static GUIStyle buttonStyle;
	public static GUIStyle redButtonStyle;
	public static GUIStyle greenButtonStyle;
	public static GUIStyle warningStyle;
	public static GUIStyle headerStyle;
	
	private static Texture2D MakeTex (int width, int height, Color col)
	{
		Color[] pix = new Color[width * height];

		for (int i = 0; i < pix.Length; i++)
			pix[i] = col;

		Texture2D result = new Texture2D(width, height);
		result.SetPixels(pix);
		result.Apply();

		return result;
	}

	public static void InitializeStyles ()
	{
		 GUISkin gameKitSkin = Resources.Load<GUISkin>("GameKit/UI/GameKitSkinB");

		 mainStyle = gameKitSkin.GetStyle("MainStyle");
		 subStyle1 = gameKitSkin.GetStyle("SubStyle1");
		 subStyle2 = gameKitSkin.GetStyle("SubStyle2");
		 subStyle3 = gameKitSkin.GetStyle("SubStyle3");
		 buttonStyle = gameKitSkin.GetStyle("ButtonStyle");
		 redButtonStyle = gameKitSkin.GetStyle("RedButton");
		 greenButtonStyle = gameKitSkin.GetStyle("GreenButton");
		 warningStyle = gameKitSkin.GetStyle("WarningStyle");
		 headerStyle = gameKitSkin.GetStyle("HeaderStyle");
		 

		 /*mainStyle = new GUIStyle("box");
		 mainStyle.normal.background = MakeTex(1, 1, new Color(0.3f, 0.3f, 0.3f, 1f));
		 mainStyle.normal.textColor = Color.black;
		 
		 
		 subStyle1 = new GUIStyle("box");
		 subStyle1.normal.background = MakeTex(1, 1, new Color(0.25f, 0.25f, 0.25f, 1f));
		 subStyle1.normal.textColor = Color.black;
		 
		 subStyle2 = new GUIStyle("box");
		 subStyle2.normal.background = MakeTex(1, 1, new Color(0.20f, 0.20f, 0.20f, 1f));
		 subStyle2.normal.textColor = Color.black;
		 
		 buttonStyle = new GUIStyle("box");
		 buttonStyle.normal.background = MakeTex(1, 1, new Color(0.35f, 0.35f, 0.35f, 1f));
		 buttonStyle.normal.textColor = Color.white;
		 buttonStyle.alignment = TextAnchor.MiddleCenter;
		 buttonStyle.fontStyle = FontStyle.Bold;
		 
		 redButtonStyle = new GUIStyle("box");
		 redButtonStyle.normal.background = MakeTex(1, 1, new Color(0.80f, 0.20f, 0.20f, 1f));
		 redButtonStyle.normal.textColor = Color.white;
		 redButtonStyle.alignment = TextAnchor.MiddleCenter;
		 redButtonStyle.fontStyle = FontStyle.Bold;
		 
		 greenButtonStyle = new GUIStyle("box");
		 greenButtonStyle.normal.background = MakeTex(1, 1, new Color(0.10f, 0.50f, 0.10f, 1f));
		 greenButtonStyle.normal.textColor = Color.white;
		 greenButtonStyle.alignment = TextAnchor.MiddleCenter;
		 greenButtonStyle.fontStyle = FontStyle.Bold;
		 
		 warningStyle = new GUIStyle("box");
		 warningStyle.normal.background = MakeTex(1, 1, new Color(0.6f, .1f, .1f, 1f));
		 warningStyle.normal.textColor = Color.black;
		 warningStyle.fontStyle = FontStyle.Bold;
		 
		 headerStyle = new GUIStyle("box");
		 headerStyle.normal.background = MakeTex(1, 1, new Color(0.35f, 0.35f, 0.35f, 1f));
		 headerStyle.normal.textColor = Color.black;
		 headerStyle.alignment = TextAnchor.MiddleCenter;
		 headerStyle.fontStyle = FontStyle.Bold;*/
	}
	
	public static void PreShotDirty(string undoName, Object target)
	{
		if (EditorApplication.isPlaying) return;
		
		Undo.RecordObject(target, undoName);
	}

	public static void DirtyStuff(Object target)
	{
		if (EditorApplication.isPlaying) return;
		
		EditorUtility.SetDirty(target);
		PrefabUtility.RecordPrefabInstancePropertyModifications(target);
		UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
	}

	public static bool Exists(this SerializedProperty property)
	{
		bool isNotNull = property.propertyType == SerializedPropertyType.ObjectReference &&
		              property.objectReferenceValue != null;

		return isNotNull;
	}
	
	public static void DrawArc (float angle, float range, Transform t)
	{
		Handles.color = Color.blue;
		Vector3 effectAngleA = (-angle / 2).DirFromAngle(t);
		Vector3 effectAngleB = (angle / 2).DirFromAngle(t);

		Vector3 tPosition = t.position;
		Handles.DrawWireArc(tPosition, Vector3.up, effectAngleA, angle, range);
		
		Handles.DrawLine(tPosition, tPosition + effectAngleA * range);
		Handles.DrawLine(tPosition, tPosition + effectAngleB * range);
	}
}