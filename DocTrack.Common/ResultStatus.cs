namespace DocTrack.Common
{
    public class ResultStatus
    {
        #region private variable
        private static object _message = null;
        private static object _data = null;
        #endregion

        static ResultStatus() { }

        public static object Response(string status, object message, object data)
        {
            var Data = new { Status = status, Message = message, Data = data };

            return Data;
        }

        public static object Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public static object Data
        {
            get { return _data; }
            set { _data = value; }
        }
    }
}
