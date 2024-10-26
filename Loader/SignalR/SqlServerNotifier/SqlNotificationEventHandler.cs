using System.Data.SqlClient;

namespace Loader.SqlServerNotifier
{
    public delegate void SqlNotificationEventHandler(object sender, SqlNotificationEventArgs e);
}