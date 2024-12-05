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
        [SerializeField] List<Item> input;  // List các item yêu cầu cho Furnace (ví dụ: gỗ)
        [SerializeField] List<int> inputAmounts; // List các số lượng yêu cầu cho mỗi item
        [SerializeField] List<Item> output; // List các item kết quả sau khi gộp (ví dụ: rìu)
        [SerializeField] List<int> outputAmounts; // List số lượng item kết quả sau khi gộp
        [SerializeField] List<int> processingTime;  // Thời gian gộp cho mỗi item

        // Smelting Logic
        bool isSmelting;
        int index;

        // Reference đến Inventory của người chơi
        private PlayerInventory playerInventory;

        // Start is called before the first frame update
        void Start()
        {
            index = -1;
            isSmelting = false;

            playerInventory = Player.Instance.GetComponent<PlayerInventory>();
        }

        // Hàm tương tác với Furnace
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
                // Kiểm tra xem item có phải là phần của công thức và đủ số lượng để gộp
                if (item != null && item == input[i] && itemCount >= inputAmounts[i])
                {
                    index = i;
                    isSmelting = true;

                    // Xóa số lượng item cần thiết khỏi inventory
                    playerInventory.RemoveItem(playerInventory.CurrentlySelectedInventoryItem, inputAmounts[i]);

                    // Bắt đầu quá trình gộp
                    InvokeSmelted(); // Sau khi gộp thành công, tiếp tục quá trình
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
            // Gọi hàm Smelted sau thời gian xử lý
            Invoke("Smelted", processingTime[index]);
        }

        private void Smelted()
        {
            // Kết thúc quá trình gộp, cập nhật trạng thái
            isSmelting = false;

            // Thêm item kết quả vào inventory
            playerInventory.AddItem(output[index], outputAmounts[index]); // Thêm item mới vào inventory

            // Reset trạng thái gộp
            index = -1;
        }
    }
}
