using System;

namespace Societatis.HAL.Tests
{
    public class ConcreteLink : ILink
    {
        public Uri HRef => throw new NotImplementedException();

        public bool? Templated => throw new NotImplementedException();

        public string Type => throw new NotImplementedException();

        public Uri Deprecation => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public Uri Profile => throw new NotImplementedException();

        public string Title => throw new NotImplementedException();

        public string HrefLang => throw new NotImplementedException();
    }

    public abstract class AbstractLink : ILink
    {
        public Uri HRef => throw new NotImplementedException();

        public bool? Templated => throw new NotImplementedException();

        public string Type => throw new NotImplementedException();

        public Uri Deprecation => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public Uri Profile => throw new NotImplementedException();

        public string Title => throw new NotImplementedException();

        public string HrefLang => throw new NotImplementedException();
    }

    public class LinkWithoutDefaultConstructor : ILink
    {
        public LinkWithoutDefaultConstructor(string uri)
        {
        }

        public Uri HRef => throw new NotImplementedException();

        public bool? Templated => throw new NotImplementedException();

        public string Type => throw new NotImplementedException();

        public Uri Deprecation => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public Uri Profile => throw new NotImplementedException();

        public string Title => throw new NotImplementedException();

        public string HrefLang => throw new NotImplementedException();
    }

}