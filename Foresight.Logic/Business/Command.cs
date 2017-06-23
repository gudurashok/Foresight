using System.Collections.Generic;
using System.Linq;
using ScalableApps.Foresight.Logic.Common;

namespace ScalableApps.Foresight.Logic.Business
{
    public class Command
    {
        public int Id { get; set; }
        public int Nr { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UIControlName { get; set; }
        public bool IsActive { get; set; }
        public CommandType Type { get; set; }
        public IList<CommandProp> Properties { private get; set; }

        public string GetPropertyValue(string propertyName)
        {
            var result = Properties.First(p => p.PropName == propertyName);

            if (result == null)
                throw new ValidationException(
                    string.Format("Command property '{0}' not found for command {1}", propertyName, Name));

            return result.PropValue;
        }
    }
}
