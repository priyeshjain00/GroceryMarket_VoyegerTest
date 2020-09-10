using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryMarket_Voyager
{
   interface IProduct
   {
      string Name { get; set; }
      decimal UnitPrice { get; set; }
      int BulkPriceForQuantity { get; set; }
      decimal BulkPrice { get; set; }

   }

   class A : IProduct
   {
      public A() => Name = "A";

      public string Name { get; set; }
      public decimal UnitPrice { get; set; }
      public int BulkPriceForQuantity { get; set; }
      public decimal BulkPrice { get; set; }

      public A ShallowCopy() => (A)MemberwiseClone();

   }

   class B : IProduct
   {
      public B() => Name = "B";

      public string Name { get; set; }
      public decimal UnitPrice { get; set; }
      public int BulkPriceForQuantity { get; set; }
      public decimal BulkPrice { get; set; }

      public B ShallowCopy() => (B)MemberwiseClone();

   }

   class C : IProduct
   {
      public C() => Name = "C";
      public string Name { get; set; }
      public decimal UnitPrice { get; set; }
      public int BulkPriceForQuantity { get; set; }
      public decimal BulkPrice { get; set; }

      public C ShallowCopy() => (C)MemberwiseClone();

   }

   class D : IProduct
   {
      public D() => Name = "D";

      public string Name { get; set; }
      public decimal UnitPrice { get; set; }
      public int BulkPriceForQuantity { get; set; }
      public decimal BulkPrice { get; set; }

      public D ShallowCopy() => (D)MemberwiseClone();

   }

}
