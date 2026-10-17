using Aristoxenus.Domain.Language.Bases;
using Aristoxenus.Domain.Language.Errors.Runtime;

namespace Aristoxenus.Language
{
    public partial class LanguageContext
    {
        private readonly Dictionary<string, LanguageValue> _values = [];
        private LanguageContext? _containingScope;
        public LanguageContext()
        {
            _containingScope = null;
        }

        public LanguageContext(LanguageContext containingScope)
        {
            _containingScope = containingScope;
        }

        public void Define(string name, LanguageValue value)
        {
            _values[name] = value;
        }

        public LanguageValue GetValue(string name)
        {
            if (_values.ContainsKey(name))
                return _values[name];

            if (_containingScope != null)
                return _containingScope.GetValue(name);

            throw new UnknownIdentifier(name);
        }

        public void Assign(string name, LanguageValue value)
        {
            if (_values.ContainsKey(name))
            {
                _values[name] = value;
                return;
            }
            if (_containingScope != null)
            {
                _containingScope.Assign(name, value);
                return;
            }
            throw new UnknownIdentifier(name);
        }

    }
}
