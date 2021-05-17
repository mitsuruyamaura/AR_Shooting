using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType {
    LifeUp,
    Bullet,
}


public class ItemController : EventBase<int>
{
    public ItemType itemType;

    public int itemAmout;

    private PlayerController playerController;

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="playerController"></param>
    public void SetUpItem(PlayerController playerController) {
        this.playerController = playerController;
    }

    public override void TriggerEvent(int value) {
        ItemEffect(value);
    }

    /// <summary>
    /// アイテムの効果
    /// </summary>
    /// <param name="value"></param>
    private void ItemEffect(int value) {
        switch (itemType) {
            case ItemType.LifeUp:
                playerController.CalcHp(itemAmout);
                break;

            case ItemType.Bullet:
                playerController.CalcBulletCount(itemAmout);
                break;
        }

        Destroy(gameObject);
    }
}
