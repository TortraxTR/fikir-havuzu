namespace api.Validation
{
    // T.C. Kimlik No checksum (the standard 11-digit algorithm): the 10th digit is derived
    // from the odd/even position sums of the first 9 digits, and the 11th is a checksum
    // over the first 10. Rejects anything that isn't a real, well-formed number, not just
    // an 11-character string.
    public static class TcKimlikNoValidator
    {
        public static bool IsValid(string? value)
        {
            if (value == null || value.Length != 11 || value[0] == '0')
            {
                return false;
            }

            Span<int> digits = stackalloc int[11];
            for (var i = 0; i < 11; i++)
            {
                if (!char.IsAsciiDigit(value[i]))
                {
                    return false;
                }

                digits[i] = value[i] - '0';
            }

            var oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
            var evenSum = digits[1] + digits[3] + digits[5] + digits[7];

            var expectedTenth = ((oddSum * 7) - evenSum) % 10;
            if (expectedTenth < 0)
            {
                expectedTenth += 10;
            }

            if (expectedTenth != digits[9])
            {
                return false;
            }

            var expectedEleventh = (oddSum + evenSum + digits[9]) % 10;
            return expectedEleventh == digits[10];
        }
    }
}
