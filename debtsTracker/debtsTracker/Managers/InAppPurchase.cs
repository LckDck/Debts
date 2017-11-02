using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Debts.Interfaces;
using debtsTracker;
using Xamarin.InAppBilling;
using Xamarin.InAppBilling.Utilities;

namespace Debts.Managers
{
	public class InAppPurchase : SimpleTimerContainer, IInAppPurchase
	{
		private InAppBillingServiceConnection _serviceConnection;
		private TaskCompletionSource<bool> _tcs;
		private TaskCompletionSource<bool> _tcsConnection;
		private TaskCompletionSource<string> _tcsRestore;
		private TaskCompletionSource<string> _buyTcs;
		private List<Product> _products = new List<Product> ();
		private string _purchaseId;
		private Purchase _alreadyPurchased;

		public InAppPurchase ()
		{
			StartSetup ();
		}

		public string PaidItem {
			get {
				return "no_ads";
			}
		}


		public Task<string> BuyProduct (string id)
		{
			if (!_serviceConnection.Connected) {
				StartTimer ();
			} else {
				var product = _products.Find (item => item.ProductId == id);
				if (product != null && _alreadyPurchased == null) {
					_serviceConnection.BillingHandler.BuyProduct (product);
				} else {
					StartTimer ();
				}
			}

			_buyTcs = new TaskCompletionSource<string> ();
			return _buyTcs.Task;
		}



		public async Task<ProductInfo> GetProdutctInfo (string id)
		{
			if (!_products.Any ()) {
				await GetInventory ();
			}
			var product = _products.Find (item => item.ProductId == id);
			if (product != null) {
				var title = product.Title;
				var index = title.IndexOf ("(", StringComparison.Ordinal);
				if (index > -1) {
					title = title.Substring (0, index).TrimEnd ();
				}

				ProductInfo prodInfo;
				if (Build.VERSION.SdkInt < BuildVersionCodes.Lollipop) {
					prodInfo = new ProductInfo {
						Id = product.ProductId,
						PriceString = product.Price_Currency_Code + product.Price,
						DisplayName = title
					};
				} else {
					prodInfo = new ProductInfo {
						Id = product.ProductId,
						PriceString = product.Price,
						DisplayName = title
					};
				}

				if (_alreadyPurchased!= null && _alreadyPurchased.ProductId == product.ProductId) {
					prodInfo.Bought = true;
				}

				if (!string.IsNullOrEmpty (product.Price_Amount_Micros)) {
					prodInfo.Price = Convert.ToDecimal (product.Price_Amount_Micros) / 1000000;
				}

				return prodInfo;
			}

			return null;
		}

		public void Disconnect ()
		{
			if (_serviceConnection != null) {
				if (_serviceConnection.Connected) {
					UnBind ();
					_serviceConnection.Disconnect ();
				}
			}
		}

		public bool OnActivityResult (int requestCode, Result resultCode, Intent data)
		{
			// Ask the open service connection's billing handler to process this request
			if (_buyTcs != null) {
				_serviceConnection.BillingHandler.HandleActivityResult (requestCode, resultCode, data);
				return true;
			}
			return false;
		}

		private void StartSetup ()
		{
			// A Licensing and In-App Billing public key is required before an app can communicate with
			// Google Play, however you DON'T want to store the key in plain text with the application.
			// The Unify command provides a simply way to obfuscate the key by breaking it into two or
			// or more parts, specifying the order to reassemlbe those parts and optionally providing
			// a set of key/value pairs to replace in the final string. 
			string value = Security.Unify (
				new [] {
					"MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAuYyJLgxpwNLhsqzkGw/qA0j" ,
                "335u7gCKh1aWCAmoDgNJYmFdLxpYcgJJWFfnZbWiAHoivRwk9qvRxq8Lea1nXNMIBkQY5LnFM", 
                "QzaGPzculqFssramTWizDEIR8ptelluUzr1qtVOoSeyjjrIXgqmHwSCILJ7knKr823lfaimeJNPorR", 
                "Z+0xxJ3zPSf/DGIO4Kkzk/aLSvRqREiIYX0XB2leYJJxFJ5nJapJ7uS+BgsOCJSaM9aKbtNORmf7pPLIzvjpWl", 
                "GcAhp+bFnu93zVDf9OKhW28/lHypIa9y4eKx12ojwr1+E1ogDX/CjivEWMve66xbp0+znuwyT2nj0ExCKwIDAQAB"
					},
				new [] { 0, 1, 2, 3, 4});

			// Create a new connection to the Google Play Service
            _serviceConnection = new InAppBillingServiceConnection (MainActivity.Current, value);
			_serviceConnection.OnConnected += OnConnected;
			_serviceConnection.OnDisconnected += OnDisconnected;
			_serviceConnection.OnInAppBillingError += OnInAppBillingError;
		}

		private void OnInAppBillingError (InAppBillingErrorType error, string message)
		{
			UnBind ();

			if (_tcsConnection != null) {
				_tcsConnection.TrySetResult (false);
			}
		}

		private void OnDisconnected ()
		{
			UnBind ();

			if (_tcsConnection != null) {
				_tcsConnection.TrySetResult (false);
			}
		}

		private void OnConnected ()
		{
			UnBind ();
			Bind ();

			if (_tcsConnection != null) {
				_tcsConnection.TrySetResult (true);
			}
		}

		private void Bind ()
		{
			_serviceConnection.BillingHandler.OnProductPurchased += OnProductPurchased;
			_serviceConnection.BillingHandler.OnPurchaseConsumed += OnPurchaseConsumed;

			// Attach to the various error handlers to report issues
			_serviceConnection.BillingHandler.OnGetProductsError += OnGetProductsError;
			_serviceConnection.BillingHandler.OnInvalidOwnedItemsBundleReturned += OnInvalidOwnedItemsBundleReturned;
			_serviceConnection.BillingHandler.OnProductPurchasedError += OnProductPurchasedError;
			_serviceConnection.BillingHandler.OnPurchaseConsumedError += OnPurchaseConsumedError;
			_serviceConnection.BillingHandler.InAppBillingProcesingError += InAppBillingProcesingError;
			_serviceConnection.BillingHandler.OnUserCanceled += OnUserCanceled;
			_serviceConnection.BillingHandler.OnPurchaseFailedValidation += OnPurchaseFailedValidation;
			_serviceConnection.BillingHandler.QueryInventoryError += OnQueryInventoryError;
			_serviceConnection.BillingHandler.BuyProductError += OnBuyProductError;
		}

		private void UnBind ()
		{
			if (_serviceConnection != null && _serviceConnection.BillingHandler != null) {
				_serviceConnection.BillingHandler.OnProductPurchased -= OnProductPurchased;
				_serviceConnection.BillingHandler.OnPurchaseConsumed -= OnPurchaseConsumed;

				_serviceConnection.BillingHandler.OnGetProductsError -= OnGetProductsError;
				_serviceConnection.BillingHandler.OnInvalidOwnedItemsBundleReturned -= OnInvalidOwnedItemsBundleReturned;
				_serviceConnection.BillingHandler.OnProductPurchasedError -= OnProductPurchasedError;
				_serviceConnection.BillingHandler.OnPurchaseConsumedError -= OnPurchaseConsumedError;
				_serviceConnection.BillingHandler.InAppBillingProcesingError -= InAppBillingProcesingError;
				_serviceConnection.BillingHandler.OnUserCanceled -= OnUserCanceled;
				_serviceConnection.BillingHandler.OnPurchaseFailedValidation -= OnPurchaseFailedValidation;
				_serviceConnection.BillingHandler.QueryInventoryError -= OnQueryInventoryError;
				_serviceConnection.BillingHandler.BuyProductError -= OnBuyProductError;
			}
		}

		private void OnBuyProductError (int responseCode, string sku)
		{

			if (_buyTcs != null) {
				_buyTcs.TrySetResult (null);
				_buyTcs = null;
			}

			if (_tcsRestore != null) {
				_tcsRestore.TrySetResult (null);
				_tcsRestore = null;
			}
		}

		private void OnQueryInventoryError (int responseCode, Bundle skuDetails)
		{

			if (_buyTcs != null) {
				_buyTcs.TrySetResult (null);
				_buyTcs = null;
			}

			if (_tcs != null) {
				_tcs.TrySetResult (false);
				_tcs = null;
			}
		}

		private void OnPurchaseFailedValidation (Purchase purchase, string purchaseData, string purchaseSignature)
		{
			
			if (_buyTcs != null) {
				_buyTcs.TrySetResult (null);
				_buyTcs = null;
			}

			if (_tcsRestore != null) {
				_tcsRestore.TrySetResult (null);
				_tcsRestore = null;
			}
		}

		private void OnUserCanceled ()
		{
			if (_buyTcs != null) {
				_buyTcs.TrySetResult (null);
				_buyTcs = null;
			}

			if (_tcsRestore != null) {
				_tcsRestore.TrySetResult (null);
				_tcsRestore = null;
			}

			if (_tcs != null) {
				_tcs.TrySetResult (false);
				_tcs = null;
			}
		}

		private void InAppBillingProcesingError (string message)
		{
			
			if (_buyTcs != null) {
				_buyTcs.TrySetResult (null);
				_buyTcs = null;
			}

			if (_tcsRestore != null) {
				_tcsRestore.TrySetResult (null);
				_tcsRestore = null;
			}

			if (_tcs != null) {
				_tcs.TrySetResult (false);
				_tcs = null;
			}
		}

		private void OnPurchaseConsumedError (int responseCode, string token)
		{

			if (_buyTcs != null) {
				_buyTcs.TrySetResult (null);
				_buyTcs = null;
			}

			if (_tcsRestore != null) {
				_tcsRestore.TrySetResult (null);
				_tcsRestore = null;
			}
		}

		private void OnProductPurchasedError (int responseCode, string sku)
		{
			

			if (_buyTcs != null) {
				_buyTcs.TrySetResult (null);
				_buyTcs = null;
			}

			if (_tcsRestore != null) {
				_tcsRestore.TrySetResult (null);
				_tcsRestore = null;
			}
		}

		private void OnInvalidOwnedItemsBundleReturned (Bundle ownedItems)
		{
			

			if (_buyTcs != null) {
				_buyTcs.TrySetResult (null);
				_buyTcs = null;
			}

			if (_tcsRestore != null) {
				_tcsRestore.TrySetResult (null);
				_tcsRestore = null;
			}

			if (_tcs != null) {
				_tcs.TrySetResult (false);
				_tcs = null;
			}
		}

		private void OnGetProductsError (int responseCode, Bundle ownedItems)
		{
			

			if (_tcs != null) {
				_tcs.TrySetResult (false);
				_tcs = null;
			}

			if (_buyTcs != null) {
				_buyTcs.TrySetResult (null);
				_buyTcs = null;
			}

			if (_tcsRestore != null) {
				_tcsRestore.TrySetResult (null);
				_tcsRestore = null;
			}
		}

		private void OnPurchaseConsumed (string token)
		{
			if (_buyTcs != null) {
				_buyTcs.TrySetResult (token + "```" + _purchaseId);
				_buyTcs = null;
			}

			if (_tcsRestore != null) {
				_tcsRestore.TrySetResult (token + "```" + _purchaseId);
				_tcsRestore = null;
			}

			_purchaseId = string.Empty;
		}

		private void OnProductPurchased (int response, Purchase purchase, string purchaseData, string purchaseSignature)
		{
			if (!_serviceConnection.Connected) {
				if (_buyTcs != null) {
					_buyTcs.TrySetResult (null);
					_buyTcs = null;
				}
				return;
			}

			_purchaseId = purchase.ProductId;

			if (_buyTcs != null) {
				_buyTcs.TrySetResult (purchase.PurchaseToken);
			}
			//if (!_serviceConnection.BillingHandler.ConsumePurchase(purchase))
			//{
			//    if(_buyTcs != null)
			//    {
			//        _buyTcs.TrySetResult(null);
			//        _buyTcs = null;
			//    }
			//}
		}

		private async Task<bool> GetInventory ()
		{
			if (!_serviceConnection.Connected) {
				var b = await Connect ();
				if (!b) {
					//await Resolver.Resolve<INavigationService> ().DisplayAlertAsync (AppResources.NoConnection, string.Empty);
					_tcs.TrySetResult (false);
				}
			}
#if DEBUG
			var products = await _serviceConnection.BillingHandler.QueryInventoryAsync (new List<string>
			{
				PaidItem
			}, ItemType.Product);

#else
            var products = await _serviceConnection.BillingHandler.QueryInventoryAsync(new List<string> 
            {
				PaidItem
			}, ItemType.Product);
#endif


			if (products != null) {
				_products = products.ToList ();
			}

			if (_products.Count > 0) {
				var alreadyPurchased = _serviceConnection.BillingHandler.GetPurchases (ItemType.Product).ToList ();

				if (alreadyPurchased.Count > 0) {
					_alreadyPurchased = alreadyPurchased.FirstOrDefault ();
				}
			}
			return _products.Any ();
		}

		private Task<bool> Connect ()
		{
			_tcsConnection = new TaskCompletionSource<bool> ();
			_serviceConnection.Connect ();
			return _tcsConnection.Task;
		}

		public Task<string> RestoreProduct ()
		{
			_tcsRestore = new TaskCompletionSource<string> ();

			StartTimer ();

			return _tcsRestore.Task;
		}

		private bool Callback ()
		{
			if (_alreadyPurchased != null) {
				_purchaseId = _alreadyPurchased.ProductId;
				if (!_serviceConnection.BillingHandler.ConsumePurchase (_alreadyPurchased)) {
					_tcsRestore.TrySetResult (string.Empty);
				}
			} else {
				_tcsRestore.TrySetResult (string.Empty);
			}
			return false;
		}





		protected override void OnTimerTick (object sender, EventArgs e)
		{
			base.OnTimerTick (sender, e);
			if (_buyTcs != null) {
				if (_alreadyPurchased == null) {
					_buyTcs.TrySetResult (null);
				} else {
					_buyTcs.TrySetResult (_alreadyPurchased.PurchaseToken);
				}
			}

			if (_tcsRestore != null) {

				if (_alreadyPurchased != null) {
					_tcsRestore.TrySetResult (_alreadyPurchased.PurchaseToken);
				} else {
					_tcsRestore.TrySetResult (null);
				}
			}
		}

	}
}