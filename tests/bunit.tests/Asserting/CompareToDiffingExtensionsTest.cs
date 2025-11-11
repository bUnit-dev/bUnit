namespace Bunit;

public class CompareToDiffingExtensionsTest : BunitContext
{
	[Fact(DisplayName = "CompareTo with rendered fragment and string")]
	public void Test002()
	{
		var rf1 = Render<Simple1>(ps => ps.Add(p => p.Header, "FOO"));
		var rf2 = Render<Simple1>(ps => ps.Add(p => p.Header, "BAR"));

		rf1.CompareTo(rf2.Markup).Count.ShouldBe(1);
	}

	[Fact(DisplayName = "CompareTo with rendered fragment and rendered fragment")]
	public void Test003()
	{
		var rf1 = Render<Simple1>(ps => ps.Add(p => p.Header, "FOO"));
		var rf2 = Render<Simple1>(ps => ps.Add(p => p.Header, "BAR"));

		rf1.CompareTo(rf2).Count.ShouldBe(1);
	}

	[Fact(DisplayName = "CompareTo with INode and INodeList")]
	public void Test004()
	{
		var rf1 = Render<Simple1>(ps => ps.Add(p => p.Header, "FOO"));
		var rf2 = Render<Simple1>(ps => ps.Add(p => p.Header, "BAR"));

		var elm = rf1.Find("h1");
		elm.CompareTo(rf2.Nodes).Count.ShouldBe(1);
	}

	[Fact(DisplayName = "CompareTo with INodeList and INode")]
	public void Test005()
	{
		var rf1 = Render<Simple1>(ps => ps.Add(p => p.Header, "FOO"));
		var rf2 = Render<Simple1>(ps => ps.Add(p => p.Header, "BAR"));

		var elm = rf1.Find("h1");

		rf2.Nodes.CompareTo(elm).Count.ShouldBe(1);
	}
}
