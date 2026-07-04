using UnityEngine;

public class OutlineTesting : MonoBehaviour, IOutlined
{
   private Outline outline;
   float _outlineWidth = 4;

   private void Awake()
   {
      outline = GetComponent<Outline>();
      //_outlineWidth = outline.OutlineWidth;
   }

   public void DrawOutline(GameObject obj)
   {
      outline.OutlineWidth = _outlineWidth;
   }

   public void EraseOutline(GameObject obj)
   {
      outline.OutlineWidth = 0f;
   }
}
