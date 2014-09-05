using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using LUOBO.Entity;
using LUOBO.BLL;
using System.Data;
using LUOBO.Access;
namespace Luobo.Service.Test
{
	class Program
	{
		static void Main(string[] args)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(Int64));
			dt.Columns.Add("Sex", typeof(string));
			DataRow dr = dt.NewRow();
			dr["ID"] = 1;
			dr["Sex"] = "男";
			dt.Rows.Add(dr);
			DataRow dr1 = dt.NewRow();
			dr1["ID"] = 2;
			dr1["Sex"] = "女";
			dt.Rows.Add(dr1);
			List<TestObject> lists = DataChange<TestObject>.FillModel(dt);
			int i;
			int j = 0;

		}
	}
}
