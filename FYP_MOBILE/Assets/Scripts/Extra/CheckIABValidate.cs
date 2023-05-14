using System;
using System.Collections;
using SimpleJSON;
using UnityEngine;

public class CheckIABValidate : MonoBehaviour
{
	public string packageName;

	public string client_id;

	public string client_secret;

	public string refresh_token;

	private Purchase currentPurchase;

	private Action<bool, string, validateResult> callback;

	public void check(Purchase purchase, Action<bool, string, validateResult> mCallback)
	{
		if (purchase == null)
		{
			callback(arg1: false, "purchase is null", null);
			return;
		}
		currentPurchase = purchase;
		callback = mCallback;
		try
		{
			WWWForm wWWForm = new WWWForm();
			wWWForm.AddField("grant_type", "refresh_token");
			wWWForm.AddField("client_id", client_id);
			wWWForm.AddField("client_secret", client_secret);
			wWWForm.AddField("refresh_token", refresh_token);
			WWW www = new WWW("https://pardakht.cafebazaar.ir/devapi/v2/auth/token/", wWWForm);
			StartCoroutine(waitForRefreshCode(www));
		}
		catch
		{
			callback(arg1: false, "error requesting access code", null);
		}
	}

	private IEnumerator waitForRefreshCode(WWW www)
	{
		yield return www;
		if (www.error == null)
		{
			string text = JSON.Parse(www.text)["access_token"].Value.ToString();
			WWW www2 = new WWW("https://pardakht.cafebazaar.ir/devapi/v2/api/validate/" + packageName + "/inapp/" + currentPurchase.productId + "/purchases/" + currentPurchase.orderId + "/?access_token=" + text);
			StartCoroutine(waitForRequest(www2));
		}
		else
		{
			callback(arg1: false, "error getting access code", null);
		}
	}

	private IEnumerator waitForRequest(WWW www)
	{
		yield return www;
		if (www.error == null)
		{
			JSONNode jSONNode = JSON.Parse(www.text);
			if (jSONNode["developerPayload"].Value.ToString() == currentPurchase.payload)
			{
				validateResult validateResult2 = new validateResult();
				validateResult2.isConsumed = jSONNode["consumptionState"].AsInt == 0;
				validateResult2.isRefund = jSONNode["purchaseState"].AsInt == 1;
				validateResult2.kind = jSONNode["kind"].Value.ToString();
				validateResult2.payload = jSONNode["developerPayload"].Value.ToString();
				validateResult2.time = jSONNode["purchaseTime"].Value.ToString();
				callback(arg1: true, "purchase is valid", validateResult2);
			}
			else
			{
				callback(arg1: false, "error validating purchase. payload is not valid.", null);
			}
		}
		else
		{
			callback(arg1: false, "error validating purchase. " + www.error, null);
		}
	}
}
