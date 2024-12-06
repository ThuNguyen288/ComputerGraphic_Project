using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kinnly
{
    public class Furnace : MonoBehaviour, IInteractable
    {
        [Header("Core")]
        [SerializeField] Item furnace;

        [Header("Recipe")]
        [SerializeField] List<Item> input;
        [SerializeField] List<int> inputAmounts;
        [SerializeField] List<Item> output;
        [SerializeField] List<int> outputAmounts;
        [SerializeField] List<int> processingTime;

        [Header("Decorations")]
        [SerializeField] ParticleSystem smeltingEffect; // Hiệu ứng lửa
        [SerializeField] AudioSource smeltingSound;     // Âm thanh nung chảy

        bool isSmelting;
        int index;

        private PlayerInventory playerInventory;

        void Start()
        {
            index = -1;
            isSmelting = false;

            playerInventory = Player.Instance.GetComponent<PlayerInventory>();
            if (smeltingEffect != null) smeltingEffect.Stop(); // Đảm bảo hiệu ứng tắt lúc đầu
        }

        public void Interact(PlayerInventory playerInventory)
        {
            if (isSmelting)
            {
                DialogBox.instance.Show("Currently Working", 1f);
                return;
            }

            if (playerInventory.CurrentlySelectedInventoryItem.Item != null)
            {
                Smelting(playerInventory);
            }
        }

        private void Smelting(PlayerInventory playerInventory)
        {
            Item item = playerInventory.CurrentlySelectedInventoryItem.Item;
            int itemCount = playerInventory.CurrentlySelectedInventoryItem.Amount;

            for (int i = 0; i < input.Count; i++)
            {
                if (item != null && item == input[i] && itemCount >= inputAmounts[i])
                {
                    index = i;
                    isSmelting = true;

                    playerInventory.RemoveItem(playerInventory.CurrentlySelectedInventoryItem, inputAmounts[i]);

                    // Bật hiệu ứng nung chảy
                    if (smeltingEffect != null) smeltingEffect.Play();
                    if (smeltingSound != null) smeltingSound.Play();

                    InvokeSmelted();
                    return;
                }
            }

            if (!isSmelting)
            {
                DialogBox.instance.Show("This Item Cannot be Merged", 1f);
            }
        }

        private void InvokeSmelted()
        {
            Invoke("Smelted", processingTime[index]);
        }

        private void Smelted()
        {
            isSmelting = false;

            // Tắt hiệu ứng nung chảy
            if (smeltingEffect != null) smeltingEffect.Stop();
            if (smeltingSound != null) smeltingSound.Stop();

            playerInventory.AddItem(output[index], outputAmounts[index]);

            index = -1;
        }
    }
}
