using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RaceAnalysis.Models
{
    public class KeyValueViewModel
    {

        public List<KeyValuePair<string,string>> KeyValuePairs { get; set; }
    }
    public class KeyValueViewModel_Save
    {
        private KeyValuePair<string, string> _kvp;
        public KeyValuePair<string, string> KVP
        {
            get { return _kvp; }
            set { _kvp = value; }
        }

        public string KVPKey
        {
            get { return _kvp.Key; }
            set { _kvp = new KeyValuePair<string, string>(value, _kvp.Value); }
        }

        public string KVPValue
        {
            get { return _kvp.Value; }
            set { _kvp = new KeyValuePair<string, string>(_kvp.Key, value); }
        }
    }
}