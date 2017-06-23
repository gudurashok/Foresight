using System;
using Scalable.Shared.Common;

namespace Scalable.Shared.Domain
{
    public class Identity
    {
        private string _id;

        public Identity(string id)
        {
            setId(id);
        }

        private void setId(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ValidationException("Incorrect identity");

            _id = id;
        }

        public static Identity GetNewIdentity()
        {
            //return new Identity("1"); // Generate from ...
            throw new NotImplementedException();
        }
    }
}
