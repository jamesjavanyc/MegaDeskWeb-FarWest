using MegaDeskWeb.Data;
using Microsoft.EntityFrameworkCore;
using static MegaDeskWeb.Models.Quote;

namespace MegaDeskWeb.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new MegaDeskWebContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<MegaDeskWebContext>>()))
            {
                // Look for any desks.
                if (context.Quote.Any())
                {
                    return;   // DB has been seeded
                }

                context.Quote.AddRange(
                    new Quote
                    {
                        DateQuote = DateTime.Now,
                        CustomerName = "Harry Potter",
                        Width = 65,
                        Depth = 45,
                        NumDrawers = 2,
                        Material = DesktopMaterial.Pine,
                        DeliveryOption = RushOrder.Rush5Day,
                        TotalPrice = 500
                    },
                    new Quote
                    {
                        DateQuote = DateTime.Now,
                        CustomerName = "L3 Harris",
                        Width = 65,
                        Depth = 45,
                        NumDrawers = 2,
                        Material = DesktopMaterial.Pine,
                        DeliveryOption = RushOrder.Rush5Day,
                        TotalPrice = 500
                    }

                );
                context.SaveChanges();
            }
        }
    }
}
