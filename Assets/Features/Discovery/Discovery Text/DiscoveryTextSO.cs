using UnityEngine;

[CreateAssetMenu(fileName = "New Discovery Text", menuName = "Discovery/Discovery Text")]
public class DiscoveryTextSO : ScriptableObject
{
    [TextArea(20, 20)]
    public string text;
}
