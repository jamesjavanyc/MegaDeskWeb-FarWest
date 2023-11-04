using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace MegaDeskWeb.Models
{
    public class Quote
    {

        public int Id { get; set; }

        [Display(Name = "Order Date")]
        [DataType(DataType.Date)]
        public DateTime DateQuote { get; set; }


        private int[,] _rushOrderPrices;

        [Display(Name = "Customer Name")]
        [Required]
        [StringLength(60)]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Depth must be specified")]
        [Range(12, 48, ErrorMessage = "Please enter a depth from 12 to 48 inches")]
        [DisplayName("Depth")]
        public decimal Depth { get; set; }

        [Required(ErrorMessage = "Width must be specified")]
        [Range(24, 96, ErrorMessage = "Please enter a width from 24 to 96 inches")]
        [DisplayName("Width")]
        public decimal Width
        {
            get; set;
        }
        [Display(Name = "Drawers")]
        [Range(0, 7, ErrorMessage = "Count is 0 - 7")]
        public int NumDrawers { get; set; }

        [Display(Name = "Material")]
        public DesktopMaterial Material { get; set; }

        [Display(Name = "Rush")]
        public RushOrder DeliveryOption { get; set; }

        [DisplayName("Price")]
        public decimal TotalPrice { get; set; }

    }

    public enum DesktopMaterial
    {
        Pine = 50,
        Laminate = 100,
        Oak = 200,
        Rosewood = 300,
        Veneer = 125
    };


    public enum RushOrder
    {
        [Description("Rush 3 Days")]
        Rush3Day,

        [Description("Rush 5 Days")]
        Rush5Day,

        [Description("Rush 7 Days")]
        Rush7Day,

        [Description("No Rush")]
        Normal14Day
    }

    public static class QuotePriceCalculator
    {
        private static decimal BASE_PRICE = 200;
        private static decimal MAX_BASE_SIZE = 1000;
        private static int PRICE_PER_SQUAREAREA = 1;
        private static int RUSH_LARGE_SIZE = 2000;
        private static int PRICE_PER_DRAWER = 50;
        private static int[,] RushDayPrices = new int[,] { { 60, 70, 80 }, { 40, 50, 60 }, { 30, 35, 40 } };
        public static decimal CalculateTotalPrice(Quote quote)
        {
            return BASE_PRICE + SurfaceAreaCost(quote) + DrawerCost(quote) + (int)quote.Material + RushOrderCost(quote);
        }

        private static decimal SurfaceAreaCost(Quote quote)
        {
            var surfaceArea = quote.Depth * quote.Width;
            if (surfaceArea > MAX_BASE_SIZE)
            {
                return (surfaceArea - MAX_BASE_SIZE) * PRICE_PER_SQUAREAREA;

            }
            return 0;
        }

        private static decimal DrawerCost(Quote quote)
        {
            return quote.NumDrawers * PRICE_PER_DRAWER;
        }

        private static decimal RushOrderCost(Quote quote)
        {
            if (quote.DeliveryOption == RushOrder.Normal14Day) return 0;

            decimal rushPrice = 0;
            var surfaceArea = quote.Depth * quote.Width;
            switch (quote.DeliveryOption)
            {
                case RushOrder.Rush3Day:
                    if (surfaceArea < MAX_BASE_SIZE)
                    {
                        rushPrice = RushDayPrices[0, 0];
                    }
                    else if (surfaceArea <= RUSH_LARGE_SIZE)
                    {
                        rushPrice = RushDayPrices[0, 1];
                    }
                    else
                    {
                        rushPrice = RushDayPrices[0, 2];
                    }
                    break;

                case RushOrder.Rush5Day:
                    if (surfaceArea < MAX_BASE_SIZE)
                    {
                        rushPrice = RushDayPrices[1, 0];
                    }
                    else if (surfaceArea <= RUSH_LARGE_SIZE)
                    {
                        rushPrice = RushDayPrices[1, 1];
                    }
                    else
                    {
                        rushPrice = RushDayPrices[1, 2];
                    }
                    break;

                case RushOrder.Rush7Day:
                    if (surfaceArea < MAX_BASE_SIZE)
                    {
                        rushPrice = RushDayPrices[2, 0];
                    }
                    else if (surfaceArea <= RUSH_LARGE_SIZE)
                    {
                        rushPrice = RushDayPrices[2, 1];
                    }
                    else
                    {
                        rushPrice = RushDayPrices[2, 2];
                    }
                    break;
            }
            return rushPrice;
        }
    }

}
