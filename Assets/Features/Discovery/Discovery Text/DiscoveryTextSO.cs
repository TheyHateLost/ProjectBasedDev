using UnityEngine;

[CreateAssetMenu(fileName = "New Discovery Text", menuName = "Discovery/Discovery Text")]
public class DiscoveryTextSO : ScriptableObject
{
    [TextArea]
    public string text;
}
