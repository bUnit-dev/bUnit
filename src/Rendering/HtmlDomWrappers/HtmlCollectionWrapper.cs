using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    internal class HtmlCollectionWrapper<T> : WrapperBase<IHtmlCollection<T>>, IHtmlCollection<T>, INodeWrapper
        where T : IElement
    {
        public HtmlCollectionWrapper(Func<IHtmlCollection<T>?> getObject) : base(getObject)
        {
        }

        public int Length => WrappedObject.Length;

        public T this[string id] => GetOrWrap<T>(HashCode.Combine("this+string", id), () => WrappedObject[id]);

        public T this[int index] => GetOrWrap<T>(HashCode.Combine("this+int", index), () => WrappedObject[index]);

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
