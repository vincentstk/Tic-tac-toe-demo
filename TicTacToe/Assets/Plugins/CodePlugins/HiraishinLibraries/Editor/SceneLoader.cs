using UnityEditor;
namespace Hiraishin.EditorUtilities
{
	public partial class SceneManagerHelper
	{
		#if UNITY_EDITOR
		[MenuItem("Tools/Scene Manager Helper/Open/DemoScene")]
		public static void LoadDemoScene()
		{
			OpenScene("Assets/Scenes/DemoScene.unity");
		}
		#endif
	}
}