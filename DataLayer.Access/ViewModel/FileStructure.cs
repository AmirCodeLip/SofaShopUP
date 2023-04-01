using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataLayer.Access.ViewModel
{
    public class FileStructure
    {
        public int Id { get; set; }
        [JsonProperty("filename_extensions")]
        public List<string> FileNameExtensions { get; set; }
        private List<List<byte>> hexSignatures;
        public List<List<byte>> HexSignatures { 
            get
            {
                if(hexSignatures == null)
                    hexSignatures = RawHS.Select(c => c.Select(c2 => Convert.FromHexString(c2)[0]).ToList()).ToList();
                return hexSignatures;
            }
        }
        [JsonProperty("hex_signatures")]
        public List<List<string>> RawHS { get; set; }
        [JsonProperty("content_types")]
        public List<string> ContentTypes { get; set; }
        [JsonProperty("type_kind")]
        public string TypeKind { get; set; }
    }
}
