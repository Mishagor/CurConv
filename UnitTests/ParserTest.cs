namespace UnitTests
{
    using webbapp.Controllers.Data;

    public class ParserTest
    {
        [Theory]
        [InlineData("-1")]
        [InlineData("1000000001")]
        public void OutOfRangeTest(string input)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => Parser.Parse(input));
        }

        [Theory]
        [InlineData("1A")]
        [InlineData("2.54")]
        [InlineData("2,5454")]
        [InlineData("28,230,54")]
        public void InvalidInputTest(string input)
        {
            Assert.Throws<ArgumentException>(() => Parser.Parse(input));
        }

        [Theory]
        [InlineData("", "")]
        [InlineData("0", "zero dollars")]
        [InlineData("1", "one dollar")]
        [InlineData("25,1", "twenty-five dollars and ten cents")]
        [InlineData("0,01", "zero dollars and one cent")]
        [InlineData("45 100", "forty-five thousand one hundred dollars")]
        [InlineData("45100", "forty-five thousand one hundred dollars")]
        [InlineData("999 999 999,99", "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-nine cents")]
        [InlineData("999999999,91", "nine hundred ninety-nine million nine hundred ninety-nine thousand nine hundred ninety-nine dollars and ninety-one cent")]
        [InlineData("800800800,80", "eight hundred million eight hundred thousand eight hundred dollars and eighty cents")]
        [InlineData("70 007 000,00", "seventy million seven thousand dollars")]
        [InlineData("06 000 606,06", "six million six hundred six dollars and six cents")]
        [InlineData("55 000 000", "fifty-five million dollars")]
        [InlineData("044 000", "forty-four thousand dollars")]
        [InlineData("300", "three hundred dollars")]
        [InlineData("21", "twenty-one dollar")]
        [InlineData("15,", "fifteen dollars")]
        [InlineData(",34", "zero dollars and thirty-four cents")]
        [InlineData("1,2", "one dollar and twenty cents")]

        public void CasesTest(string input, string expctd)
        {
            Assert.Equal(expctd, Parser.Parse(input));
        }
    }
}