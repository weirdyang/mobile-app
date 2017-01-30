namespace LH.Forcas.Domain.UserData
{
    using System;
    using System.Text.RegularExpressions;

    public struct AccountNumber
    {
        private static readonly Regex ParseRegex = new Regex("(?:(?<prefix>\\d+)-)?(?<number>\\d+)/(?<code>\\d{4})");

        public static AccountNumber Parse(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("The input value cannot be empty.", nameof(value));
            }

            var match = ParseRegex.Match(value);

            if (!match.Success)
            {
                throw new ArgumentException("The input value does not match the expected format.", nameof(value));
            }

            var result = new AccountNumber();
            result.BankRoutingCode = int.Parse(match.Groups["code"].Value);
            result.Number = long.Parse(match.Groups["number"].Value);

            var prefix = match.Groups["prefix"].Value;
            if (!string.IsNullOrEmpty(prefix))
            {
                result.Prefix = int.Parse(prefix);
            }

            return result;
        }

        public int Prefix { get; set; }

        public long Number { get; set; }

        public int BankRoutingCode { get; set; }

        public override string ToString()
        {
            if (this.Prefix != default(int))
            {
                return $"{this.Prefix}-{this.Number}/{this.BankRoutingCode}";
            }

            return $"{this.Number}/{this.BankRoutingCode}";
        }

        public bool Equals(AccountNumber other)
        {
            return this.BankRoutingCode == other.BankRoutingCode && this.Number == other.Number && this.Prefix == other.Prefix;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AccountNumber && this.Equals((AccountNumber) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                long hashCode = this.BankRoutingCode;
                hashCode = (hashCode*397) ^ this.Number;
                hashCode = (hashCode*397) ^ this.Prefix;
                return (int)hashCode;
            }
        }

        public static bool operator ==(AccountNumber left, AccountNumber right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(AccountNumber left, AccountNumber right)
        {
            return !left.Equals(right);
        }
    }
}