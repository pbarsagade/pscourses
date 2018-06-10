using restapi3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restapi3.Entities
{
    public static class DataContextExtensions
    {
        public static void EnsureSeedDataForContext(this DataContext context)
        {
            context.Responses.RemoveRange(context.Responses);
            context.SaveChanges();


            List<GuestResponse> responses = new List<GuestResponse>();

            responses.AddRange(new List<GuestResponse> {
                        new GuestResponse
                        {
                            Email = "testc@mail.com",
                            Name = "Guest1",
                            Phone = "23452345234",
                            WillAttend = false
                        },
                        new GuestResponse
                        {
                            Email = "testNew@mail.com",
                            Name = "Guest2",
                            Phone = "2345234",
                            WillAttend = true
                        }
                    }
                );
            context.Responses.AddRange(responses);
            context.SaveChanges();
        }
    }
}
