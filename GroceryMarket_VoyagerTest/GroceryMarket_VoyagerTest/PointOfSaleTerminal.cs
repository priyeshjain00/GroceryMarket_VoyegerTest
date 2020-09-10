using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace GroceryMarket_Voyager
{
   public class PointOfSaleTerminal
   {
      List<IProduct> scannedProducts = new List<IProduct>();
      A itemAForPriceReference = new A();
      B itemBForPriceReference = new B();
      C itemCForPriceReference = new C();
      D itemDForPriceReference = new D();

      public void SetPricing(string productName, decimal unitPrice, int? bulkQuanity, decimal? bulkPrice)
      {
         switch (productName)
         {
            case "A":
               SetPricing(itemAForPriceReference, unitPrice, bulkQuanity, bulkPrice);
               break;

            case "B":
               SetPricing(itemBForPriceReference, unitPrice, bulkQuanity, bulkPrice);
               break;

            case "C":
               SetPricing(itemCForPriceReference, unitPrice, bulkQuanity, bulkPrice);
               break;

            case "D":
               SetPricing(itemDForPriceReference, unitPrice, bulkQuanity, bulkPrice);
               break;

            default:
               throw new InvalidDataException($"Sorry but object for this name {productName} does not exist");

         }
      }

      private void SetPricing(IProduct product, decimal unitPrice, int? bulkQuanity, decimal? bulkPrice)
      {
         // Check if only bulk price or bulk quantity is set
         if (((bulkQuanity == null || bulkQuanity==0.00m) && bulkPrice > 0.00m) || (bulkQuanity > 0 && (bulkPrice == null || bulkPrice == 0.00m)))
            throw new InvalidOperationException("Bulk quantity and bulk price both need to be set together and need to have bigger values than 0");

         product.UnitPrice = unitPrice;
         product.BulkPriceForQuantity = bulkQuanity.HasValue ? (int)bulkQuanity : 0;
         product.BulkPrice = bulkPrice.HasValue ? (int)bulkPrice : 0;
      }


      public void ScanProduct(string productName)
      {
         switch (productName)
         {
            case "A":
               scannedProducts.Add(itemAForPriceReference.ShallowCopy());
               break;

            case "B":
               scannedProducts.Add(itemBForPriceReference.ShallowCopy());
               break;

            case "C":
               scannedProducts.Add(itemCForPriceReference.ShallowCopy());
               break;

            case "D":
               scannedProducts.Add(itemDForPriceReference.ShallowCopy());
               break;

            default:
               throw new InvalidOperationException($"Apologies! but this product {productName} does not seem be existed. Please contact store assistant for further help");
         }
      }

      public decimal CalculateTotal()
      {
         var sumForItemA = getTotalSumForItem<A>(scannedProducts.OfType<A>().Count());
         var sumForItemB = getTotalSumForItem<B>(scannedProducts.OfType<B>().Count());
         var sumForItemC = getTotalSumForItem<C>(scannedProducts.OfType<C>().Count());
         var sumForItemD = getTotalSumForItem<D>(scannedProducts.OfType<D>().Count());

         scannedProducts.Clear(); // clear scanned items

         decimal totalSum = sumForItemA + sumForItemB + sumForItemC + sumForItemD;
         return Math.Round(totalSum, 2);
      }

      private decimal getTotalSumForItem<T>(int numberOfItem) where T : IProduct
      {
         if (numberOfItem <= 0)
            return 0.00m;

         var product = scannedProducts.OfType<T>().First();
         var unitPrice = product.UnitPrice;
         var bulkPrice = product.BulkPrice;
         var bulkPriceQuantity = product.BulkPriceForQuantity;

         if (bulkPriceQuantity > 1)
         {
            var totalPriceForItem = (numberOfItem % bulkPriceQuantity) * unitPrice; //total sum for items where Unitprice should be applied
            totalPriceForItem += (int)(numberOfItem / bulkPrice) * bulkPrice;  //unitPrice + sum for bulk quantity

            return totalPriceForItem;
         }

         return numberOfItem * unitPrice;
      }

      static void Main()
      {
         var terminal = new PointOfSaleTerminal();
         terminal.SetPricing("A", 1.25m, 3, 3.00m);
         terminal.SetPricing("B", 4.25m, null, null);
         terminal.SetPricing("C", 1, 6, 5.00m);
         terminal.SetPricing("D", 0.75m, null, null);
         terminal.ScanProduct("A");
         terminal.ScanProduct("B");
         terminal.ScanProduct("C");
         terminal.ScanProduct("D");
         terminal.ScanProduct("A");
         terminal.ScanProduct("B");
         terminal.ScanProduct("A");
         var totalSum = terminal.CalculateTotal();

         terminal.ScanProduct("C");
         terminal.ScanProduct("C");
         terminal.ScanProduct("C");
         terminal.ScanProduct("C");
         terminal.ScanProduct("C");
         terminal.ScanProduct("C");
         terminal.ScanProduct("C");
         totalSum = terminal.CalculateTotal();

         terminal.ScanProduct("A");
         terminal.ScanProduct("B");
         terminal.ScanProduct("C");
         terminal.ScanProduct("D");
         totalSum = terminal.CalculateTotal();

      }
   }
}
