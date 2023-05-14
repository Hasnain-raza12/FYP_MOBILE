using System;

[Serializable]
public class Product
{
	public enum ProductType
	{
		Consumable,
		NonConsumable
	}

	public string productId;

	public ProductType type;
}
