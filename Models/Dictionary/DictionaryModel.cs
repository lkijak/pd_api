using System;
using System.Collections.Generic;

namespace pd_api.Models.Dictionary
{
    public class DictionaryModel : BaseModel
    {
        public string Name { get; set; }
        public Dictionary<string, string> Dictionary { get; set; } = new Dictionary<string, string>();
    }
}
