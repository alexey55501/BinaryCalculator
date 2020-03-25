using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace BinaryCalculator.Classes.BL
{
    #region Enums
    public enum NumberingSchemes
    {
        AToZ,
        ZeroToZ
    }
    #endregion
    public class BaseConverter
    {
        #region Fields & constants
        private int _fromRadix;
        private string _fromNumberingScheme;
        private int _toRadix;
        private string _toNumberingScheme;
        private int _maxFromSchemeCharacter;
        #endregion
        #region Factory methods & constructors
        private BaseConverter(int fromRadix, NumberingSchemes fromScheme, int toRadix, NumberingSchemes toScheme)
        {
            #region Validate arguments
            if (fromRadix < 2 || fromRadix > 36)
                throw new ArgumentOutOfRangeException("fromRadix", "Radix can be from 2 to 36 inclusive");
            if (toRadix < 2 || toRadix > 36)
                throw new ArgumentOutOfRangeException("toRadix", "Radix can be from 2 to 36 inclusive");
            if (fromRadix > 26 && fromScheme == NumberingSchemes.AToZ)
                throw new ArgumentOutOfRangeException("fromRadix", "Invalid numbering scheme for specified number base");
            if (toRadix > 26 && fromScheme == NumberingSchemes.AToZ)
                throw new ArgumentOutOfRangeException("toRadix", "Invalid numbering scheme for specified number base");
            #endregion
            _fromRadix = fromRadix;
            _fromNumberingScheme = GetCharactersForNumberingScheme(fromScheme);
            _toRadix = toRadix;
            _toNumberingScheme = GetCharactersForNumberingScheme(toScheme);
            _maxFromSchemeCharacter = (fromScheme == NumberingSchemes.ZeroToZ) ? fromRadix : fromRadix + 1;
        }
        #endregion
        #region Public interface
        #region Class helpers
        public static string Convert(int fromRadix, int toRadix, string value)
        {
            return Convert(fromRadix, NumberingSchemes.ZeroToZ, toRadix, NumberingSchemes.ZeroToZ, value);
        }
        public static string Convert(int fromRadix, NumberingSchemes fromScheme, int toRadix, NumberingSchemes toScheme, string value)
        {
            BaseConverter BaseConverter = new BaseConverter(fromRadix, fromScheme, toRadix, toScheme);
            return BaseConverter.Convert(value);
        }
        #endregion
        #region Instance helpers
        public string Convert(string value)
        {
            #region Validate arguments
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException("value");
            #endregion
            long temp = ConvertToBase10(value, _fromRadix, _fromNumberingScheme, _maxFromSchemeCharacter);
            if (_toRadix == 10)
                return temp.ToString();
            else
                return ConvertFromBase10(temp, _toRadix, _toNumberingScheme);
        }
        #endregion
        #endregion
        #region Helpers
        private static string GetCharactersForNumberingScheme(NumberingSchemes scheme)
        {
            string characters;
            switch (scheme)
            {
                case NumberingSchemes.AToZ:
                    characters = "_ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                case NumberingSchemes.ZeroToZ:
                    characters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("scheme");
            }
            return characters;
        }
        private static long ConvertToBase10(string value, int fromBase, string characters, int maxFromSchemeCharacter)
        {
            StringBuilder fromValue = new StringBuilder(value);
            int power = 0;
            long result = 0;
            while (fromValue.Length > 0)
            {
                int index = Array.IndexOf<char>(characters.ToCharArray(), fromValue[fromValue.Length - 1]);
                if (index < 0)
                    throw new FormatException("Unsupported character in value string");
                if (index >= maxFromSchemeCharacter)
                    throw new FormatException("Value contains character not valid for number base");
                result += (index * (long)Math.Pow(fromBase, power));
                if (result < 0)
                    throw new OverflowException();
                fromValue.Length--;
                power++;
            }
            return result;
        }
        private static string ConvertFromBase10(long value, int toBase, string characters)
        {
            StringBuilder builder = new StringBuilder();
            while (value > 0)
            {
                int remainder = (int)(value % toBase);
                builder.Insert(0, characters[remainder]);
                value /= toBase;
            }
            return builder.ToString();
        }
        #endregion
    }
}
