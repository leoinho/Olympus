using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.Olympus.Dominio.Util
{
    public class ConsultaSQL
    {
        public SqlParameter CreateDateTimeParameter(string paramName, DateTime? value)
        {
            SqlParameter param;
            if (value != null && value.HasValue && value.Value > DateTime.Parse("1900-01-01 00:00:00"))
                param = new SqlParameter(paramName, value.Value);
            else
                param = new SqlParameter(paramName, DBNull.Value);

            return param;
        }

        public SqlParameter CreateStringParameter(string paramName, String value)
        {
            SqlParameter param;
            if (value != null && value.Trim() != String.Empty)
                param = new SqlParameter(paramName, value);
            else
                param = new SqlParameter(paramName, DBNull.Value);

            return param;
        }

        public SqlParameter CreateIntParameter(string paramName, int? value)
        {
            SqlParameter param;
            if (value != null && value.HasValue && value.Value > 0)
                param = new SqlParameter(paramName, value.Value);
            else
                param = new SqlParameter(paramName, DBNull.Value);

            return param;
        }

        public SqlParameter CreateBigIntParameter(string paramName, Int64? value)
        {
            SqlParameter param;
            if (value != null && value.HasValue && value.Value > 0)
                param = new SqlParameter(paramName, value.Value);
            else
                param = new SqlParameter(paramName, DBNull.Value);

            return param;
        }

        public SqlParameter CreateDecimalZeroParameter(string paramName, decimal? value)
        {
            SqlParameter param;
            if (value != null && value.HasValue && value.Value >= 0)
                param = new SqlParameter(paramName, value.Value);
            else
                param = new SqlParameter(paramName, DBNull.Value);

            return param;
        }

        public SqlParameter CreateBoolParameter(string paramName, Boolean? value)
        {
            SqlParameter param;
            if (value != null)
                param = new SqlParameter(paramName, value.Value);
            else
                param = new SqlParameter(paramName, DBNull.Value);

            return param;
        }

    }
}
