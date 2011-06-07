namespace WaveTech.Scutex.Model
{
	public struct LicensePlaceholder
	{
		public PlaceholderTypes Type { get; set; }
		public ValidationTypes ValidationType { get; set; }
		public char Token { get; set; }
		public string Value { get; set; }
		public int Length { get; set; }
		public bool IsChecksum { get; set; }

		public bool Equals(LicensePlaceholder other)
		{
			return Equals(other.Type, Type) && Equals(other.ValidationType, ValidationType) && other.Token == Token && Equals(other.Value, Value) && other.Length == Length && other.IsChecksum.Equals(IsChecksum);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj)) return false;
			if (obj.GetType() != typeof (LicensePlaceholder)) return false;
			return Equals((LicensePlaceholder) obj);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				int result = Type.GetHashCode();
				result = (result*397) ^ ValidationType.GetHashCode();
				result = (result*397) ^ Token.GetHashCode();
				result = (result*397) ^ (Value != null ? Value.GetHashCode() : 0);
				result = (result*397) ^ Length;
				result = (result*397) ^ IsChecksum.GetHashCode();
				return result;
			}
		}

		public static bool operator ==(LicensePlaceholder left, LicensePlaceholder right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(LicensePlaceholder left, LicensePlaceholder right)
		{
			return !left.Equals(right);
		}
	}
}