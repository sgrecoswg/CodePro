
namespace Mosaic.CodePro.Models
{
    public class CodeGeneratorSource : NotifyPropertyChanged
    {
        private CodeSources _codeSource;
        public CodeSources CodeSource
        {
            get
            {
                return _codeSource;
            }
            set
            {
                _codeSource = value;
                RaisePropertyChange("CodeSource");
            }
        }
    }
}
