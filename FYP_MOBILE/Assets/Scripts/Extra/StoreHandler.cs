using SimpleJSON;
using UnityEngine;

[RequireComponent(typeof(InAppStore))]
public class StoreHandler : MonoBehaviour
{
	public string publicKey;

	public string payload;

	private AndroidJavaObject pluginUtilsClass;

	private AndroidJavaObject activityContext;

	private static string unityClass = "com.unity3d.player.UnityPlayerNativeActivity";

	private void initiateBilling()
	{
		if (pluginUtilsClass != null)
		{
			return;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			activityContext = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
		}
		using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.bobardo.bazaar.iab.ServiceBillingBazaar"))
		{
			if (androidJavaClass2 != null)
			{
				pluginUtilsClass = androidJavaClass2.CallStatic<AndroidJavaObject>("instance", new object[0]);
				pluginUtilsClass.CallStatic("setContext", activityContext);
				pluginUtilsClass.Call("setPublicKey", publicKey);
				pluginUtilsClass.Call("startIabServiceInBg");
			}
		}
	}

	public void BuyAndConsume(string produc_sku)
	{
		initiateBilling();
		if (pluginUtilsClass != null)
		{
			pluginUtilsClass.Call("PurchaseAndConsume", produc_sku, payload);
		}
	}

	public void BuyProduct(string produc_sku)
	{
		initiateBilling();
		if (pluginUtilsClass != null)
		{
			pluginUtilsClass.Call("launchPurchaseFlow", produc_sku, payload);
		}
	}

	public void CheckInventory(string produc_sku)
	{
		initiateBilling();
		if (pluginUtilsClass != null)
		{
			pluginUtilsClass.Call("checkHasPurchase", produc_sku);
		}
	}

	public void getPurchaseResult(string result)
	{
		if (result.Length == 0 || result == string.Empty || result == null)
		{
			GetComponent<InAppStore>().purchasedFailed(16, "unknown error!!!");
			return;
		}
		try
		{
			JSONNode jSONNode = JSON.Parse(result);
			int asInt = jSONNode["errorCode"].AsInt;
			string text = jSONNode["data"].Value.ToString();
			if (asInt == 0)
			{
				GetComponent<InAppStore>().purchasedSuccessful(getPurchaseData(text));
			}
			else
			{
				GetComponent<InAppStore>().purchasedFailed(asInt, text);
			}
		}
		catch
		{
			GetComponent<InAppStore>().purchasedFailed(17, "the result from cafeBazaar is not valid.");
		}
	}

	public void getInventoryResult(string result)
	{
		if (result.Length == 0 || result == string.Empty || result == null)
		{
			GetComponent<InAppStore>().purchasedFailed(16, "unknown error!!!");
			return;
		}
		try
		{
			JSONNode jSONNode = JSON.Parse(result);
			int asInt = jSONNode["errorCode"].AsInt;
			string text = jSONNode["data"].Value.ToString();
			if (asInt == 0)
			{
				GetComponent<InAppStore>().purchasedSuccessful(getPurchaseData(text));
			}
			else
			{
				GetComponent<InAppStore>().purchasedFailed(asInt, text);
			}
		}
		catch
		{
			GetComponent<InAppStore>().purchasedFailed(17, "the result from cafeBazaar is not valid.");
		}
	}

	private Purchase getPurchaseData(string data)
	{
		JSONNode jSONNode = JSONNode.Parse(data);
		return new Purchase
		{
			orderId = jSONNode["orderId"].Value.ToString(),
			purchaseToken = jSONNode["purchaseToken"].Value.ToString(),
			payload = jSONNode["developerPayload"].Value.ToString(),
			packageName = jSONNode["packageName"].Value.ToString(),
			purchaseState = jSONNode["purchaseState"].AsInt,
			purchaseTime = jSONNode["purchaseTime"].Value.ToString(),
			productId = jSONNode["productId"].Value.ToString(),
			json = data
		};
	}

	public void DebugLog(string msg)
	{
		Debug.Log(msg);
	}

	private void OnApplicationQuit()
	{
		if (pluginUtilsClass != null)
		{
			pluginUtilsClass.Call("stopIabHelper");
		}
	}
}
