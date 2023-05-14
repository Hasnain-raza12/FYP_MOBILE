using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(StoreHandler))]
public class InAppStore : MonoBehaviour
{
	public Text StatusText;

	public GameObject ShopLoading;

	public Product[] products;

	private int selectedProductIndex;

	private void Start()
	{
		ShopLoading.SetActive(value: false);
	}

	public void purchasedSuccessful(Purchase purchase)
	{
		GetComponent<CheckIABValidate>().check(purchase, onPurchaseValidated);
		ShopLoading.SetActive(value: true);
	}

	public void purchasedFailed(int errorCode, string info)
	{
	}

	public void userHasThisProduct(Purchase purchase)
	{
		if (selectedProductIndex != 3)
		{
			throw new UnassignedReferenceException("you forgot to give user the product after purchase. product: " + purchase.productId);
		}
	}

	public void failToGetUserInventory(int errorCode, string info)
	{
	}

	public void purchaseProduct(int productIndex)
	{
		selectedProductIndex = productIndex;
		Product product = products[productIndex];
		if (product.type == Product.ProductType.Consumable)
		{
			GetComponent<StoreHandler>().BuyAndConsume(product.productId);
		}
		else if (product.type == Product.ProductType.NonConsumable)
		{
			GetComponent<StoreHandler>().BuyProduct(product.productId);
		}
	}

	public void checkIfUserHasProduct(int productIndex)
	{
		selectedProductIndex = productIndex;
		GetComponent<StoreHandler>().CheckInventory(products[productIndex].productId);
	}

	private void onPurchaseValidated(bool success, string message, validateResult result)
	{
		if (success)
		{
			switch (selectedProductIndex)
			{
			default:
				throw new UnassignedReferenceException("you forgot to give user the product after purchase. product: ");
			case 0:
			case 1:
			case 2:
			case 3:
				break;
			}
		}
		else
		{
			Debug.Log(message);
		}
		ShopLoading.SetActive(value: false);
	}
}
