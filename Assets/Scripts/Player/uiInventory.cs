using System;
using UnityEngine;
using UnityEngine.UI;

public class uiInventory : MonoBehaviour
{
   [SerializeField] private Inventory playerInventory;
   [SerializeField] private Image[] slots;
   [SerializeField] private PlayerMovement playerInputs;
   [SerializeField] private GameObject[] UIinventory;
   private void Start()
   {
      playerInventory.OnInventoryChanged += UpdateItemInventory;
      UpdateItemInventory();
   }
   private void Update()
   {
      if ((playerInputs._controls.Player.OpenInventory.WasPerformedThisFrame() || playerInputs._controls.UI.CloseInventory.WasPerformedThisFrame()) && Time.timeScale != 0)
      {
         SwitchUI();
      }
   }
   void UpdateItemInventory()
   {
      var items = playerInventory.GetInventory;

      for (int i = 0; i < slots.Length; i++)
      {
         if (i < items.Count)
         {
            slots[i].sprite = items[i].itemImage;
         }
         else
         {
            slots[i].sprite = null;
         }
      }
   }
   void SwitchUI()
   {
      bool isEnabled = !UIinventory[0].activeSelf;
      foreach (var VARIABLE in UIinventory)
      {
         VARIABLE.SetActive(!VARIABLE.activeSelf);
      }
      if (isEnabled)
      {
         playerInputs._controls.Player.Disable();
         playerInputs._controls.UI.Enable();
      }
      else
      {
         playerInputs._controls.Player.Enable();
         playerInputs._controls.UI.Disable();
      }
      
      Cursor.visible = UIinventory[0].activeSelf;
      Cursor.lockState = UIinventory[0].activeSelf ? CursorLockMode.None : CursorLockMode.Locked;
      //playerInputs.enabled = !playerInputs.enabled; jajaj... nose
   }
}
