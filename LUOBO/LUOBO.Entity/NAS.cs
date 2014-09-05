using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LUOBO.Entity
{
    public class NAS
    {
        public Int32 Id { get; set; }
        /// <summary>
        /// Nas name
        /// </summary>
        public String NasName { get; set; }
        public String ShortName { get; set; }

        public String Type { get; set; }
        public Int32 Ports { get; set; }
        /// <summary>
        /// password
        /// </summary>
        public String Secret { get; set; }
        /// <summary>
        /// server
        /// </summary>
        public String Server { get; set; }
        public String Community { get; set; }
        public String Description { get; set; }

    }
}
