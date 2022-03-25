using Marvelous.AccountCheckingByChuZhig.BLL.Models;
using System.Net;
using AngleSharp;
using System.Xml;
using System.Xml.Serialization;
using System.Data;
using System.Text;
using Marvelous.Contracts.Enums;

namespace Marvelous.AccountCheckingByChuZhig.BLL.Helpers
{
    public class CurrencyParser
    {
        public CurrencyParser()
        {
        }

        public decimal ParseCurrencyAmountTo(Currency currencyFrom, Currency currencyTo, DateTime date)
        {
            decimal amountFromInRub = GetRateInRub(currencyFrom, date);
            decimal amountToInRub = GetRateInRub(currencyTo, date);

            decimal result = amountFromInRub / amountToInRub;
            return result;
            
        }

        public decimal GetRateInRub(Currency currency, DateTime date)
        {
            if (currency == Currency.RUB)
                return 1m;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            string url = $"http://www.cbr.ru/scripts/XML_daily.asp?date_req={date.ToString("d")}";
            //XmlDocument xml_doc = new XmlDocument();
            //xml_doc.Load(url);
            DataSet ds = new DataSet();
            ds.ReadXml(url);
            DataTable currencies = ds.Tables["Valute"];
            foreach (DataRow row in currencies.Rows)
            {
                if (row["CharCode"].ToString() == currency.ToString())//Ищу нужный код валюты
                {
                    return Convert.ToDecimal(row["Value"])/ Convert.ToDecimal(row["Nominal"]); //Возвращаю значение курсы валюты
                }
            }
            return 0m;
        }

    }
}
