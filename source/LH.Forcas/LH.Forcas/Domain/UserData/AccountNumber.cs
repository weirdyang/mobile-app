namespace LH.Forcas.Domain.UserData
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    public struct AccountNumber
    {
        private static readonly IDictionary<string, Func<string, AccountNumber>> IBANParsers = new Dictionary<string, Func<string, AccountNumber>>
        {
            { "CZ", FromCzIban }
        };

        public static AccountNumber FromIban(string iban)
        {
            if (string.IsNullOrEmpty(iban))
            {
                throw new ArgumentNullException(nameof(iban));
            }

            if (iban.Length < 4)
            {
                throw new FormatException("The IBAN has to be at least 4 characters long.");
            }

            if (!IsIbanValid(iban))
            {
                throw new FormatException($"The iban {iban} is invalid (checksum validation failed).");
            }

            Func<string, AccountNumber> parserFunc;
            var prefix = iban.Substring(0, 2).ToUpperInvariant();

            if (!IBANParsers.TryGetValue(prefix, out parserFunc))
            {
                throw new NotSupportedException($"The IBAN prefix {prefix} is not supported.");
            }

            return parserFunc.Invoke(iban);
        }

        public static bool IsIbanValid(string iban)
        {
            return CalculateIbanRemainder(iban) == 1;
        }

        #region Czech Republic

        private static readonly Regex CzLocalRegex = new Regex("(?:(?<prefix>\\d+)-)?(?<number>\\d+)/(?<bban>\\d{4})", RegexOptions.CultureInvariant);
        private static readonly Regex CzIbanRegex = new Regex("CZ(\\d{2})(?<bban>\\d{4})(?<prefix>\\d{6})(?<number>\\d{10})", RegexOptions.CultureInvariant);

        public static AccountNumber FromCzLocal(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException(nameof(value), "The input value cannot be empty.");
            }

            var match = CzLocalRegex.Match(value);

            if (!match.Success)
            {
                throw new FormatException("The input value does not match the expected format.");
            }

            var prefix = match.Groups["prefix"].Value;
            var number = match.Groups["number"].Value;
            var bban = match.Groups["bban"].Value;

            var iban = ToCzIban(prefix, number, bban);
            var localFormat = ToCzLocal(prefix, number, bban);

            return new AccountNumber(iban, localFormat);
        }

        private static AccountNumber FromCzIban(string iban)
        {
            if (string.IsNullOrEmpty(iban))
            {
                throw new ArgumentNullException(nameof(iban), "The input value cannot be empty.");
            }

            var match = CzIbanRegex.Match(iban);

            if (!match.Success)
            {
                throw new ArgumentException("The input value does not match the expected format.", nameof(iban));
            }

            var prefix = match.Groups["prefix"].Value;
            var number = match.Groups["number"].Value;
            var bban = match.Groups["bban"].Value;

            var localFormat = ToCzLocal(prefix, number, bban);

            return new AccountNumber(iban, localFormat);
        }

        private static string ToCzLocal(string prefix, string number, string bban)
        {
            if (!string.IsNullOrEmpty(prefix) && !string.IsNullOrEmpty(prefix.TrimStart('0')))
            {
                return $"{prefix}-{number}/{bban}";
            }

            return $"{number}/{bban}";
        }

        private static string ToCzIban(string prefix, string number, string bban)
        {
            var iban = $"CZ00{bban}{prefix.PadLeft(6, '0')}{number.PadLeft(10, '0')}";

            return CalculateIbanChecksum(iban);
        }

        #endregion

        private AccountNumber(string iban, string localFormat)
        {
            this.Iban = iban;
            this.LocalFormat = localFormat;
        }

        public string Iban { get; }

        public string LocalFormat { get; }

        public override string ToString()
        {
            return $"IBAN: {this.Iban}, Local: {this.LocalFormat}";
        }

        public bool Equals(AccountNumber other)
        {
            return this.Iban == other.Iban;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AccountNumber && this.Equals((AccountNumber)obj);
        }

        public override int GetHashCode()
        {
            return this.Iban.GetHashCode();
        }

        public static bool operator ==(AccountNumber left, AccountNumber right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AccountNumber left, AccountNumber right)
        {
            return !left.Equals(right);
        }

        private static string CalculateIbanChecksum(string zeroIban)
        {
            var checkDigits = 98 - CalculateIbanRemainder(zeroIban);

            var builder = new StringBuilder(zeroIban);
            builder[2] = (char)(checkDigits/10+'0');
            builder[3] = (char)(checkDigits%10+'0');

            return builder.ToString();
        }

        private static int CalculateIbanRemainder(string iban)
        {
            // Code in this method was inspired by code created DvdKhl (https://gist.github.com/DvdKhl/6139665), released under MIT License

            if (iban.Length < 4)
            {
                throw new FormatException();
            }

            var checksum = 0;

            for (var i = 0; i < iban.Length; i++)
            {
                int value;

                var c = iban[(i + 4) % iban.Length];

                if (char.IsDigit(c))
                {
                    value = c - '0';
                }
                else if (char.IsLetter(c) && char.IsUpper(c))
                {
                    value = c - 'A';
                    checksum = (checksum * 10 + (value / 10 + 1)) % 97;
                    value %= 10;
                }
                else if (char.IsLetter(c) && char.IsLower(c))
                {
                    value = c - 'a';
                    checksum = (checksum * 10 + (value / 10 + 1)) % 97;
                    value %= 10;
                }
                else
                {
                    throw new FormatException();
                }

                checksum = (checksum * 10 + value) % 97;
            }

            return checksum;
        }
    }
}