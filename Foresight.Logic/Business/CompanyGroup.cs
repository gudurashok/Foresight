using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.Business
{
    public class CompanyGroup
    {
        private const string foresightDbPassword = "iScalable@2011";

        public int Id { get; internal set; }
        public string Code { get; set; }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new ValidationException(Resources.NameCannotBeEmpty);

                _name = value;
            }
        }

        public string FilePath { get; set; }
        public static string Password
        {
            get { return foresightDbPassword; }
        }

        public CompanyGroup(string name, string filePath)
        {
            Name = name;
            FilePath = filePath;
        }

        public string CreateDatabaseName()
        {
            return Name.Replace(" ", "").Trim() + "Foresight";
        }

        public static CompanyGroup CreateNewGroup()
        {
            return new CompanyGroup("(New Company Group)", "");
        }

        public override string ToString()
        {
            return Name;
        }

        public bool IsNew()
        {
            return Id == 0;
        }
    }
}
