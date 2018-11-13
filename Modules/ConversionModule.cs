//using Discord;
//using Discord.Commands;
//using ECommerceHelper.Common;
//using ECommerceHelper.CurrencyConverter;
//using Radon.Core;
//using System.Threading.Tasks;
//using UnitsNet;

//namespace Radon.Modules
//{
//    public class ConversionModule : CommandBase
//    {
//        //CurrencyService curr = new CurrencyService();
//        //[Command("CurrencyConvert")]
//        //[Alias("conv", "curr", "cur")]
//        //[Summary("Convert a currency to another")]
//        //public async Task CurrencyConvert(decimal amount, CurrencyCode fromCurr)
//        //{
//        //    await curr.ConvertAsync(amount, fromCurr);
//        //}
//        //public async Task CurrencyConvert(decimal amount, CurrencyCode fromCurr, CurrencyCode toCurr)
//        //{
//        //    await curr.ConvertAsync(amount, fromCurr, toCurr);
//        //}
//        [Command("Convert")]
//        public async Task Convert(string type, QuantityValue value, string fromUnit, string toUnit)
//        {
//            double result = 0;
//            if (!UnitConverter.TryConvertByName(value, type, fromUnit, toUnit, out result) && !UnitConverter.TryConvertByAbbreviation(value, type, fromUnit, toUnit, out result))
//            {
//                await ReplyEmbedAsync(new EmbedBuilder()
//                    .WithTitle("Error")
//                    .WithDescription("Error"));
//            }
//            else
//            {
//                await ReplyEmbedAsync(new EmbedBuilder()
//                    .WithTitle("Converted")
//                    .WithDescription($"{value.ToString()}{fromUnit} > {toUnit} = {result}"));
//            }
//        }
//    }
//}
