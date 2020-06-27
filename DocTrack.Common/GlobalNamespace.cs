using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocTrack.Common
{
    public class GlobalNamespace
    {
        public static string ConnectionString { get; set; } = "DHTConnString";

        public static string Failed { get; set; } = "Invalid data submitted, try again.";

        public static string Unique => "Data {0} is already in the system.";

        public static string Submit => "Data successfully Submitted.";

        public static string Saved => "Data successfully Saved.";

        public static string Created => "Data successfully created.";

        public static string Updated => "Data successfully updated.";

        public static string Deleted => "Data successfully deleted.";

        public static string NotFound => "Data not found.";

        public static string LoginAttemptFailed => "Invalid username or password, please try again.";
    }
}
