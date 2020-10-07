using System;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class KeyTest
	{
		[Theory(DisplayName = "Constructor with value parameter should return initialized Key object")]
		[InlineData(" ")]
		[InlineData("x")]
		[InlineData("A")]
		[InlineData("2")]
		[InlineData("0")]
		[InlineData("&")]
		[InlineData("Key")]
		[InlineData("TEST_test$44")]
		public void ConstructorWithValue(string value)
		{
			var key = new Key(value);
			key.Value.ShouldBe(value);
			key.Code.ShouldBe(value);
			key.ControlKey.ShouldBeFalse();
			key.ShiftKey.ShouldBeFalse();
			key.AltKey.ShouldBeFalse();
			key.CommandKey.ShouldBeFalse();
		}

		[Theory(DisplayName = "Constructor with empty value parameter should throw exception")]
		[InlineData(null)]
		[InlineData("")]
		public void ConstructorWithNullValue(string value)
		{
			Should.Throw<ArgumentNullException>(() => new Key(value));
		}

		[Theory(DisplayName = "Constructor with value and code parameters should return initialized Key object")]
		[InlineData(" ", " ")]
		[InlineData("x", "x")]
		[InlineData("A", "A")]
		[InlineData("2", "2")]
		[InlineData("0", "0")]
		[InlineData("&", "&")]
		[InlineData("Key", "Key")]
		[InlineData("TEST_test$44", "TEST_test$44")]
		[InlineData(" ", "Space")]
		[InlineData("x", "0")]
		[InlineData("A", "a")]
		[InlineData("2", "Code 2")]
		[InlineData("0", "ABC")]
		[InlineData("&", "amp")]
		[InlineData("Key", "Test")]
		[InlineData("TEST_test$44", "$44-test")]
		public void ConstructorWithValueAndCode(string value, string code)
		{
			var key = new Key(value, code);
			key.Value.ShouldBe(value);
			key.Code.ShouldBe(code);
			key.ControlKey.ShouldBeFalse();
			key.ShiftKey.ShouldBeFalse();
			key.AltKey.ShouldBeFalse();
			key.CommandKey.ShouldBeFalse();
		}

		[Theory(DisplayName = "Constructor with empty value or code should throw exception")]
		[InlineData(null, "")]
		[InlineData("", null)]
		[InlineData(null, "t")]
		[InlineData("", "t")]
		[InlineData("T", null)]
		[InlineData("T", "")]
		public void ConstructorWithNullValueAndNonNullCode(string value, string code)
		{
			Should.Throw<ArgumentNullException>(() => new Key(value, code));
		}

		[Theory(DisplayName = "Constructor with value character should return initialized Key object")]
		[InlineData(' ')]
		[InlineData('x')]
		[InlineData('A')]
		[InlineData('2')]
		[InlineData('0')]
		[InlineData('&')]
		public void ConstructorWithValueCharacter(char value)
		{
			var key = new Key(value);
			key.Value.ShouldBe(value.ToString());
			key.Code.ShouldBe(value.ToString());
			key.ControlKey.ShouldBeFalse();
			key.ShiftKey.ShouldBeFalse();
			key.AltKey.ShouldBeFalse();
			key.CommandKey.ShouldBeFalse();
		}

	}
}
