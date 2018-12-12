using System.Collections.Generic;

namespace Stations.Web.Models
{
    public class ValidationErrorModel
    {
        public List<ValidationErrorMessageModel> Errors { get; set; }
    }

    public class ValidationErrorMessageModel
    {
        public string Name { get; set; }
        public string Error { get; set; }

        public ValidationErrorMessageModel(string name, string error)
        {
            Name = name;
            Error = error;
        }
    }
}
