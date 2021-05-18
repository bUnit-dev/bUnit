#if NET5_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bunit.TestDoubles;
using Microsoft.AspNetCore.Components;

namespace Bunit.ComponentFactories
{
	internal class StubComponentFactory<TComponent> : IComponentFactory
		where TComponent : IComponent
	{
		private readonly Type componentTypeToStub = typeof(TComponent);
		private readonly bool renderParameters;

		public StubComponentFactory(bool renderParameters)
		{
			this.renderParameters = renderParameters;
		}

		public bool CanCreate(Type componentType)
			=> componentType == componentTypeToStub;

		public IComponent Create(Type componentType)
			=> new Stub<TComponent>(renderParameters);
	}
}
#endif
