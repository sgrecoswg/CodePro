

namespace SensibleProgramming.CodePro.Models
{
    public class CodeGenerationOptions : NotifyPropertyChanged
    {
        private bool _outputAsClassLibrary;
        public bool OutputAsClassLibrary {
        get
        {
            return _outputAsClassLibrary;
        }
        set
        {
            _outputAsClassLibrary = value;
            RaisePropertyChange("OutputAsClassLibrary");
        }
    }

        private bool _outputAsMVC;
        public bool OutputAsMVC {
        get
        {
            return _outputAsMVC;
        }
        set
        {
                _outputAsMVC = value;
            RaisePropertyChange("OutputAsMVC");
        }
    }

        private bool _outputAsWebAPI;
        public bool OutputAsWebAPI {
        get
        {
            return _outputAsWebAPI;
        }
        set
        {
                _outputAsWebAPI = value;
            RaisePropertyChange("OutputAsWebAPI");
        }
    }

        private bool _outputAsWPF;
        public bool OutputAsWPF {
        get
        {
            return _outputAsWPF;
        }
        set
        {
                _outputAsWPF = value;
            RaisePropertyChange("OutputAsWPF");
        }
    }

        private bool _outputAsJSLibrary;

        public bool OutputAsJSLibrary
        {
            get { return _outputAsJSLibrary; }
            set {
                _outputAsJSLibrary = value;
                RaisePropertyChange("OutputAsJSLibrary");
            }
        }

        private bool _outputDAL;

        public bool OutputDAL
        {
            get { return _outputDAL; }
            set { _outputDAL = value; RaisePropertyChange("OutputDAL"); }
        }

        private bool _outputDomain;

        public bool OutputDomain
        {
            get { return _outputDomain; }
            set { _outputDomain = value; RaisePropertyChange("OutputDomain"); }
        }

        public bool OutputCommandSEvents
        {
            get { return _outputDomain; }
            set { _outputDomain = value; RaisePropertyChange("OutputCommandSEvents"); }
        }

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

        
        private DALTypes _dALTypes;
        public DALTypes DALType
        {
            get
            {
                return _dALTypes;
            }
            set
            {
                _dALTypes = value;
                RaisePropertyChange("DALTypes");
            }
        }

        private string _outputFolderPath;
        public string OutputFolderPath
        {
            get
            {
                return _outputFolderPath;
            }
            set
            {
                _outputFolderPath = value;
                RaisePropertyChange("OutputFolderPath");
            }
        }



    }

}
