using UnityEngine;

public class PersistentGuid : MonoBehaviour
{
    [SerializeField] private string guid;
    public string Guid => guid;

    #if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(guid))
        {
            guid = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}
