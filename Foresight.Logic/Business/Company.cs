using ScalableApps.Foresight.Logic.Common;
using ScalableApps.Foresight.Logic.Properties;

namespace ScalableApps.Foresight.Logic.Business
{
    public class Company
    {
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

        public CompanyGroup Group { get; set; }

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
