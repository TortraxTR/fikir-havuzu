// Mirrors api/Validation/TcKimlikNoValidator.cs — the standard T.C. Kimlik No checksum.
export function isValidTcKimlikNo(value: string): boolean {
	if (value.length !== 11 || value[0] === '0' || !/^\d{11}$/.test(value)) {
		return false;
	}

	const digits = value.split('').map(Number);

	const oddSum = digits[0] + digits[2] + digits[4] + digits[6] + digits[8];
	const evenSum = digits[1] + digits[3] + digits[5] + digits[7];

	const expectedTenth = (((oddSum * 7) - evenSum) % 10 + 10) % 10;
	if (expectedTenth !== digits[9]) {
		return false;
	}

	const expectedEleventh = (oddSum + evenSum + digits[9]) % 10;
	return expectedEleventh === digits[10];
}
