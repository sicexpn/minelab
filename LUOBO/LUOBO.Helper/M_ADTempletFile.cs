using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Helper
{
    public class M_ADTempletFile
    {
        public String File_Name { get; set; }

        public String File_Note { get; set; }

        public String File_Url { get; set; }

        public Boolean isPortal { get; set; }

        public List<M_ADTempletUnit> File_Templet { get; set; }

        public List<M_ADContentItem> File_Content { get; set; }
    }
}
