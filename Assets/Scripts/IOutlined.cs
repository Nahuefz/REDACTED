using UnityEngine;
public interface IOutlined
{
    public void DrawOutline(GameObject obj) { }
    public void DrawOutline(){ }
    public void DrawOutline(int outlineIndex){ }
    public void EraseOutline(GameObject obj) { }
    public void EraseOutline(){ }
    public void EraseOutline(int outlineIndex){ }
}
