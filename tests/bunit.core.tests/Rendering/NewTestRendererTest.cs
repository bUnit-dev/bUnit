using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bunit.Rendering;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using Shouldly;
using static Bunit.ComponentParameterFactory;
using Xunit;

namespace Bunit.Rendering
{
	public class NoChildNoParams : ComponentBase
	{
		public const string MARKUP = "hello world";
		protected override void BuildRenderTree(RenderTreeBuilder builder)
			=> builder.AddMarkupContent(0, MARKUP);
	}

	public class ThrowsDuringSetParams : ComponentBase
	{
		public static readonly InvalidOperationException EXCEPTION =
			new InvalidOperationException("THROWS ON PURPOSE");

		public override Task SetParametersAsync(ParameterView parameters) => throw EXCEPTION;
	}

	public class HasParams : ComponentBase
	{
		[Parameter] public string? Value { get; set; }
		[Parameter] public RenderFragment? ChildContent { get; set; }

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddMarkupContent(0, Value);
			builder.AddContent(1, ChildContent);
		}
	}

	public class RenderTrigger : ComponentBase
	{
		[Parameter] public string? Value { get; set; }

		public Task Trigger() => InvokeAsync(StateHasChanged);
		public Task TriggerWithValue(string value)
		{
			Value = value;
			return InvokeAsync(StateHasChanged);
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.AddMarkupContent(0, Value);
		}
	}

	public class ToggleChild : ComponentBase
	{
		private bool _showing = true;

		[Parameter] public RenderFragment? ChildContent { get; set; }

		public Task DisposeChild()
		{
			_showing = false;
			return InvokeAsync(StateHasChanged);
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			if (_showing)
				builder.AddContent(0, ChildContent);
		}
	}

	internal class TestRenderedFragment : IRenderedComponent
	{
		private readonly object _markupAccessLock = new object();
		private string _markup = string.Empty;

		public bool IsDisposed { get; private set; }

		public int ComponentId { get; protected set; }

		public string Markup
		{
			get
			{
				if (IsDisposed)
					throw new ComponentDisposedException(ComponentId);

				lock (_markupAccessLock)
				{
					return _markup;
				}
			}
		}

		public int RenderCount { get; protected set; }

		public TestRenderedFragment(int componentId)
		{
			ComponentId = componentId;
		}

		void IRenderedComponent.OnRender(NewRenderEvent renderEvent)
		{
			if (IsDisposed)
				return;

			lock (_markupAccessLock)
			{
				var (rendered, changed, disposed) = renderEvent.GetRenderStatus(this);

				if (disposed)
				{
					((IDisposable)this).Dispose();
					return;
				}

				if (rendered)
				{
					OnRender(renderEvent);
					RenderCount++;

					if (changed)
						UpdateMarkup(renderEvent.Frames);
				}
			}
		}
		
		void IDisposable.Dispose()
		{
			IsDisposed = true;
			_markup = string.Empty;
		}

		protected void UpdateMarkup(RenderTreeFrameCollection framesCollection)
		{
			lock (_markupAccessLock)
			{
				_markup = HewHtmlizer.GetHtml(ComponentId, framesCollection);
			}
		}

		protected virtual void OnRender(NewRenderEvent renderEvent) { }
	}

	internal class TestRenderedComponent<TComponent> : TestRenderedFragment where TComponent : IComponent
	{
		private TComponent _instance = default!;

		public TComponent Instance => _instance ?? throw new InvalidOperationException("Component has not rendered yet...");

		public TestRenderedComponent(int componentId) : base(componentId)
		{
		}

		public TestRenderedComponent(int componentId, TComponent instance, RenderTreeFrameCollection componentFrames) : base(componentId)
		{
			_instance = instance;
			RenderCount++;
			UpdateMarkup(componentFrames);
		}

		protected override void OnRender(NewRenderEvent renderEvent)
		{
			if (_instance is null)
			{
				SetComponentAndID(renderEvent);
			}
		}

		private void SetComponentAndID(NewRenderEvent renderEvent)
		{
			var frames = renderEvent.Frames[ComponentId];
			if (frames.Array[0].Component is TComponent wrapperComponent)
			{
				_instance = wrapperComponent;
				ComponentId = frames.Array[0].ComponentId;
			}
			else
			{
				throw new InvalidOperationException("Component instance not found at expected position in render tree.");
			}
		}
	}

	internal sealed class TestRenderedComponentActivator : IRenderedComponentActivator
	{
		public IRenderedComponent CreateRenderedComponent<T>(int componentId) where T : IComponent
			=> new TestRenderedComponent<T>(componentId);
		public IRenderedComponent CreateRenderedComponent(int componentId)
			=> new TestRenderedFragment(componentId);
		public IRenderedComponent CreateRenderedComponent<T>(int componentId, T component, RenderTreeFrameCollection componentFrames) where T : IComponent
			=> new TestRenderedComponent<T>(componentId, component, componentFrames);
	}

	internal static class TestRendererExtensions
	{
		public static TestRenderedComponent<TComponent> RenderComponent<TComponent>(this NewTestRenderer renderer, params ComponentParameter[] parameters)
			where TComponent : IComponent
		{
			return (TestRenderedComponent<TComponent>)renderer.RenderComponent<TComponent>(parameters);
		}
	}

	public class NewTestRendererTest
	{
		private ILoggerFactory LoggerFactory { get; } = NullLoggerFactory.Instance;
		private TestServiceProvider Services { get; } = new TestServiceProvider();
		private IRenderedComponentActivator Activator { get; } = new TestRenderedComponentActivator();

		[Fact(DisplayName = "RenderFragment re-throws exception from component")]
		public void Test004()
		{
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);
			RenderFragment thowingFragment = b => { b.OpenComponent<ThrowsDuringSetParams>(0); b.CloseComponent(); };

			Should.Throw<InvalidOperationException>(() => sut.RenderFragment(thowingFragment))
				.Message.ShouldBe(ThrowsDuringSetParams.EXCEPTION.Message);
		}

		[Fact(DisplayName = "RenderComponent re-throws exception from component")]
		public void Test003()
		{
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			Should.Throw<InvalidOperationException>(() => sut.RenderComponent<ThrowsDuringSetParams>())
				.Message.ShouldBe(ThrowsDuringSetParams.EXCEPTION.Message);
		}

		[Fact(DisplayName = "Can render fragment without children and no parameters")]
		public void Test001()
		{
			const string MARKUP = "<h1>hello world</h1>";
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = (TestRenderedFragment)sut.RenderFragment(builder => builder.AddMarkupContent(0, MARKUP));

			cut.RenderCount.ShouldBe(1);
			cut.Markup.ShouldBe(MARKUP);
		}

		[Fact(DisplayName = "Can render component without children and no parameters")]
		public void Test002()
		{
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<NoChildNoParams>();

			cut.RenderCount.ShouldBe(1);
			cut.Markup.ShouldBe(NoChildNoParams.MARKUP);
			cut.Instance.ShouldBeOfType<NoChildNoParams>();
		}

		[Fact(DisplayName = "Can render component with parameters")]
		public void Test005()
		{
			const string VALUE = "FOO BAR";
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<HasParams>((nameof(HasParams.Value), VALUE));

			cut.Instance.Value.ShouldBe(VALUE);
		}

		[Fact(DisplayName = "Can render component with child component")]
		public void Test006()
		{
			const string PARENT_VALUE = "PARENT";
			const string CHILD_VALUE = "CHILD";

			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<HasParams>(
				(nameof(HasParams.Value), PARENT_VALUE),
				ChildContent<HasParams>((nameof(HasParams.Value), CHILD_VALUE))
			);

			cut.Markup.ShouldStartWith(PARENT_VALUE);
			cut.Markup.ShouldEndWith(CHILD_VALUE);
		}

		[Fact(DisplayName = "Rendered component gets RenderCount updated on re-render")]
		public async Task Test010()
		{
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<RenderTrigger>();

			cut.RenderCount.ShouldBe(1);

			await cut.Instance.Trigger();

			cut.RenderCount.ShouldBe(2);
		}

		[Fact(DisplayName = "Rendered component gets Markup updated on re-render")]
		public async Task Test011()
		{
			const string EXPECTED = "NOW VALUE";
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<RenderTrigger>();

			cut.RenderCount.ShouldBe(1);

			await cut.Instance.TriggerWithValue(EXPECTED);

			cut.RenderCount.ShouldBe(2);
			cut.Markup.ShouldBe(EXPECTED);
		}

		[Fact(DisplayName = "FindComponent returns first component nested inside another rendered component")]
		public void Test020()
		{
			// arrange
			const string PARENT_VALUE = "PARENT";
			const string CHILD_VALUE = "CHILD";

			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<HasParams>(
				(nameof(HasParams.Value), PARENT_VALUE),
				ChildContent<HasParams>((nameof(HasParams.Value), CHILD_VALUE))
			);

			// act
			var childCut = (TestRenderedComponent<HasParams>)sut.FindComponent<HasParams>(cut);

			// assert
			childCut.Markup.ShouldBe(CHILD_VALUE);
			childCut.RenderCount.ShouldBe(1);
		}

		[Fact(DisplayName = "FindComponent throws if parentComponent parameter is null")]
		public void Test021()
		{
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			Should.Throw<ArgumentNullException>(() => sut.FindComponent<HasParams>(null!));
		}

		[Fact(DisplayName = "FindComponent throws if component is not found")]
		public void Test022()
		{
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);
			var cut = sut.RenderComponent<HasParams>();

			Should.Throw<ComponentNotFoundException>(() => sut.FindComponent<HasParams>(cut));
		}

		[Fact(DisplayName = "FindComponents returns all components nested inside another rendered component")]
		public void Test030()
		{
			// arrange
			const string GRAND_PARENT_VALUE = nameof(GRAND_PARENT_VALUE);
			const string PARENT_VALUE = nameof(PARENT_VALUE);
			const string CHILD_VALUE = nameof(CHILD_VALUE);


			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<HasParams>(
				(nameof(HasParams.Value), GRAND_PARENT_VALUE),
				ChildContent<HasParams>(
					(nameof(HasParams.Value), PARENT_VALUE),
					ChildContent<HasParams>(
						(nameof(HasParams.Value), CHILD_VALUE)
					)
				)
			);

			// act
			var childCuts = sut.FindComponents<HasParams>(cut)
				.Cast<TestRenderedComponent<HasParams>>()
				.ToArray();

			// assert
			childCuts[0].Markup.ShouldBe(PARENT_VALUE + CHILD_VALUE);
			childCuts[0].RenderCount.ShouldBe(1);

			childCuts[1].Markup.ShouldBe(CHILD_VALUE);
			childCuts[1].RenderCount.ShouldBe(1);
		}

		[Fact(DisplayName = "FindComponents throws if parentComponent parameter is null")]
		public void Test031()
		{
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			Should.Throw<ArgumentNullException>(() => sut.FindComponents<HasParams>(null!));
		}

		[Fact(DisplayName = "Retrieved rendered child component with FindComponent gets updated on re-render")]
		public async Task Test040()
		{
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var parent = sut.RenderComponent<HasParams>(
				ChildContent<RenderTrigger>()
			);

			// act
			var cut = (TestRenderedComponent<RenderTrigger>)sut.FindComponent<RenderTrigger>(parent);

			cut.RenderCount.ShouldBe(1);

			await cut.Instance.TriggerWithValue("X");

			cut.RenderCount.ShouldBe(2);
			cut.Markup.ShouldBe("X");
		}

		[Fact(DisplayName = "Retrieved rendered child component with FindComponents gets updated on re-render")]
		public async Task Test041()
		{
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var parent = sut.RenderComponent<HasParams>(
				ChildContent<RenderTrigger>()
			);

			// act
			var cut = (TestRenderedComponent<RenderTrigger>)sut.FindComponents<RenderTrigger>(parent).Single();

			cut.RenderCount.ShouldBe(1);

			await cut.Instance.TriggerWithValue("X");

			cut.RenderCount.ShouldBe(2);
			cut.Markup.ShouldBe("X");
		}

		[Fact(DisplayName = "Rendered component updates on re-renders from child components with changes in render tree")]
		public async Task Test050()
		{
			// arrange
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<HasParams>(
				ChildContent<RenderTrigger>()
			);
			var child = (TestRenderedComponent<RenderTrigger>)sut.FindComponent<RenderTrigger>(cut);

			// act
			await child.Instance.TriggerWithValue("X");

			// assert
			cut.RenderCount.ShouldBe(2);
			cut.Markup.ShouldBe("X");
		}

		[Fact(DisplayName = "When component is disposed by renderer, getting Markup throws and IsDisposed returns true")]
		public async Task Test060()
		{
			// arrange
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<ToggleChild>(
				ChildContent<NoChildNoParams>()
			);
			var child = (TestRenderedComponent<NoChildNoParams>)sut.FindComponent<NoChildNoParams>(cut);

			// act
			await cut.Instance.DisposeChild();

			// assert
			child.IsDisposed.ShouldBeTrue();
			Should.Throw<ComponentDisposedException>(() => child.Markup);
		}

		[Fact(DisplayName = "Rendered component updates itself if a child's child is disposed")]
		public async Task Test061()
		{
			// arrange
			using var sut = new NewTestRenderer(Activator, Services, LoggerFactory);

			var cut = sut.RenderComponent<ToggleChild>(
				ChildContent<ToggleChild>(
					ChildContent<NoChildNoParams>()
				)
			);
			var child = (TestRenderedComponent<ToggleChild>)sut.FindComponent<ToggleChild>(cut);
			var childChild = (TestRenderedComponent<NoChildNoParams>)sut.FindComponent<NoChildNoParams>(cut);

			// act
			await child.Instance.DisposeChild();

			// assert
			childChild.IsDisposed.ShouldBeTrue();
			cut.Markup.ShouldBe(string.Empty);
		}

		[Fact(DisplayName = "When test renderer is disposed, so is all rendered components")]
		public void Test070()
		{
			TestRenderedComponent<NoChildNoParams> cut;
			using (var sut = new NewTestRenderer(Activator, Services, LoggerFactory))
			{
				cut = sut.RenderComponent<NoChildNoParams>();
			}
			cut.IsDisposed.ShouldBeTrue();
		}
	}
}
