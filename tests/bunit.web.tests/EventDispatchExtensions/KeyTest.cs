using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Components.Web;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class KeyTest
	{
		public static IEnumerable<object[]> KeyValueTestData { get; } = GetKeyValueTestData().Select(c => new object[] { c }).ToList();

		public static IEnumerable<object[]> CharTestData { get; } = new[] { ' ', 'x', 'A', '2', '0', '&', (char)0 }
			.Select(c => new object[] { c }).ToList();

		public static IEnumerable<Key[]> EqualsTestData { get; } = GetEqualsTestData().Select(i => new[] { i.First, i.Second });

		public static IEnumerable<Key?[]> NonEqualsTestData { get; } = GetNonEqualsTestData().Select(i => new[] { i.First, i.Second });

		public static IEnumerable<object[]> KeyAndObjectTestData { get; } = GetKeyAndObjectTestData().Select(i => new[] { i.First, i.Second });

		public static IEnumerable<object[]> KeyWithModifiersTestData { get; } = GetKeyWithModifiersTestData().Select(k => new object[] { k, true })
			.Concat(GetKeyWithModifiersTestData().Select(k => new object[] { k, false }))
			.ToList();

		public static IEnumerable<object[]> MainKeyAndModifierKeyTestData { get; } = GetMainKeyAndModifierKeyTestData();

		public static IEnumerable<Key[]> MainKeyToCombineTestData { get; } = GetKeyWithModifiersTestData().Select(k => new[] { k }).ToList();

		public static IEnumerable<Key[]> Combine2MainKeysTestData { get; } = Get2MainKeysTestData().Select(i => new[] { i.First, i.Second });

		public static IEnumerable<object[]> KeyboardEventArgsTestData { get; } = GetKeyboardEventArgsTestData();

		[Theory(DisplayName = "Get method with value parameter should return initialized Key object")]
		[MemberData(nameof(KeyValueTestData))]
		public void GetWithValue(string value)
		{
			var key = Key.Get(value);
			key.Value.ShouldBe(value);
			key.Code.ShouldBe(value);
			key.ControlKey.ShouldBeFalse();
			key.ShiftKey.ShouldBeFalse();
			key.AltKey.ShouldBeFalse();
			key.CommandKey.ShouldBeFalse();
		}

		[Theory(DisplayName = "Get method with empty value parameter should throw exception")]
		[InlineData(null)]
		[InlineData("")]
		public void GetWithNullValue(string value)
		{
			Should.Throw<ArgumentNullException>(() => Key.Get(value));
		}

		[Theory(DisplayName = "Casting from string should return initialized Key object")]
		[MemberData(nameof(KeyValueTestData))]
		public void CanCastFromString(string value)
		{
			Key key = value;
			key.Value.ShouldBe(value);
			key.Code.ShouldBe(value);
			key.ControlKey.ShouldBeFalse();
			key.ShiftKey.ShouldBeFalse();
			key.AltKey.ShouldBeFalse();
			key.CommandKey.ShouldBeFalse();
		}

		[Theory(DisplayName = "Casting from null or empty string throws ArgumentNullException")]
		[InlineData(null)]
		[InlineData("")]
		public void CastingFromNullStringThrowsException(string value)
		{
			Should.Throw<ArgumentNullException>(() => (Key)value);
		}

		[Theory(DisplayName = "Get method with value and code parameters should return initialized Key object")]
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
		public void GetWithValueAndCode(string value, string code)
		{
			var key = Key.Get(value, code);
			key.Value.ShouldBe(value);
			key.Code.ShouldBe(code);
			key.ControlKey.ShouldBeFalse();
			key.ShiftKey.ShouldBeFalse();
			key.AltKey.ShouldBeFalse();
			key.CommandKey.ShouldBeFalse();
		}

		[Theory(DisplayName = "Get method with empty value or code should throw exception")]
		[InlineData(null, "")]
		[InlineData("", null)]
		[InlineData(null, "t")]
		[InlineData("", "t")]
		[InlineData("T", null)]
		[InlineData("T", "")]
		public void GetWithNullValueAndNonNullCode(string value, string code)
		{
			Should.Throw<ArgumentNullException>(() => Key.Get(value, code));
		}

		[Theory(DisplayName = "Get with char value should return initialized Key object")]
		[MemberData(nameof(CharTestData))]
		public void GetFromChar(char value)
		{
			var key = Key.Get(value);
			key.Value.ShouldBe(value.ToString());
			key.Code.ShouldBe(value.ToString());
			key.ControlKey.ShouldBeFalse();
			key.ShiftKey.ShouldBeFalse();
			key.AltKey.ShouldBeFalse();
			key.CommandKey.ShouldBeFalse();
		}

		[Theory(DisplayName = "Casting from char should return initialized Key object")]
		[MemberData(nameof(CharTestData))]
		public void CanCastFromChar(char value)
		{
			Key key = value;
			key.Value.ShouldBe(value.ToString());
			key.Code.ShouldBe(value.ToString());
			key.ControlKey.ShouldBeFalse();
			key.ShiftKey.ShouldBeFalse();
			key.AltKey.ShouldBeFalse();
			key.CommandKey.ShouldBeFalse();
		}

		[Fact(DisplayName = "Get method returns same instances for predefined keys")]
		public void GetPredefinedKeys()
		{
			var keyType = typeof(Key);
			var predefinedKeyProperties = keyType.GetProperties(BindingFlags.Public | BindingFlags.Static)
				.Where(p => p.CanRead && p.PropertyType == keyType);

			foreach (var predefinedKeyProperty in predefinedKeyProperties)
			{
				var predefinedKey = (Key)predefinedKeyProperty.GetValue(null)!;
				var instanceKey = Key.Get(predefinedKey.Value, predefinedKey.Code);
				instanceKey.ShouldBeSameAs(predefinedKey);
			}
		}

		[Theory(DisplayName = "Keys with same values should be equal")]
		[MemberData(nameof(EqualsTestData))]
		public void EqualsShouldBeTrueForSameKeys(Key key1, Key key2)
		{
			key1.Equals(key2).ShouldBeTrue();
			key2.Equals(key1).ShouldBeTrue();
			key1.Equals((object)key2).ShouldBeTrue();
			key2.Equals((object)key1).ShouldBeTrue();
			(key1 == key2).ShouldBeTrue();
			(key2 == key1).ShouldBeTrue();
			(key1 != key2).ShouldBeFalse();
			(key2 != key1).ShouldBeFalse();
		}

		[Theory(DisplayName = "Keys with same values should have same hash code")]
		[MemberData(nameof(EqualsTestData))]
		public void SameHashCode(Key key1, Key key2)
		{
			var hashCode1 = key1.GetHashCode();
			var hashCode2 = key2.GetHashCode();
			hashCode1.ShouldBe(hashCode2);
		}

		[Fact(DisplayName = "Null keys should be equal")]
		[SuppressMessage("Maintainability", "CA1508:Avoid dead conditional code", Justification = "Point of test is to validate correctly implemented Equals")]
		public void NullsAreEqual()
		{
			Key? key1 = default;
			Key? key2 = null;
			(key1 == key2).ShouldBeTrue();
			(key2 == key1).ShouldBeTrue();
			(key1 != key2).ShouldBeFalse();
			(key2 != key1).ShouldBeFalse();
		}

		[Theory(DisplayName = "Keys with different values should not be equal")]
		[MemberData(nameof(NonEqualsTestData))]
		public void EqualsShouldBeFalseForDifferentKeys(Key? key1, Key? key2)
		{
			if (key1 is not null)
			{
				key1.Equals(key2).ShouldBeFalse();
				key1.Equals((object?)key2).ShouldBeFalse();
			}

			if (key2 is not null)
			{
				key2.Equals(key1).ShouldBeFalse();
				key2.Equals((object?)key1).ShouldBeFalse();
			}

			(key1 == key2).ShouldBeFalse();
			(key2 == key1).ShouldBeFalse();
			(key1 != key2).ShouldBeTrue();
			(key2 != key1).ShouldBeTrue();
		}

		// This may be flaky test. Hash codes can have collisions.
		[Theory(DisplayName = "Keys with different values should have different hash codes")]
		[MemberData(nameof(NonEqualsTestData))]
		public void DifferentHashCodes(Key? key1, Key? key2)
		{
			var hashCode1 = key1?.GetHashCode() ?? 0;
			var hashCode2 = key2?.GetHashCode() ?? 0;
			hashCode1.ShouldNotBe(hashCode2);
		}

		[Theory(DisplayName = "Key should not be equal to different object type")]
		[MemberData(nameof(KeyAndObjectTestData))]
		public void KeyShouldBeDifferentFromObject(Key key, object other)
		{
			key.Equals(other).ShouldBeFalse();
		}

		[Theory(DisplayName = "WithControlKey should set ControlKey flag")]
		[MemberData(nameof(KeyWithModifiersTestData))]
		public void WithControlKey(Key key, bool flag)
		{
			var result = key.WithControlKey(flag);
			result.ShouldNotBeSameAs(key);
			result.Value.ShouldBe(key.Value);
			result.Code.ShouldBe(key.Code);
			result.ControlKey.ShouldBe(flag);
			result.AltKey.ShouldBe(key.AltKey);
			result.ShiftKey.ShouldBe(key.ShiftKey);
			result.CommandKey.ShouldBe(key.CommandKey);
		}

		[Theory(DisplayName = "WithShiftKey should set ShiftKey flag")]
		[MemberData(nameof(KeyWithModifiersTestData))]
		public void WithShiftKey(Key key, bool flag)
		{
			var result = key.WithShiftKey(flag);
			result.ShouldNotBeSameAs(key);
			result.Value.ShouldBe(key.Value);
			result.Code.ShouldBe(key.Code);
			result.ControlKey.ShouldBe(key.ControlKey);
			result.AltKey.ShouldBe(key.AltKey);
			result.ShiftKey.ShouldBe(flag);
			result.CommandKey.ShouldBe(key.CommandKey);
		}

		[Theory(DisplayName = "WithAltKey should set AltKey flag")]
		[MemberData(nameof(KeyWithModifiersTestData))]
		public void WithAltKey(Key key, bool flag)
		{
			var result = key.WithAltKey(flag);
			result.ShouldNotBeSameAs(key);
			result.Value.ShouldBe(key.Value);
			result.Code.ShouldBe(key.Code);
			result.ControlKey.ShouldBe(key.ControlKey);
			result.AltKey.ShouldBe(flag);
			result.ShiftKey.ShouldBe(key.ShiftKey);
			result.CommandKey.ShouldBe(key.CommandKey);
		}

		[Theory(DisplayName = "WithCommandKey should set CommandKey flag")]
		[MemberData(nameof(KeyWithModifiersTestData))]
		public void WithCommandKey(Key key, bool flag)
		{
			var result = key.WithCommandKey(flag);
			result.ShouldNotBeSameAs(key);
			result.Value.ShouldBe(key.Value);
			result.Code.ShouldBe(key.Code);
			result.ControlKey.ShouldBe(key.ControlKey);
			result.AltKey.ShouldBe(key.AltKey);
			result.ShiftKey.ShouldBe(key.ShiftKey);
			result.CommandKey.ShouldBe(flag);
		}

		[Theory(DisplayName = "Combine key and key modifier should return key with modifier set")]
		[MemberData(nameof(MainKeyAndModifierKeyTestData))]
		public void Combine2Keys(Key mainKey, Key keyModifier, bool controlKey, bool shiftKey, bool altKey, bool commandKey)
		{
			AssertCombinedKey(mainKey.Combine(keyModifier));
			AssertCombinedKey(keyModifier.Combine(mainKey));
			AssertCombinedKey(mainKey + keyModifier);
			AssertCombinedKey(keyModifier + mainKey);

			void AssertCombinedKey(Key resultKey)
			{
				resultKey.Value.ShouldBe(mainKey.Value);
				resultKey.Code.ShouldBe(mainKey.Code);
				resultKey.ControlKey.ShouldBe(controlKey);
				resultKey.ShiftKey.ShouldBe(shiftKey);
				resultKey.AltKey.ShouldBe(altKey);
				resultKey.CommandKey.ShouldBe(commandKey);
			}
		}

		[Theory(DisplayName = "Combine key with null should return the same key")]
		[MemberData(nameof(MainKeyToCombineTestData))]
		public void CombineKeyWithNull(Key key)
		{
			key.Combine(null).ShouldBeSameAs(key);
			(key + null).ShouldBeSameAs(key);
			(null! + key).ShouldBeSameAs(key);
		}

		[Theory(DisplayName = "Combine 2 main keys should throw ArgumentException")]
		[MemberData(nameof(Combine2MainKeysTestData))]
		public void Combine2MainKeys(Key key1, Key key2)
		{
			Should.Throw<ArgumentException>(() => key1.Combine(key2));
			Should.Throw<ArgumentException>(() => key2.Combine(key1));
			Should.Throw<ArgumentException>(() => key1 + key2);
			Should.Throw<ArgumentException>(() => key2 + key1);
		}

		[Theory(DisplayName = "Can convert to KeyboardEventArgs")]
		[MemberData(nameof(KeyboardEventArgsTestData))]
		public void ToKeyboardEventArgs(Key key, string value, string code, bool ctrlKey, bool shiftKey, bool altKey, bool metaKey)
		{
			KeyboardEventArgs result = key;
			result.Key.ShouldBe(value);
			result.Code.ShouldBe(code);
			result.CtrlKey.ShouldBe(ctrlKey);
			result.ShiftKey.ShouldBe(shiftKey);
			result.AltKey.ShouldBe(altKey);
			result.MetaKey.ShouldBe(metaKey);
			result.Location.ShouldBe(0);
			result.Type.ShouldBe(null);
		}

		[Fact(DisplayName = "Can convert null to KeyboardEventArgs")]
		public void NullToKeyboardEventArgs()
		{
			Key key = null!;
			KeyboardEventArgs result = key;
			result.ShouldBeNull();
		}

		private static IEnumerable<string> GetKeyValueTestData()
		{
			return new[]
			{
				" ",
				"x",
				"A",
				"2",
				"0",
				"&",
				"Key",
				"TEST_test$44",
				"F5",
			};
		}

		private static IEnumerable<(Key First, Key Second)> GetEqualsTestData()
		{
			var xKey = Key.Get('x');
			var dollarKey = Key.Get("$", "Dollar");
			var customKey = Key.Get("Test", "12345");

			return new[]
			{
				(Key.Enter, Key.Enter),
				(Key.F10, Key.F10),
				(Key.NumberPad2, Key.NumberPad2),
				(Key.Space, Key.Space),
				(Key.Control, Key.Control),
				(Key.Shift, Key.Shift),
				(Key.Alt, Key.Alt),
				(Key.Command, Key.Command),
				(Key.Enter, Key.Get("Enter")),
				(Key.F10, Key.Get("F10")),
				(Key.NumberPad2, Key.Get("2", "Numpad2")),
				(Key.Space, Key.Get(" ", "Space")),
				(Key.Control, Key.Get("Control", "ControlLeft") + Key.Control),
				(Key.Shift, Key.Get("Shift", "ShiftLeft") + Key.Shift),
				(Key.Alt, Key.Get("Alt", "AltLeft") + Key.Alt),
				(Key.Command, Key.Get("Meta", "MetaLeft") + Key.Command),
				(xKey, xKey),
				(dollarKey, dollarKey),
				(customKey, customKey),
				(xKey, Key.Get('x')),
				(dollarKey, Key.Get("$", "Dollar")),
				(customKey, Key.Get("Test", "12345")),
				(Key.Control + Key.Alt + Key.Shift, Key.Control + Key.Alt + Key.Shift),
				(Key.Enter + Key.Command, Key.Enter + Key.Command),
				(Key.F10 + Key.Control, Key.F10 + Key.Control),
				(Key.Space + Key.Alt, Key.Get(" ", "Space") + Key.Alt),
				(xKey + Key.Shift, xKey + Key.Shift),
				(dollarKey + Key.Control + Key.Shift, Key.Get("$", "Dollar") + Key.Shift + Key.Control),
				(customKey + Key.Alt + Key.Control, Key.Get("Test", "12345") + Key.Control + Key.Alt),
			};
		}

		private static IEnumerable<(Key? First, Key? Second)> GetNonEqualsTestData()
		{
			var xKey = Key.Get('x');
			var dollarKey = Key.Get("$", "Dollar");
			var customKey = Key.Get("Test", "12345");

			return new (Key?, Key?)[]
			{
				(Key.Enter, Key.Escape),
				(Key.F1, Key.F2),
				(Key.NumberPad2, Key.NumberPad8),
				(Key.Space, Key.Control),
				(Key.Alt, Key.Shift),
				(xKey, Key.Get('y')),
				(dollarKey, Key.Get("$", "Pound")),
				(customKey, Key.Get("test", "12345")),
				(Key.Control + Key.Alt + Key.Shift, Key.Control + Key.Alt),
				(Key.Enter + Key.Control, Key.Enter + Key.Alt),
				(Key.F10 + Key.Alt, Key.F10 + Key.Shift),
				(Key.NumberPad2 + Key.Command, Key.NumberPad3 + Key.Command),
				(Key.Space + Key.Command, Key.Space + Key.Alt),
				(xKey + Key.Control + Key.Alt, xKey + Key.Shift + Key.Alt),
				(dollarKey + Key.Command + Key.Shift, dollarKey + Key.Command),
				(customKey + Key.Control + Key.Alt + Key.Shift, customKey),
				(xKey, null),
				(Key.Control, null),
				(Key.Enter + Key.Command, null),
				(Key.F10, null),
			};
		}

		private static IEnumerable<(Key First, object Second)> GetKeyAndObjectTestData()
		{
			return new (Key, object)[]
			{
				(Key.Space, false),
				(Key.Get('G'), 'G'),
				(Key.Get("Test"), "Test"),
				(Key.Control, 2),
				(Key.Get("Event", "Args"), EventArgs.Empty),
			};
		}

		private static IEnumerable<Key> GetKeyWithModifiersTestData()
		{
			return new[]
			{
				Key.Enter,
				Key.Add,
				Key.Get('c'),
				Key.Get("T", "Test"),
				Key.Enter + Key.Control,
				Key.Add + Key.Shift,
				Key.Get('c') + Key.Alt,
				Key.Get("T", "Test") + Key.Command,
				Key.Space + Key.Control + Key.Alt + Key.Shift,
				Key.Get('0') + Key.Shift + Key.Control,
			};
		}

		private static IEnumerable<object[]> GetMainKeyAndModifierKeyTestData()
		{
			return new[]
			{
				new object[] { Key.Enter, Key.Control, true, false, false, false },
				new object[] { Key.Add, Key.Shift, false, true, false, false },
				new object[] { Key.Get('c'), Key.Alt, false, false, true, false },
				new object[] { Key.Get("T", "Test"), Key.Command, false, false, false, true },
				new object[] { Key.Escape + Key.Alt, Key.Control, true, false, true, false },
				new object[] { Key.NumberPad0, Key.Alt + Key.Shift, false, true, true, false },
				new object[] { Key.Multiply + Key.Command, Key.Command, false, false, false, true },
				new object[] { Key.Enter + Key.Shift, Key.Shift + Key.Control, true, true, false, false },
				new object[] { Key.Get('1') + Key.Command, Key.Shift + Key.Control + Key.Alt, true, true, true, true },
				new object[] { Key.Enter + Key.Shift + Key.Alt, Key.Shift + Key.Control, true, true, true, false },
			};
		}

		private static IEnumerable<(Key First, Key Second)> Get2MainKeysTestData()
		{
			return new[]
			{
				(Key.Enter, Key.Enter),
				(Key.Space, Key.Backspace),
				(Key.Get('0'), Key.NumberPad0),
				(Key.Enter + Key.Control, Key.Enter),
				(Key.Add + Key.Alt, Key.Enter + Key.Shift),
				(Key.Get("T", "Test"), Key.Get("T", "Test")),
				(Key.Get('0') + Key.Command, Key.NumberPad0 + Key.Command + Key.Alt),
			};
		}

		private static IEnumerable<object[]> GetKeyboardEventArgsTestData()
		{
			return new[]
			{
				new object[] { Key.Enter, "Enter", "Enter", false, false, false, false },
				new object[] { Key.Add, "+", "NumpadAdd", false, false, false, false },
				new object[] { Key.Get('c'), "c", "c", false, false, false, false },
				new object[] { Key.Get("T", "Test"), "T", "Test", false, false, false, false },
				new object[] { Key.Enter + Key.Control, "Enter", "Enter", true, false, false, false },
				new object[] { Key.Add + Key.Shift, "+", "NumpadAdd", false, true, false, false },
				new object[] { Key.Get('c') + Key.Alt, "c", "c", false, false, true, false },
				new object[] { Key.Get("T", "Test") + Key.Command, "T", "Test", false, false, false, true },
				new object[] { Key.Control, "Control", "ControlLeft", true, false, false, false },
				new object[] { Key.Shift, "Shift", "ShiftLeft", false, true, false, false },
				new object[] { Key.Alt, "Alt", "AltLeft", false, false, true, false },
				new object[] { Key.Command, "Meta", "MetaLeft", false, false, false, true },
				new object[] { Key.Space + Key.Control + Key.Alt + Key.Shift, " ", "Space", true, true, true, false },
			};
		}
	}
}
