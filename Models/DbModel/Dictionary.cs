using System.Collections.Generic;

namespace pd_api.Models.DbModel
{
    public class Dictionary : BaseModel
    {
        public string Name { get; set; }
        public Dictionary<string, string> DictionaryData { get; set; } = new Dictionary<string, string>();
    }
}
