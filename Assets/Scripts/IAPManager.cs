using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Extension;

public class IAPManager : MonoBehaviour
{
    private string coins100 = "coins100";
    private string coins250 = "coins250";
    private string coins400 = "coins400";
    private string coins750 = "coins750";
    
    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == coins100)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 100);
            SoundManager.instance.PlaySFX("purchase");

        }
        if (product.definition.id == coins250)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 250);
            SoundManager.instance.PlaySFX("purchase");

        }
        if (product.definition.id == coins400)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 400);
            SoundManager.instance.PlaySFX("purchase");

        }
        if (product.definition.id == coins750)
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins", 0) + 750);
            SoundManager.instance.PlaySFX("purchase");

        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureDescription purchaseFailureReason)
    {
        Debug.Log(product.definition.id + "purchase failure reason" + purchaseFailureReason);
    }
}
