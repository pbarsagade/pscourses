using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Data
{
    public static class DataContextExtension
    {
        public static void DataContextSeed(this DataContext context)
        {
            context.Addresses.RemoveRange(context.Addresses);
            context.SaveChanges();

            List<Address> addresses = new List<Address>();

            addresses.AddRange(new List<Address> {
                        new Address
                        {
                            Line1 ="Test line 1",
                            Line2 ="Test line 2",
                            Phone = "234523452345"
                        },
                        new Address
                        {
                            Line1 ="Test 2 line 1",
                            Line2 ="Test 2 line 2",
                            Phone = "252352323"
                        }
                    }
                );
            context.Addresses.AddRange(addresses);
            context.SaveChanges();
        }
    }
}
