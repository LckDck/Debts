using System;
using System.Threading.Tasks;

namespace Debts.Interfaces
{
	public interface IInAppPurchase
	{
		string PaidItem { get; }
		Task<string> RestoreProduct ();
		Task<string> BuyProduct (string id);
		Task<ProductInfo> GetProdutctInfo (string id);
		void Disconnect ();
	}
}
