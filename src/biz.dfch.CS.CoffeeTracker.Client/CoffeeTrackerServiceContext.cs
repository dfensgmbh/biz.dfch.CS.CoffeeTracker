using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace biz.dfch.CS.CoffeeTracker.Client
{
    public class CoffeeTrackerServiceContext
    {
        private Uri uri;
        protected CoffeeTrackerService.Container container;

        public CoffeeTrackerServiceContext(string uri)
        {
            this.uri = new Uri(uri);
            this.container = new CoffeeTrackerService.Container(this.uri);

            container.SendingRequest2 += (s, e) =>
            {
                Console.WriteLine("Http Request sent");
            };
        }

        public CoffeeTrackerService.Coffee GetCoffee(long id)
        {
            Contract.Requires(0 < id);

            var coffee = container.Coffees.Where(c => c.Id == id).FirstOrDefault();

            return coffee;
        }
    }
}
