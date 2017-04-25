using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using biz.dfch.CS.CoffeeTracker.Client.CoffeeTrackerService;

namespace biz.dfch.CS.CoffeeTracker.Client.Wpf.Classes.Managers
{
    public static class CoffeeOrderEnumerableExtensions
    {
        //public FilterManager(IEnumerable<CoffeeOrder> coffeeOrders)
        //{
        //    CoffeeOrders = coffeeOrders;
        //}

        public static IEnumerable<CoffeeOrder> ApplyUserFilter(this IEnumerable<CoffeeOrder> orders,
            params ApplicationUser[] users)
        {
            Contract.Requires(0 < users.Length);
            Contract.Requires(null != users[0]);
            Contract.Requires(null != orders);

            IEnumerable<CoffeeOrder> filterdEnumerable = null;

            foreach (var applicationUser in users)
            {
                filterdEnumerable =
                    orders.Where(u => u.ApplicationUser.Name.Equals(applicationUser.Name)).AsEnumerable();
            }

            return filterdEnumerable;
        }

        public static IEnumerable<CoffeeOrder> ApplyTimeFilter(this IEnumerable<CoffeeOrder> orders, DateTimeOffset from,
            DateTimeOffset until)
        {
            Contract.Requires(null != @from);
            Contract.Requires(null != until);
            Contract.Requires(@from <= until);

            return orders.Where(c => c.Created >= @from && c.Created <= until);
        }

        public static IEnumerable<CoffeeOrder> ApplyCoffeeFilter(this IEnumerable<CoffeeOrder> orders, params Coffee[] coffees)
        {
            Contract.Requires(0 < coffees.Length);
            Contract.Requires(null != coffees[0]);

            IEnumerable<CoffeeOrder> filterdEnumerable = null;

            foreach (var coffee in coffees)
            {
                filterdEnumerable = orders.Where(u => u.Coffee.Name.Equals(coffee.Name)
                                                      && u.Coffee.Brand.Equals(coffee.Brand))
                    .AsEnumerable();
            }

            return filterdEnumerable;
        }
    }
}