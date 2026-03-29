using UnityEngine;

[CreateAssetMenu(fileName="New Tool", menuName="Tool")]
public class ToolSO : ScriptableObject
{
    public Texture2D cursorTexture;
    [SerializeField] Tools tool;

    public Tools GetTool() => tool;
}
