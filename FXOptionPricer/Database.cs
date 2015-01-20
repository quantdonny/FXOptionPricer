using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace FXOptionPricer
{
    public static class Database
    {

        public static List<OHLCBar> getOHLCForCcy(String inCcy)
        {
            List<OHLCBar> aListOfOHLC = new List<OHLCBar>();
            String FileName = inCcy + "Daily.txt";
            StreamReader aReader = new StreamReader(FileName);
            String aLine = "";
            while ((aLine = aReader.ReadLine()) != null)
            {
                String[] StringArray = aLine.Split(',');
                DateTime thisDateTime = System.Convert.ToDateTime(StringArray[1]);
                double Open = System.Convert.ToDouble(StringArray[3]);
                double High = System.Convert.ToDouble(StringArray[4]);
                double Low = System.Convert.ToDouble(StringArray[5]);
                double Close = System.Convert.ToDouble(StringArray[6]);
                aListOfOHLC.Add( new OHLCBar(Open, High, Low, Close, thisDateTime ));
            }
            return aListOfOHLC;
        }

        public static double getInitialValueForCcy(String inCcy)
        {
            if (inCcy == "EURUSD") { return 1.25; }
            if (inCcy == "GBPUSD") { return 1.55; }
            if (inCcy == "USDJPY") { return 120.00; }
            if (inCcy == "USDCHF") { return 0.95; }
            if (inCcy == "AUDUSD") { return 0.85; }
            return 0.0;
        }

        public static double getTickValueForCcy(String inCcy)
        {
            if (inCcy == "USDJPY") { return 0.01; }
            return 0.0001;
        }
        
    }

    public static class SmileCalc
    {

        public static List<KeyValuePair<double, double>> getSmile(List<KeyValuePair<double, double>> inStrikeToPrice, Dictionary<double, List<KeyValuePair<double, double>>> inVolToPriceList)
        {
            List<KeyValuePair<double, double>> myStrikeToVol = new List<KeyValuePair<double, double>>();
            for (int i = 0; i < inStrikeToPrice.Count; i++)
            {
                double thisStrike = inStrikeToPrice[i].Key;
                double CurrentVol = 0;
                double CurrentDifference = Double.NaN;
                foreach (KeyValuePair<double, List<KeyValuePair<double, double>>> VolToPriceList in inVolToPriceList)
                {
                    if (Double.IsNaN(CurrentDifference) == true)
                    {
                        CurrentVol = VolToPriceList.Key;
                        CurrentDifference = Math.Abs(VolToPriceList.Value[i].Value - inStrikeToPrice[i].Value);
                    }
                    double NewDifference = Math.Abs(VolToPriceList.Value[i].Value - inStrikeToPrice[i].Value);
                    if (NewDifference < CurrentDifference)
                    {
                        CurrentDifference = NewDifference;
                        CurrentVol = VolToPriceList.Key;
                    }
                }
                myStrikeToVol.Add(new KeyValuePair<double, double>(thisStrike, CurrentVol));
            }
            return myStrikeToVol;
        }
    }

    public class GenericFactory<T>
    {
        private Dictionary<String, Type> RegisteredProducts;

        public GenericFactory(String inFolderName, String inBaseName)
        {
            RegisteredProducts = new Dictionary<String, Type>();
            Type[] aListOfStrategies = Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, inFolderName, StringComparison.Ordinal)).ToArray();
            foreach (Type aType in aListOfStrategies)
            {
                if( aType.BaseType.Name == inBaseName )
                    if (RegisteredProducts.ContainsKey(aType.Name) == false)
                        RegisteredProducts.Add(aType.Name, aType);
            }

        }

        public Dictionary<String, T> getDictionaryOfNameToProducts()
        {
            Dictionary<String, T> myDictOfNameToProducts = new Dictionary<String, T>();
            foreach (KeyValuePair<String, Type> aNameToTypePair in RegisteredProducts)
            {
                ConstructorInfo StrategyConstructor = aNameToTypePair.Value.GetConstructor(new Type[0]);
                T ThisObject = (T)StrategyConstructor.Invoke(new Object[0]);
                String thisName = (String)ThisObject.GetType().GetField("Name").GetValue(ThisObject);
                myDictOfNameToProducts.Add(thisName, ThisObject);
            }
            return myDictOfNameToProducts;        
        }

    }

}
