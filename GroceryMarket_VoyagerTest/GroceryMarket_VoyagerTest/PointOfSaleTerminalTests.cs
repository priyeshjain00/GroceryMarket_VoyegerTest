using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryMarket_Voyager
{
   [TestFixture]
   class PointOfSaleTerminalTest
   {
      [TestCase(new string[] { "A", "B", "C", "D", "A", "B", "A" }, 13.25)]
      [TestCase(new string[] { "C", "C", "C", "C", "C", "C", "C" }, 6.00)]
      [TestCase(new string[] { "A", "B", "C", "D" }, 7.25)]
      [TestCase(new string[] { "D", "B", "A", "D", "B", "B", "A", "C", "A" }, 18.25)]
      public void ScanSomeProducts_CalculatePrice_VerifyUnitPriceAndBulkPriceAppliedAccordingly(string[] listOfItemsToBeScanned, decimal expectedTotalSum)
      {
         var terminal = new PointOfSaleTerminal();
         terminal.SetPricing("A", 1.25m, 3, 3.00m);
         terminal.SetPricing("B", 4.25m, null, null);
         terminal.SetPricing("C", 1, 6, 5.00m);
         terminal.SetPricing("D", 0.75m, null, null);

         foreach (var item in listOfItemsToBeScanned)
            terminal.ScanProduct(item);

         Assert.AreEqual(expectedTotalSum, terminal.CalculateTotal(), "Calculated price should be based on bulk price and unit price specified");
      }


      [TestCase("A", 1.25, 3, 3.00)]
      [TestCase("B", 4.25, null, null)]
      [TestCase("C", 1, 6, 5.00)]
      [TestCase("D", 0.75, null, null)]
      public void SetPricing_ScanSingleUnitOfItem_CheckCalculatedPriceIsUnitPrice(string productName, decimal unitPrice, int bulkQuantity, decimal bulkPrice)
      {
         var terminal = new PointOfSaleTerminal();
         terminal.SetPricing(productName, unitPrice, bulkQuantity, bulkPrice);

         terminal.ScanProduct(productName);
         Assert.AreEqual(unitPrice, terminal.CalculateTotal(), "Total Price for the scanned item should be equal to its unit price");
      }

      [Test]
      public void ScanProdcutThatDoesNotExistWithSystem_CheckInvalidOperationExceptionThrown()
      {
         var terminal = new PointOfSaleTerminal();

         Assert.Throws<InvalidOperationException>(() => terminal.ScanProduct("E"));
      }


      [Test]
      public void SetPriceOfAProdcutThatDoesNotExistWithSystem_CheckInvalidDataExceptionThrown()
      {
         var terminal = new PointOfSaleTerminal();

         Assert.Throws<System.IO.InvalidDataException>(() => terminal.SetPricing("E", 1.00m, 6, 4.00m));
      }


      [TestCase("A", 1.25, 3, 3.00)]
      [TestCase("C", 1, 6, 5.00)]
      public void SetPricing_ScanBulkQuantityOfItem_CheckCalculatedPriceIsBulkPrice(string productName, decimal unitPrice, int bulkQuantity, decimal bulkPrice)
      {
         var terminal = new PointOfSaleTerminal();
         terminal.SetPricing(productName, unitPrice, bulkQuantity, bulkPrice);

         for (var itemCount = 0; itemCount < bulkQuantity; itemCount++)
            terminal.ScanProduct(productName);

         Assert.AreEqual(bulkPrice, terminal.CalculateTotal(), "Total Price for the scanned item should be equal to its bulk price");
      }


      [TestCase("B", 4.25, null, null)]
      [TestCase("D", 0.75, null, null)]
      public void DoNotSetBulkPricing_Scan10Items_CheckCalculatedPriceIsbasedOnUnitPrice(string productName, decimal unitPrice, int bulkQuantity, decimal bulkPrice)
      {
         var terminal = new PointOfSaleTerminal();
         terminal.SetPricing(productName, unitPrice, bulkQuantity, bulkPrice);

         for (var itemCount = 0; itemCount < 10; itemCount++)
            terminal.ScanProduct(productName);

         Assert.AreEqual(unitPrice * 10, terminal.CalculateTotal(), "Total Price for the scanned item should be equal to its ten times of its 10 times of unit price");
      }

      [TestCase("D", 0.75, 5, null)]
      [TestCase("D", 0.75, null, 5)]
      [TestCase("D", 0.75, 0, 10)]
      [TestCase("D", 0.75, 10, 0)]

      public void SetInvalidBulkPricing_VerifyThatSystemThrowsInvalidOperationException(string productName, decimal unitPrice, int bulkQuantity, decimal bulkPrice)
      {
         var terminal = new PointOfSaleTerminal();

         Assert.Throws<InvalidOperationException>(() => terminal.SetPricing(productName, unitPrice, bulkQuantity, bulkPrice));
      }
   }
}
