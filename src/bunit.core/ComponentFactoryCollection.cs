#if NET5_0_OR_GREATER
using System.Collections;
using System.Collections.Generic;

namespace Bunit
{
	/// <summary>
	/// Represents a collection of <see cref="IComponentFactory"/>.
	/// </summary>
	public sealed class ComponentFactoryCollection : IList<IComponentFactory>
	{
		private readonly IList<IComponentFactory> factories = new List<IComponentFactory>();

		/// <summary>
		/// Gets or sets a <see cref="IComponentFactory"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the element to get or set.</param>
		/// <returns>The <see cref="IComponentFactory"/> at the specified index.</returns>
		public IComponentFactory this[int index] { get => factories[index]; set => factories[index] = value; }

		/// <summary>
		/// Gets the number of <see cref="IComponentFactory"/> contained in the <see cref="ComponentFactoryCollection"/>.
		/// </summary>
		public int Count => factories.Count;

		/// <summary>
		/// Gets a value indicating whether the <see cref="ComponentFactoryCollection"/> is read-only.
		/// </summary>
		public bool IsReadOnly => factories.IsReadOnly;

		/// <summary>
		/// Adds an <see cref="IComponentFactory"/> to the <see cref="ComponentFactoryCollection"/>.
		/// </summary>
		/// <param name="item">The <see cref="IComponentFactory"/> to add to the collection.</param>
		public void Add(IComponentFactory item) => factories.Add(item);

		/// <summary>
		/// Removes all <see cref="IComponentFactory"/>s from the <see cref="ComponentFactoryCollection"/>.
		/// </summary>
		public void Clear() => factories.Clear();

		/// <summary>
		/// Determines whether the <see cref="ComponentFactoryCollection"/> contains a specific <see cref="IComponentFactory"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="ComponentFactoryCollection"/>..</param>
		/// <returns>true if item is found in the <see cref="ComponentFactoryCollection"/>; otherwise, false.</returns>
		public bool Contains(IComponentFactory item) => factories.Contains(item);

		/// <summary>
		/// Copies the <see cref="IComponentFactory"/>s of the <see cref="ComponentFactoryCollection"/> to an <see cref="System.Array"/>, starting at a particular <see cref="System.Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied from <see cref="ComponentFactoryCollection"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
		public void CopyTo(IComponentFactory[] array, int arrayIndex) => factories.CopyTo(array, arrayIndex);

		/// <summary>
		/// Returns an enumerator that iterates through the collection of <see cref="IComponentFactory"/>.
		/// </summary>
		/// <returns>An enumerator that can be used to iterate through the collection of <see cref="IComponentFactory"/>.</returns>
		public IEnumerator<IComponentFactory> GetEnumerator() => factories.GetEnumerator();

		/// <summary>
		/// Determines the index of a specific <see cref="IComponentFactory"/> in the <see cref="ComponentFactoryCollection"/>.
		/// </summary>
		/// <param name="item">The <see cref="IComponentFactory"/> to locate in the <see cref="ComponentFactoryCollection"/>..</param>
		/// <returns>The index of <see cref="IComponentFactory"/> if found in the list; otherwise, -1.</returns>
		public int IndexOf(IComponentFactory item) => factories.IndexOf(item);

		/// <summary>
		/// Inserts an <see cref="IComponentFactory"/> to the <see cref="ComponentFactoryCollection"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <see cref="IComponentFactory"/> should be inserted.</param>
		/// <param name="item">The <see cref="IComponentFactory"/> to insert into the <see cref="ComponentFactoryCollection"/>.</param>
		public void Insert(int index, IComponentFactory item) => factories.Insert(index, item);

		/// <summary>
		/// Removes the first occurrence of a specific <see cref="IComponentFactory"/> from the <see cref="ComponentFactoryCollection"/>.
		/// </summary>
		/// <param name="item">The <see cref="IComponentFactory"/> to remove from the <see cref="ComponentFactoryCollection"/>.</param>
		/// <returns>true if <see cref="IComponentFactory"/> was successfully removed from the <see cref="ComponentFactoryCollection"/>; otherwise, false. This method also returns false if <see cref="IComponentFactory"/> is not found in the original <see cref="ComponentFactoryCollection"/>.</returns>
		public bool Remove(IComponentFactory item) => factories.Remove(item);

		/// <summary>
		/// Removes the <see cref="ComponentFactoryCollection"/> <see cref="IComponentFactory"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the <see cref="IComponentFactory"/> to remove.</param>
		public void RemoveAt(int index) => factories.RemoveAt(index);

		/// <summary>
		/// Returns an enumerator that iterates through a <see cref="ComponentFactoryCollection"/>.
		/// </summary>
		/// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the <see cref="ComponentFactoryCollection"/>.</returns>
		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)factories).GetEnumerator();
	}
}
#endif
