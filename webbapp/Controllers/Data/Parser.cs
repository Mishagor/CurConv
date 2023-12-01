using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;

namespace webbapp.Controllers.Data
{
    /// <summary>
    /// Present text as number and converts it to string named representation.
    /// </summary>
    public static class Parser
    {
        /// <summary>
        /// Split string on million, thousand, hundred, ten and cent categories and convert the splits to string representation.
        /// </summary>
        /// <param name="input"><see cref="string"/> input number.</param>
        /// <returns><see cref="string"/> containing word discription of the number.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para>number is less than 0.</para>
        /// <para>-or-</para>
        /// <para>number is greater than 999 999 999,99.</para>
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para>number contains invalid symbols.</para>
        /// </exception>
        public static string Parse(string input)
        {            
            if (string.IsNullOrEmpty(input))
            {
                return string.Empty;
            }

            // preprocess
            input = input.Replace(" ", "").TrimStart('0');
            if (input.Length == 0)
            {
                return "zero dollars";
            }

            // Out of range validation 
            if (input.Length > 13 || input[0] == '-')
            {
                throw new ArgumentOutOfRangeException(nameof(input));
            }
            
            SplittedNumber sn = SplittedNumber.SplitString(input);
            return sn.ToString();

        }

        private class SplittedNumber
        {
            private const string Million = "million";
            private const string Hundred = "hundred";
            private const string Thousand = "thousand";
            private const string Dollars = "dollars";
            private const string Dollar = "dollar";
            private const string Cents = "cents";
            private const string Cent = "cent";

            private static string[] digits = new string[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
            private static string[] c2d = new string[100];

            private static int shift = 0;

            // Hundreds of millions
            private int? millHund = null;

            // Tens of millions
            private int? millTens = null;

            // Hundreds of thousands
            private int? thHund = null;

            // Tens of thousands
            private int? thTens = null;

            // Hundreds
            private int? hundred = null;

            // Tens
            private int? tens = null;

            // Cents
            private int? cents = null;

            static SplittedNumber()
            {
                // initialize two-digits converter
                string[] decs = new string[]
                {
                "twenty",
                "thirty",
                "forty",
                "fifty",
                "sixty",
                "seventy",
                "eighty",
                "ninety",
                };


                // 0-9
                for (int i = 0; i < digits.Length; i++)
                {
                    c2d[i] = digits[i];
                }

                // 10-19
                c2d[10] = "ten";
                c2d[11] = "eleven";
                c2d[12] = "twelve";
                c2d[13] = "thirteen";
                c2d[14] = "fourteen";
                c2d[15] = "fifteen";
                c2d[16] = "sixteen";
                c2d[17] = "seventeen";
                c2d[18] = "eighteen";
                c2d[19] = "nineteen";

                // 20-99
                for (int dec = 0; dec < decs.Length; dec++)
                {
                    c2d[(dec + 2) * 10] = decs[dec];
                    for (int u = 1; u < digits.Length; u++)
                    {
                        c2d[(dec + 2) * 10 + u] = string.Format(CultureInfo.InvariantCulture, "{0}-{1}", decs[dec], digits[u]);
                    }
                }
            }

            /// <summary>
            /// Prevent from creating the default <see cref="SplittedNumber"/>.
            /// </summary>
            private SplittedNumber() { }

            public static SplittedNumber SplitString(string s)
            {
                int idx = s.IndexOf(",");
                SplittedNumber sn = new SplittedNumber();
                if (idx >= 0)
                {
                    // Determine cents part
                    if (idx < s.Length - 3)
                    {
                        throw new ArgumentException("too many digits in cents part.");
                    }
                    else if (idx < s.Length - 1)
                    {
                        string scents = s.Substring(idx + 1);
                        if (scents.Length == 1)
                        {
                            scents += "0";
                        }

                        sn.cents = SplittedNumber.ConvertToDigits(scents);
                    }

                    if (idx == 0)
                    {
                        // Without dollars
                        return sn;
                    }

                    // Extract dollars
                    s = s.Substring(0, idx);                    
                }

                if (s.Length > 9)
                {
                    throw new ArgumentOutOfRangeException();
                }

                SplittedNumber.shift = 0;

                // Before 1000
                SplittedNumber.ProcessNext3Digits(s, (hundred, tens) => { sn.tens = tens; sn.hundred = hundred; });
                
                // Thousand
                SplittedNumber.ProcessNext3Digits(s, (hundred, tens) => { sn.thTens = tens; sn.thHund = hundred; });

                // Million
                SplittedNumber.ProcessNext3Digits(s, (hundred, tens) => { sn.millTens = tens; sn.millHund = hundred; });
     
                return sn;
            }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                
                // Million
                if (this.millHund.HasValue)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, "{0} {1} ", SplittedNumber.GetString(this.millHund.Value), SplittedNumber.Hundred);
                }

                if (this.millTens.HasValue)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, "{0} {1} ", SplittedNumber.GetString(this.millTens.Value), SplittedNumber.Million);
                }
                else if (this.millHund.HasValue)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, "{0} ", SplittedNumber.Million);
                }

                // Thousand
                if (this.thHund.HasValue)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, "{0} {1} ", SplittedNumber.GetString(this.thHund.Value), SplittedNumber.Hundred);
                }

                if (this.thTens.HasValue)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, "{0} {1} ", SplittedNumber.GetString(this.thTens.Value), SplittedNumber.Thousand);
                }
                else if (this.thHund.HasValue)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, "{0} ", SplittedNumber.Thousand);
                }

                // Units
                if (this.hundred.HasValue)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, "{0} {1} ", SplittedNumber.GetString(this.hundred.Value), SplittedNumber.Hundred);
                }

                if (this.tens.HasValue)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, "{0} ", SplittedNumber.GetString(this.tens.Value));
                }

                if (sb.Length == 0)
                {
                    sb.Append("zero ");
                }

                if (sb[sb.Length - 2] == 'e' && sb[sb.Length - 4] == 'o')
                {
                    // Last digit is one
                    sb.AppendFormat(SplittedNumber.Dollar);
                }
                else
                {
                    sb.AppendFormat(SplittedNumber.Dollars);
                }

                // cents
                if (this.cents.HasValue)
                {
                    sb.AppendFormat(CultureInfo.CurrentCulture, " and {0} ", SplittedNumber.GetString(this.cents.Value));
                    if (sb[sb.Length - 2] == 'e' && sb[sb.Length - 4] == 'o')
                    {
                        // Last digit is one
                        sb.AppendFormat(SplittedNumber.Cent);
                    }
                    else
                    {
                        sb.AppendFormat(SplittedNumber.Cents);
                    }
                }

                return sb.ToString();
            }

            private static int? ConvertToDigits(string s) 
            {
                int res = 0;
                if (!int.TryParse(s, out res))
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "non digit symbol in \"{0}\"", s));
                }

                return res != 0 ? res : null;
            }

            private static void ProcessNext3Digits(string s, Action<int?, int?> setter)
            {
                // Start position for tens
                int startTen = s.Length - 2 - shift;
                if (startTen < -1)
                {
                    // string too short
                    setter(null, null);
                }
                else if (startTen == -1)
                {
                    // there is only one digit
                    setter(null, SplittedNumber.ConvertToDigits(s.Substring(0, 1)));
                }
                else
                {
                    // two digits tens and maybe hundred
                    setter(
                        startTen > 0 ? SplittedNumber.ConvertToDigits(s.Substring(startTen - 1, 1)) : null,
                        SplittedNumber.ConvertToDigits(s.Substring(startTen, 2)));
                }

                // Move to next 3 digits
                shift += 3;
            }           

            private static string GetString(int number)
            {
                return SplittedNumber.c2d[number];
            }
        }
    }
}
