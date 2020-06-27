using System;
using System.DirectoryServices.Protocols;
using System.Net;

namespace DocTrack.Web.Service
{
    public static class ActiveDirectoryService
    {
        public static bool Authenticate(string username, string password)
        {
            var credential = new NetworkCredential(username, password);
            var server = new LdapDirectoryIdentifier("ptadaro.com:389");

            var conn = new LdapConnection(server, credential);

            try
            {
                conn.Bind();
            }
            catch (Exception)
            {
                return false;
            }

            conn.Dispose();
            return true;
        }
    }
}
