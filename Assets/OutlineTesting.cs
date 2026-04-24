using System;
using UnityEngine;

public class OutlineTesting : MonoBehaviour, IOutlined
{
   private Outline outline;

   private void Awake()
   {
      outline = GetComponent<Outline>();
   }

   public void DrawOutline(GameObject obj)
   {
      outline.enabled = true;
   }

   public void EraseOutline(GameObject obj)
   {
      outline.enabled = false;
   }
}
